using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using FairyGUI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Game;
using Unity.Entities;

namespace Main
{
    public static partial class AssetLoad
    {
        public const string Directory = "Assets/Res/";

        static readonly AssetPrefabLoader prefabLoader = new AssetPrefabLoader();
        static readonly AssetBaseLoader counterLoader = new AssetCounterLoader();
        static readonly AssetBaseLoader primitiveLoader = new AssetPrimitiveLoader();
        static readonly AssetBaseLoader copyLoader = new AssetCopyLoader();

        public static GameObject LoadGameObject(string url, ReleaseMode releaseMode = ReleaseMode.Release)
        {
            GameObject g = (GameObject)prefabLoader.Load(url);
            UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
            r.url = url;
            r.mode = releaseMode;
            return g;
        }
        public static async TaskAwaiter<GameObject> LoadGameObjectAsync(string url, ReleaseMode releaseMode = ReleaseMode.Release)
        {
            GameObject g = (GameObject)await prefabLoader.LoadAsync(url);
            UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
            r.url = url;
            r.mode = releaseMode;
            return g;
        }
        public static async TaskAwaiter<Entity> LoadEntityAsync(string url)
        {
            GameObject g = (GameObject)await prefabLoader.LoadAsync(url);
            Entity e = await GameObjectToEntityConversion.ConverToEntity(g);
            prefabLoader.Release(g);
            return e;
        }
        public static async TaskAwaiter<GameObject> LoadGameObjectAsync(string url, TaskAwaiter<UnityEngine.Object> task, ReleaseMode releaseMode = ReleaseMode.Release)
        {
            GameObject g = (GameObject)await prefabLoader.LoadAsync(url, task);
            UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
            r.url = url;
            r.mode = releaseMode;
            return g;
        }
        public static async TaskAwaiter<GameObject> LoadGameObjectAsync(string url, TaskAwaiterCreater creater, ReleaseMode releaseMode = ReleaseMode.Release)
        {
            GameObject g = (GameObject)await prefabLoader.LoadAsync(url, creater);
            UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
            r.url = url;
            r.mode = releaseMode;
            return g;
        }
        public static async TaskAwaiter<Entity> LoadEntityAsync(string url, TaskAwaiterCreater creater)
        {
            GameObject g = (GameObject)await prefabLoader.LoadAsync(url, creater);
            Entity e = await GameObjectToEntityConversion.ConverToEntity(g, creater.Create<Entity>());
            prefabLoader.Release(g);
            return e;
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
            if (t == typeof(Texture))
                return (T)counterLoader.Load(url);
            return (T)primitiveLoader.Load(url);
        }
        public static async TaskAwaiter<T> LoadAsync<T>(string url) where T : UnityEngine.Object
        {
            Type t = typeof(T);
#if DebugEnable
            if (t == typeof(GameObject))
            {
                Loger.Error("GameObject 不使用这个函数加载");
                return default;
            }
#endif
            if (t == typeof(Texture))
                return (T)await counterLoader.LoadAsync(url);
            return (T)await primitiveLoader.LoadAsync(url);
        }
        public static async TaskAwaiter<T> LoadAsync<T>(string url, TaskAwaiter<UnityEngine.Object> task) where T : UnityEngine.Object
        {
            Type t = typeof(T);
#if DebugEnable
            if (t == typeof(GameObject))
            {
                Loger.Error("GameObject 不使用这个函数加载");
                return default;
            }
#endif
            if (t == typeof(Texture))
                return (T)await counterLoader.LoadAsync(url, task);
            return (T)await primitiveLoader.LoadAsync(url, task);
        }
        public static async TaskAwaiter<T> LoadAsync<T>(string url, TaskAwaiterCreater creater) where T : UnityEngine.Object
        {
            Type t = typeof(T);
#if DebugEnable
            if (t == typeof(GameObject))
            {
                Loger.Error("GameObject 不使用这个函数加载");
                return default;
            }
#endif
            if (t == typeof(Texture))
                return (T)await counterLoader.LoadAsync(url, creater);
            return (T)await primitiveLoader.LoadAsync(url, creater);
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
                UrlRef r = g.GetComponent<UrlRef>();
                if (r == null)
                {
                    GameObject.DestroyImmediate(target);
                    return;
                }
                //GetComponentsInChildren这里获取的  包含他自己  所以用if else
                if (r.hasChange)
                {
                    UrlRef[] rs = r.transform.GetComponentsInChildren<UrlRef>();
                    for (int i = rs.Length - 1; i >= 0; i--)
                        ReleaseGameObject(rs[i]);
                }
                else
                    ReleaseGameObject(r);
            }
            else if (target is Texture)
                counterLoader.Release(target);
            else
                primitiveLoader.Release(target);
        }
        static void ReleaseGameObject(UrlRef r)
        {
            switch (r.mode)
            {
                case ReleaseMode.None:
                case ReleaseMode.Release:
                    prefabLoader.Release(r.gameObject);
                    break;
                case ReleaseMode.ReleaseToPool:
                    r.hasChange = false;
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
                counterLoader.Release(tr.texture);
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
                    counterLoader.Release(r.texture);
            }
        }


        class UrlRef : MonoBehaviour
        {
            [NonSerialized]
            public string url;
            [NonSerialized]
            public ReleaseMode mode;

            public bool hasChange;

            void OnTransformChildrenChanged()
            {
                hasChange = true;
            }
        }
        class TextureRef : MonoBehaviour
        {
            [NonSerialized]
            public Texture texture;
        }
    }
}
