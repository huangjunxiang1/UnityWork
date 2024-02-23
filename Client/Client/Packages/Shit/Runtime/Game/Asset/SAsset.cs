using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if FairyGUI
using FairyGUI;
#endif

namespace Game
{
    public static class SAsset
    {
        static SAsset()
        {
            if (Application.isPlaying)
            {
                _poolRoot = new GameObject("PoolRoot");
                GameObject.DontDestroyOnLoad(_poolRoot);
                _poolRoot.SetActive(false);
            }
        }

#if Addressables
        public static SAssetLoader Loader = new AddressablesLoader();
#else
        public static SAssetLoader Loader;
#endif

        public static string Directory = "Assets/Res/";

        static readonly GameObject _poolRoot;
        static readonly Dictionary<string, Queue<GameObject>> _pool = new(50);

        public static GameObject LoadGameObject(string url, ReleaseMode releaseMode = ReleaseMode.PutToPool)
        {
            string path = Directory + url;
            GameObject g;
            if (_pool.TryGetValue(path, out var pool) && pool.Count > 0)
            {
                g = pool.Dequeue();
                g.transform.parent = GameM.World.GameRoot.transform;
            }
            else
                g = Loader.LoadGameObject(path);
            UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
            r.fullPath = path;
            r.isFromLoad = true;
            r.mode = releaseMode;
            return g;
        }
        public static STask<GameObject> LoadGameObjectAsync(string url, ReleaseMode releaseMode = ReleaseMode.PutToPool)
        {
            STask<GameObject> task = new();
            async void run()
            {
                string path = Directory + url;
                GameObject g;
                if (_pool.TryGetValue(path, out var pool) && pool.Count > 0)
                {
                    g = pool.Dequeue();
                    g.transform.parent = GameM.World.GameRoot.transform;
                }
                else
                    g = await Loader.LoadGameObjectAsync(path);
                UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
                r.fullPath = path;
                r.isFromLoad = true;
                r.mode = releaseMode;
                if (task.Disposed)
                {
                    ReleaseGameObject(r);
                    return;
                }
                task.TrySetResult(g);
            }
            STimer.Add(0, 1, run);
            return task.MakeAutoCancel();
        }
        public static Scene LoadScene(string url, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return Loader.LoadScene(Directory + url, mode);
        }
        public static STask<Scene> LoadSceneAsync(string url, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return Loader.LoadSceneAsync(Directory + url, mode);
        }
        public static T Load<T>(string url) where T : UnityEngine.Object
        {
            Type t = typeof(T);
#if DebugEnable
            if (t == typeof(GameObject))
            {
                Loger.Error("GameObject 不使用这个函数加载");
                return default;
            }
#endif
            return (T)Loader.LoadObject(Directory + url);
        }
        public static STask<T> LoadAsync<T>(string url) where T : UnityEngine.Object
        {
            Type t = typeof(T);
#if DebugEnable
            if (t == typeof(GameObject))
            {
                Loger.Error("GameObject 不使用这个函数加载");
                return default;
            }
#endif
            STask<T> task = new();
            async void run()
            {
                T ret = (T)await Loader.LoadObjectAsync(Directory + url);
                if (task.Disposed)
                {
                    Loader.Release(ret);
                    return;
                }
                task.TrySetResult(ret);
            }
            STimer.Add(0, 1, run);
            return task.MakeAutoCancel();
        }

        public static void Release(UnityEngine.Object target, bool check = true)
        {
            if (!target)
            {
                Loger.Error("Asset is null");
                return;
            }
            if (target is GameObject g)
            {
                ReleaseTextureRef(g);
                var list = ObjectPool<List<UrlRef>>.Get();
                g.GetComponentsInChildren(list);
                for (int i = list.Count - 1; i >= 0; i--)
                    ReleaseGameObject(list[i]);
                list.Clear();
                ObjectPool<List<UrlRef>>.Return(list);

                //debug模式检查一次Release的是不是加载对象
                if (g && !g.GetComponent<UrlRef>())
                {
                    if (check)
                        Loger.Error($"不是从资源加载的对象 target={target}");
                    GameObject.DestroyImmediate(target);
                }
            }
            else
                Loader.Release(target);
        }
        public static void Release(Scene scene)
        {
            Loader.Release(scene);
        }
        public static void ReleasePoolsGameObjects()
        {
            foreach (var item in _pool.Values)
            {
                while (item.TryDequeue(out var go))
                    Loader.Release(go);
            }
            _pool.Clear();
        }
        static void ReleaseGameObject(UrlRef r)
        {
            switch (r.mode)
            {
                case ReleaseMode.None:
                case ReleaseMode.Destroy:
                    if (r.isFromLoad)
                        Loader.Release(r.gameObject);
                    else
                        GameObject.DestroyImmediate(r.gameObject);
                    break;
                case ReleaseMode.PutToPool:
#if DebugEnable
                    if (string.IsNullOrEmpty(r.fullPath))
                    {
                        Loger.Error("url is empty");
                        return;
                    }
#endif
                    if (!_pool.TryGetValue(r.fullPath, out var lst))
                    {
                        lst = new();
                        _pool[r.fullPath] = lst;
                    }
                    lst.Enqueue(r.gameObject);
                    r.gameObject.transform.SetParent(_poolRoot.transform);
                    break;
                default:
                    break;
            }
        }
        public static void AddTextureRef(GameObject target, Texture texture)
        {
            TextureRef tr = target.GetComponent<TextureRef>() ?? target.AddComponent<TextureRef>();
            if (tr.texture)
                Loader.Release(tr.texture);
            tr.texture = texture;
        }
        public static void ReleaseTextureRef(GameObject target)
        {
            if (!target)
            {
                Loger.Error("Asset is null");
                return;
            }   
            var list = ObjectPool<List<TextureRef>>.Get();
            target.GetComponentsInChildren(list);
            int len = list.Count;
            for (int i = 0; i < len; i++)
            {
                TextureRef r = list[i];
                if (r.texture)
                    Loader.Release(r.texture);
            }
            list.Clear();
            ObjectPool<List<TextureRef>>.Return(list);
        }

        public static async STask SetTexture(this RawImage ri, string url)
        {
            TextureRef tr = ri.gameObject.GetComponent<TextureRef>() ?? ri.gameObject.AddComponent<TextureRef>();
            string s = tr.url;
            tr.url = url;
            if (string.IsNullOrEmpty(url))
            {
                if (!string.IsNullOrEmpty(s))
                    Release(tr.texture);
                ri.texture = null;
                return;
            }
            var tex = await LoadAsync<Texture>(url);
            if (!ri)
            {
                Release(tex);
                return;
            }
            if (tr.url != url)
            {
                Release(tex);
                return;
            }
            AddTextureRef(ri.gameObject, tex);
            ri.texture = tex;
        }
#if FairyGUI
        public static async STask SetTexture(this GLoader loader, string url)
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

        class UrlRef : MonoBehaviour
        {
            public ReleaseMode mode;
            public string fullPath;

            [NonSerialized]
            public bool isFromLoad;
        }
        class TextureRef : MonoBehaviour
        {
            [NonSerialized]
            public string url;
            [NonSerialized]
            public Texture texture;

            private void Awake()
            {
                if (!texture)
                {
                    UnityEngine.UI.RawImage ri = gameObject.GetComponent<UnityEngine.UI.RawImage>();
                    if (ri)
                        ri.texture = null;
                }
            }
        }
    }
}
