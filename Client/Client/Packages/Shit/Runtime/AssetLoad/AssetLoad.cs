using System;
using UnityEngine;

namespace Main
{
    public static partial class AssetLoad
    {
        public const string Directory = "Assets/Res/";

        static readonly AssetPrefabLoader prefabLoader = new AssetPrefabLoader();
        //static readonly AssetBaseLoader counterLoader = new AssetCounterLoader();
        static readonly AssetBaseLoader primitiveLoader = new AssetPrimitiveLoader();
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
        public static TaskAwaiter<GameObject> LoadGameObjectAsync(string url, ReleaseMode releaseMode = ReleaseMode.Destroy)
        {
            TaskAwaiter<GameObject> task = new TaskAwaiter<GameObject>();
            async void run()
            {
                GameObject g = (GameObject)await prefabLoader.LoadAsync(url);
                UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
                r.url = url;
                r.isFromLoad = true;
                r.mode = releaseMode;
                if (task.IsDisposed)
                {
                    ReleaseGameObject(r);
                    return;
                }
                task.TrySetResult(g);
            }
            run();
            return task.MakeAutoCancel();
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
        public static TaskAwaiter<T> LoadAsync<T>(string url) where T : UnityEngine.Object
        {
            Type t = typeof(T);
#if DebugEnable
            if (t == typeof(GameObject))
            {
                Loger.Error("GameObject 不使用这个函数加载");
                return default;
            }
#endif
            TaskAwaiter<T> task = new();
            async void run()
            {
                T ret = (T)await primitiveLoader.LoadAsync(url);
                if (task.IsDisposed)
                {
                    primitiveLoader.Release(ret);
                    return;
                }
                task.TrySetResult(ret);
            }
            run();
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
            public Texture texture;

            private void Awake()
            {
                if (!texture)
                {
                    UnityEngine.UI.RawImage ri = gameObject.GetComponent<UnityEngine.UI.RawImage>();
                    if (ri)
                        ri.texture = Texture2D.whiteTexture;
                }
            }
        }
    }
}
