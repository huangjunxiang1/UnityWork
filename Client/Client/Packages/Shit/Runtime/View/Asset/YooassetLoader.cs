using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

#if Yooasset
using YooAsset;
namespace Game
{
    public class YooassetLoader : SAssetLoader
    {
        Dictionary<UnityEngine.GameObject, AssetHandle> gobjMap = new();
        Dictionary<UnityEngine.Object, (AssetHandle, int)> objMap = new();

        public ResourcePackage Package { get; private set; }

        public void LoadPackage(string packageName)
        {
            Package = YooAssets.TryGetPackage(packageName) ?? YooAssets.CreatePackage(packageName);
        }
        public override GameObject LoadGameObject(string url)
        {
            var handle = Package.LoadAssetSync<GameObject>(url);
            var go = handle.InstantiateSync();
            gobjMap[go] = handle;
            return go;
        }

        public override async STask<GameObject> LoadGameObjectAsync(string url)
        {
            var handle = Package.LoadAssetAsync<GameObject>(url);
            var op = handle.InstantiateAsync();
            await op.Task;
            gobjMap[op.Result] = handle;
            return op.Result;
        }

        public override UnityEngine.Object LoadObject(string url)
        {
            var handle = Package.LoadAssetSync(url);
            if (!objMap.TryGetValue(handle.AssetObject, out var v2))
                objMap[handle.AssetObject] = v2 = (handle, 0);
            v2.Item2++;
            return handle.AssetObject;
        }

        public override async STask<UnityEngine.Object> LoadObjectAsync(string url)
        {
            var handle = Package.LoadAssetAsync(url);
            await handle.Task;
            if (!objMap.TryGetValue(handle.AssetObject, out var v2))
                objMap[handle.AssetObject] = v2 = (handle, 0);
            v2.Item2++;
            return handle.AssetObject;
        }

        public override UnityEngine.SceneManagement.Scene LoadScene(string url, LoadSceneMode mode)
        {
            return Package.LoadSceneSync(url, mode).SceneObject;
        }

        public override async STask<UnityEngine.SceneManagement.Scene> LoadSceneAsync(string url, LoadSceneMode mode)
        {
            var handle = Package.LoadSceneAsync(url, mode);
            await handle.Task;
            return handle.SceneObject;
        }

        public override void Release(GameObject obj)
        {
            if (gobjMap.TryGetValue(obj, out var handle))
            {
                handle.Dispose();
                gobjMap.Remove(obj);
                GameObject.DestroyImmediate(obj);
            }
        }

        public override void Release(UnityEngine.SceneManagement.Scene obj)
        {
           
        }

        public override void Release(UnityEngine.Object obj)
        {
            if (objMap.TryGetValue(obj, out var v2))
            {
                v2.Item2--;
                if (v2.Item2 == 0) v2.Item1.Dispose();
                objMap.Remove(obj);
            }
        }

        public override STask ReleaseAllUnuseObjects()
        {
            return Package.UnloadUnusedAssetsAsync().AsTask();
        }
    }
}
#endif