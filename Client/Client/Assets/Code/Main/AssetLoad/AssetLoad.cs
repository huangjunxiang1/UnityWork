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

namespace Main
{
    public static partial class AssetLoad
    {
        public const string Directory = "Assets/Res/";

        static readonly AssetPrefabLoader prefabLoader = new AssetPrefabLoader();
        static readonly AssetBaseLoader counterLoader = new AssetCounterLoader();
        static readonly AssetBaseLoader primitiveLoader = new AssetPrimitiveLoader();
        static readonly AssetBaseLoader copyLoader = new AssetCopyLoader();

        public static T Load<T>(string url) where T : UnityEngine.Object
        {
            Type t = typeof(T);
            if (t == typeof(GameObject))
            {
                GameObject g = (GameObject)prefabLoader.Load(url);
                UrlRef aref = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
                aref.url = url;
                return g as T;
            }
            else if (t == typeof(Texture))
                return (T)counterLoader.Load(url);
            return (T)primitiveLoader.Load(url);
        }
        public static async TaskAwaiter<T> LoadAsync<T>(string url) where T : UnityEngine.Object
        {
            Type t = typeof(T);
            if (t == typeof(GameObject))
            {
                GameObject g = (GameObject)await prefabLoader.LoadAsync(url);
                UrlRef aref = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
                aref.url = url;
                return g as T;
            }
            else if (t == typeof(Texture))
                return (T)await counterLoader.LoadAsync(url);
            return (T)await primitiveLoader.LoadAsync(url);
        }
        public static async TaskAwaiter<T> LoadAsync<T>(string url, TaskAwaiter<UnityEngine.Object> task) where T : UnityEngine.Object
        {
            Type t = typeof(T);
            if (t == typeof(GameObject))
            {
                GameObject g = (GameObject)await prefabLoader.LoadAsync(url, task);
                UrlRef aref = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
                aref.url = url;
                return g as T;
            }
            else if (t == typeof(Texture))
                return (T)await counterLoader.LoadAsync(url, task);
            return (T)await primitiveLoader.LoadAsync(url, task);
        }
        public static async TaskAwaiter<T> LoadAsync<T>(string url, TaskAwaiterCreater creater) where T : UnityEngine.Object
        {
            Type t = typeof(T);
            if (t == typeof(GameObject))
            {
                GameObject g = (GameObject)await prefabLoader.LoadAsync(url, creater);
                UrlRef aref = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
                aref.url = url;
                return g as T;
            }
            else if (t == typeof(Texture))
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
            if (target is GameObject)
                prefabLoader.Release(target);
            else if (target is Texture)
                counterLoader.Release(target);
            else
                primitiveLoader.Release(target);
        }
        public static void ReleaseToPool(GameObject target)
        {
            if (!target)
            {
                Loger.Error("Asset is null");
                return;
            }
            UrlRef r = target.GetComponent<UrlRef>();
            if (r == null)
            {
                Loger.Error("没有AssetRef组件 " + target);
                GameObject.Destroy(target);
                return;
            }
            prefabLoader.ReleaseToPool(target, r.url);
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

            List<UrlRef> childs = null;
            UrlRef lastParent;

            private void OnTransformParentChanged()
            {
                UrlRef parent = this.transform.parent?.GetComponentInParent<UrlRef>();
                if (lastParent == parent)
                    return;
                if (lastParent)
                    lastParent.childs.Remove(this);

                if (parent)
                {
                    if (parent.childs == null)
                        parent.childs = new List<UrlRef>();
                    parent.childs.Add(this);
                    lastParent = parent;
                }
            }
        }
        class TextureRef : MonoBehaviour
        {
            [NonSerialized]
            public Texture texture;
        }
    }
}
