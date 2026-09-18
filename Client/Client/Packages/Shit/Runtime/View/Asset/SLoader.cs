using FairyGUI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YooAsset;

namespace Game
{
    public enum ReleaseMode
    {
        None,
        Destroy,
        PutToPool,
    }
    public class SLoader
    {
        class UrlRef : MonoBehaviour
        {
            public ReleaseMode mode;
            public string fullPath;

            [NonSerialized]
            public bool isFromLoad;
            [NonSerialized]
            public AssetHandle handle;
            [NonSerialized]
            public SLoader loader;
        }
        static SLoader()
        {
            if (Application.isPlaying)
            {
                _poolRoot = new GameObject("PoolRoot");
                GameObject.DontDestroyOnLoad(_poolRoot);
                _poolRoot.SetActive(false);
            }
        }
        class Item
        {
            public string url;
            public AssetHandle asset;
            public int num;
        }
        public SLoader(string name, ResourcePackage package)
        {
            package ??= DefaultPackage;
            _name = name;
            this.package = package;
            if (Application.isPlaying)
            {
                _loaderRoot = new GameObject(_name);
                _loaderRoot.transform.parent = _poolRoot.transform;
            }
        }

        static GameObject _poolRoot;

        string _name;
        HashSet<AssetHandle> _handles = new();
        GameObject _loaderRoot;
        Dictionary<string, Queue<GameObject>> _pool = new(50);
        Dictionary<string, Item> _urlMap = new();
        Dictionary<UnityEngine.Object, Item> _objMap = new();

        public static ResourcePackage DefaultPackage;
        public ResourcePackage package { get; private set; }
        public bool isDisposed { get; private set; } = false;

        public void SetPackage(ResourcePackage package)
        {
            this.package = package;
        }
        public void Dispose()
        {
            isDisposed = true;
            foreach (var item in this._handles)
                item.Dispose();
            GameObject.Destroy(_loaderRoot);
        }

        public GameObject LoadGameObject(string url, ReleaseMode releaseMode = ReleaseMode.PutToPool)
        {
            GameObject g;
            if (_pool.TryGetValue(url, out var pool) && pool.Count > 0)
            {
                g = pool.Dequeue();
            }
            else
            {
                var handle = package.LoadAssetSync<GameObject>(url);
                _handles.Add(handle);
                handle.WaitForAsyncComplete();
                g = handle.InstantiateSync();
                UrlRef r = g.AddComponent<UrlRef>();
                r.fullPath = url;
                r.isFromLoad = true;
                r.handle = handle;
                r.loader = this;
            }
            g.GetComponent<UrlRef>().mode = releaseMode;
            return g;
        }
        public async STask<GameObject> LoadGameObjectAsync(string url, ReleaseMode releaseMode = ReleaseMode.PutToPool)
        {
            GameObject g;
            if (_pool.TryGetValue(url, out var pool) && pool.Count > 0)
            {
                g = pool.Dequeue();
            }
            else
            {
                var handle = package.LoadAssetAsync<GameObject>(url);
                _handles.Add(handle);
                var instantiate = handle.InstantiateAsync();
                await instantiate;
                g = instantiate.Result;
                UrlRef r = g.AddComponent<UrlRef>();
                r.fullPath = url;
                r.isFromLoad = true;
                r.handle = handle;
                r.loader = this;
            }
            g.GetComponent<UrlRef>().mode = releaseMode;
            return g;
        }
        public UnityEngine.SceneManagement.Scene LoadScene(string url, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return package.LoadSceneSync(url, mode).SceneObject;
        }
        public async STask<UnityEngine.SceneManagement.Scene> LoadSceneAsync(string url, LoadSceneMode mode = LoadSceneMode.Single)
        {
            var handle = package.LoadSceneAsync(url, mode);
            await handle;
            return handle.SceneObject;
        }
        public T Load<T>(string url) where T : UnityEngine.Object
        {
            if (_urlMap.TryGetValue(url, out var v2))
            {
                v2.asset.WaitForAsyncComplete();
                v2.num++;
                return v2.asset.AssetObject as T;
            }

            Type t = typeof(T);
#if DebugEnable
            if (t == typeof(GameObject))
            {
                Loger.Error("GameObject Cannot Use This Method");
                return default;
            }
#endif
            var handle = package.LoadAssetAsync(url);
            _handles.Add(handle);
            handle.WaitForAsyncComplete();
            _urlMap[url] = _objMap[handle.AssetObject] = new() { url = url, asset = handle, num = 1 };
            return handle.AssetObject as T;
        }
        public async STask<T> LoadAsync<T>(string url) where T : UnityEngine.Object
        {
            if (_urlMap.TryGetValue(url, out var v2))
            {
                v2.num++;
                await v2.asset;
                return v2.asset.AssetObject as T;
            }

            Type t = typeof(T);
#if DebugEnable
            if (t == typeof(GameObject))
            {
                Loger.Error("GameObject Cannot Use This Method");
                return default;
            }
#endif
            var handle = package.LoadAssetAsync(url);
            _handles.Add(handle);
            v2 = _urlMap[url] = new() { url = url, asset = handle, num = 1 };
            await handle;
            _objMap[handle.AssetObject] = v2;
            return handle.AssetObject as T;
        }

