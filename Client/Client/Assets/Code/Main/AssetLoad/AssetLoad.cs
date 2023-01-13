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
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

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
        public static async TaskAwaiter<GameObject> LoadGameObjectAsync(string url, ReleaseMode releaseMode = ReleaseMode.Destroy)
        {
            GameObject g = (GameObject)await prefabLoader.LoadAsync(url);
            UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
            r.url = url;
            r.isFromLoad = true;
            r.mode = releaseMode;
            return g;
        }
        public static async TaskAwaiter<Entity> LoadEntityAsync(string url)
        {
            GameObject g = (GameObject)await prefabLoader.LoadAsync(url);
            var mgr = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity e = mgr.CreateEntity();
            Renderer r = g.GetComponent<Renderer>();
            MeshFilter mf = g.GetComponent<MeshFilter>();
            RenderMeshUtility.AddComponents(e, mgr, new RenderMeshDescription(r), new RenderMeshArray(new[] { r.sharedMaterial }, new[] { mf.sharedMesh }), MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

            mgr.AddComponentData(e, new LocalToWorld() { Value = float4x4.TRS(float3.zero, g.transform.rotation, g.transform.lossyScale) });
            prefabLoader.Release(g);
            return e;
        }
        public static async TaskAwaiter<GameObject> LoadGameObjectAsync(string url, TaskAwaiter<UnityEngine.Object> task, ReleaseMode releaseMode = ReleaseMode.Destroy)
        {
            GameObject g = (GameObject)await prefabLoader.LoadAsync(url, task);
            UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
            r.url = url;
            r.isFromLoad = true;
            r.mode = releaseMode;
            return g;
        }
        public static async TaskAwaiter<GameObject> LoadGameObjectAsync(string url, TaskAwaiterCreater creater, ReleaseMode releaseMode = ReleaseMode.Destroy)
        {
            GameObject g = (GameObject)await prefabLoader.LoadAsync(url, creater);
            UrlRef r = g.GetComponent<UrlRef>() ?? g.AddComponent<UrlRef>();
            r.url = url;
            r.isFromLoad = true;
            r.mode = releaseMode;
            return g;
        }
        public static async TaskAwaiter<Entity> LoadEntityAsync(string url, TaskAwaiterCreater creater)
        {
            GameObject g = (GameObject)await prefabLoader.LoadAsync(url, creater);
            var mgr = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity e = mgr.CreateEntity();
            Renderer r = g.GetComponent<Renderer>();
            MeshFilter mf = g.GetComponent<MeshFilter>();
            RenderMeshUtility.AddComponents(e, mgr, new RenderMeshDescription(r), new RenderMeshArray(new[] { r.sharedMaterial }, new[] { mf.sharedMesh }), MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

            mgr.AddComponentData(e, new LocalToWorld() { Value = float4x4.TRS(float3.zero, g.transform.rotation, g.transform.lossyScale) });
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

#if DebugEnable
                //debug模式检查一次Release的是不是加载对象
                UrlRef r = g.GetComponent<UrlRef>();
                if (r == null)
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
            UrlRef r = target.GetComponent<UrlRef>();
            if (r == null)
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
       
        public static void SetEmptyTextureIfIsNotFromLoad(GameObject target)
        {
#if DebugEnable
            //debug模式重置texture 以方便查问题
            TextureRef[] rs = target.GetComponentsInChildren<TextureRef>();
            int len = rs.Length;
            for (int i = 0; i < len; i++)
            {
                TextureRef r = rs[i];
                UnityEngine.UI.RawImage ri = r.gameObject.GetComponent<UnityEngine.UI.RawImage>();
                if (!r.texture && ri && ri.texture)
                    ri.texture = Texture2D.whiteTexture;
            }
#endif
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
        }
    }
}
