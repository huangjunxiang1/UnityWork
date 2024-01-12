#if FairyGUI
using FairyGUI;
#endif
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public static partial class SAsset
    {
        public const string Directory = "Assets/Res/";

        static readonly SAssetPrefabLoader prefabLoader = new();
        //static readonly AssetBaseLoader counterLoader = new AssetCounterLoader();
        static readonly SAssetBaseLoader primitiveLoader = new SAssetPrimitiveLoader();
        //static readonly AssetBaseLoader copyLoader = new AssetCopyLoader();

        public static GameObject LoadGameObject(string url, ReleaseMode releaseMode = ReleaseMode.Destroy)
        {
            GameObject g = (GameObject)prefabLoader.Load(url);
            UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
            r.url = url;
            r.isFromLoad = true;
            r.mode = releaseMode;
            return g;
        }
        public static STask<GameObject> LoadGameObjectAsync(string url, ReleaseMode releaseMode = ReleaseMode.Destroy)
        {
            STask<GameObject> task = new();
            async void run()
            {
                GameObject g = (GameObject)await prefabLoader.LoadAsync(url);
                UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
                r.url = url;
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
        public static STask<SceneInstance> LoadScene(string url, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return Addressables.LoadSceneAsync(Directory + url, mode).AsTask();
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
            return (T)primitiveLoader.Load(url);
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
                T ret = (T)await primitiveLoader.LoadAsync(url);
                if (task.Disposed)
                {
                    primitiveLoader.Release(ret);
                    return;
                }
                task.TrySetResult(ret);
            }
            STimer.Add(0, 1, run);
            return task.MakeAutoCancel();
        }

        public static void Release(UnityEngine.Object target)
        {
            if (!target)
            {
                Loger.Error("Asset is null");
                return;
            }
            if (target is GameObject g)
            {

#if DebugEnable
                //debug模式检查一次Release的是不是加载对象
                if (!g.TryGetComponent<UrlRef>(out var r))
                {
                    Loger.Error($"不是从资源加载的对象 target={target}");
                    GameObject.DestroyImmediate(target);
                    return;
                }
#endif
                UrlRef[] rs = g.GetComponentsInChildren<UrlRef>();
                for (int i = rs.Length - 1; i >= 0; i--)
                    ReleaseGameObject(rs[i], false);
            }
            else
                primitiveLoader.Release(target);
        }
        public static void ReleaseScene(SceneInstance scene)
        {
            Addressables.UnloadSceneAsync(scene);
        }
        public static void TryReleaseGameObject(GameObject target)
        {
            if (!target)
            {
                Loger.Error("Asset is null");
                return;
            }
            if (!target.TryGetComponent<UrlRef>(out var r))
            {
                GameObject.DestroyImmediate(target);
                return;
            }
            ReleaseGameObject(r, true);
        }
        static void ReleaseGameObject(UrlRef r, bool destroyIfIsNotLoad = true)
        {
            switch (r.mode)
            {
                case ReleaseMode.None:
                case ReleaseMode.Destroy:
                    if (r.isFromLoad)
                        prefabLoader.Release(r.gameObject);
                    else if (destroyIfIsNotLoad)
                        GameObject.DestroyImmediate(r.gameObject);
                    break;
                case ReleaseMode.PutToPool:
                    prefabLoader.ReleaseToPool(r.gameObject, r.url);
                    break;
                default:
                    break;
            }
        }
        public static void AddTextureRef(GameObject target, Texture texture)
        {
            TextureRef tr = target.GetComponent<TextureRef>() ?? target.AddComponent<TextureRef>();
            if (tr.texture)
                primitiveLoader.Release(tr.texture);
            tr.texture = texture;
        }
        public static void ReleaseTextureRef(GameObject target)
        {
            if (!target)
            {
                Loger.Error("Asset is null");
                return;
            }
            TextureRef[] rs = target.GetComponentsInChildren<TextureRef>();
            int len = rs.Length;
            for (int i = 0; i < len; i++)
            {
                TextureRef r = rs[i];
                if (r.texture)
                    primitiveLoader.Release(r.texture);
            }
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
            public string url;

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
