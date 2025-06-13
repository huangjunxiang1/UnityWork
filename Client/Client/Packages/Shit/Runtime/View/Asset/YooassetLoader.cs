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
        class Item
        {
            public string url;
            public AssetHandle asset;
            public int num;
        }
        Dictionary<string, Item> urlMap = new();
        Dictionary<UnityEngine.Object, Item> objMap = new();

        public ResourcePackage Package { get; private set; }

        public void SetDefaultPackage(YooAsset.ResourcePackage pkg)
        {
            Package = pkg;
            YooAssets.SetDefaultPackage(pkg);
        }
        public override GameObject LoadGameObject(string url)
        {
            if (!urlMap.TryGetValue(url, out var item))
                urlMap[url] = item = new Item { url = url, asset = Package.LoadAssetAsync<GameObject>(url) };
            item.asset.WaitForAsyncComplete();
            var go = item.asset.InstantiateSync();
            objMap[go] = item;
            item.num++;
            return go;
        }

        public override async STask<GameObject> LoadGameObjectAsync(string url)
        {
            if (!urlMap.TryGetValue(url, out var item))
                urlMap[url] = item = new Item { url = url, asset = Package.LoadAssetAsync<GameObject>(url) };
            var op = item.asset.InstantiateAsync();
            await op.Task;
            objMap[op.Result] = item;
            item.num++;
            return op.Result;
        }

        public override UnityEngine.Object LoadObject(string url)
        {
            if (!urlMap.TryGetValue(url, out var item))
                urlMap[url] = item = new Item { url = url, asset = Package.LoadAssetAsync(url) };
            item.asset.WaitForAsyncComplete();
            objMap[item.asset.AssetObject] = item;
            item.num++;
            return item.asset.AssetObject;
        }

        public override async STask<UnityEngine.Object> LoadObjectAsync(string url)
        {
            if (!urlMap.TryGetValue(url, out var item))
                urlMap[url] = item = new Item { url = url, asset = Package.LoadAssetAsync(url) };
            await item.asset.Task;
            objMap[item.asset.AssetObject] = item;
            item.num++;
            return item.asset.AssetObject;
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
            if (objMap.TryGetValue(obj, out var item))
            {
                objMap.Remove(obj);
                item.num--;
                if (item.num == 0)
                {
                    item.asset.Dispose();
                    urlMap.Remove(item.url);
                }
                GameObject.DestroyImmediate(obj);
            }
        }

        public override void Release(UnityEngine.SceneManagement.Scene obj)
        {
           
        }

        public override void Release(UnityEngine.Object obj)
        {
            if (objMap.TryGetValue(obj, out var item))
            {
                item.num--;
                if (item.num == 0)
                {
                    objMap.Remove(obj);
                    item.asset.Dispose();
                    urlMap.Remove(item.url);
                }
            }
        }

        public override STask ReleaseAllUnuseObjects()
        {
            return Package.UnloadUnusedAssetsAsync().AsTask();
        }
    }
}
#endif