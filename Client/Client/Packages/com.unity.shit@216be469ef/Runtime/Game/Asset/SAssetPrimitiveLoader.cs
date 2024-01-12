using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 原始资源加载器
    /// </summary>
    public class SAssetPrimitiveLoader : SAssetBaseLoader
    {
        public override UnityEngine.Object Load(string path)
        {
            var wait = Addressables.LoadAssetAsync<UnityEngine.Object>(SAsset.Directory + path);
            wait.WaitForCompletion();
            return wait.Result;
        }

        public override async STask<UnityEngine.Object> LoadAsync(string path)
        {
            return await Addressables.LoadAssetAsync<UnityEngine.Object>(SAsset.Directory + path).Task;
        }

        public override void Release(UnityEngine.Object target)
        {
            Addressables.Release(target);
        }
    }
}