        Dictionary<object, string> objToUrl = new();
        public async STask SetTexture(RawImage img, string url)
        {
            objToUrl.TryGetValue(img, out var old);
            if (old == url)
                return;
            if (string.IsNullOrEmpty(url))
            {
                objToUrl.Remove(img);
                Release(img.texture);
                img.texture = null;
                return;
            }
            objToUrl[img] = url;
            var tex = await LoadAsync<Texture>(url);
            if (!img)
            {
                objToUrl.Remove(img);
                Release(tex);
                return;
            }
            objToUrl.TryGetValue(img, out var now);
            if (now != url)
            {
                Release(tex);
                return;
            }
            if (old != null && img.texture)
                Release(img.texture);
            img.texture = tex;
        }
#if FairyGUI
        public async STask SetTexture(GLoader loader, string url)
        {
            loader.data = url;
            if (string.IsNullOrEmpty(url))
            {
                loader.texture = NTexture.Empty;
                return;
            }
            var tex = await LoadAsync<Texture>(url);
            if (loader.isDisposed)
            {
                Release(tex);
                return;
            }
            if ((string)loader.data != url)
            {
                Release(tex);
                return;
            }
            var nt = new NTexture(tex);
            nt.destroyMethod = DestroyMethod.Custom;
            nt.onRelease += v => v.Dispose();
            loader.texture = nt;
        }
#endif

        public void Release(UnityEngine.Object obj, bool check = true)
        {
            if (!obj)
            {
                Loger.Error("Asset is null");
                return;
            }
            if (obj is GameObject g)
            {
                var list = ObjectPool.Get<List<UrlRef>>();
                g.GetComponentsInChildren(list);
                for (int i = list.Count - 1; i >= 0; i--)
                    ReleaseGameObject(list[i]);
                list.Clear();
                ObjectPool.Return(list);

                //debug模式检查一次Release的是不是加载对象
                if (g && !g.GetComponent<UrlRef>())
                {
                    if (check)
                        Loger.Error($"不是从资源加载的对象 obj={obj}");
                    GameObject.DestroyImmediate(obj);
                }
            }
            else
            {
                if (_objMap.TryGetValue(obj,out var v2))
                {
                    v2.num--;
                    if (v2.num == 0)
                    {
                        v2.asset.Dispose();
                        _objMap.Remove(obj);
                        _urlMap.Remove(v2.url);
                        _handles.Remove(v2.asset);
                    }
                }
            }
        }
        void ReleaseGameObject(UrlRef r)
        {
            if (r.loader != this)
            {
                Loger.Error($"{r.gameObject.name} target loader is not this");
                return;
            }
            switch (r.mode)
            {
                case ReleaseMode.None:
                case ReleaseMode.Destroy:
                    if (r.isFromLoad)
                    {
                        r.handle.Dispose();
                        _handles.Remove(r.handle);
                    }
                    GameObject.DestroyImmediate(r.gameObject);
                    break;
                case ReleaseMode.PutToPool:
                    if (!r.isFromLoad)
                    {
                        GameObject.Destroy(r.gameObject);
                        return;
                    }

                    if (string.IsNullOrEmpty(r.fullPath))
                    {
                        Loger.Error("url is empty");
                        return;
                    }

                    if (!_pool.TryGetValue(r.fullPath, out var lst))
                        _pool[r.fullPath] = lst = new();
                    lst.Enqueue(r.gameObject);
                    r.gameObject.transform.SetParent(_loaderRoot.transform);
                    break;
                default:
                    break;
            }
        }
    }
}
