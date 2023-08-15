using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

namespace Main
{
    /// <summary>
    /// 拷贝资源加载器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AssetCopyLoader : AssetBaseLoader
    {
        public override UnityEngine.Object Load(string path)
        {
            var wait = Addressables.LoadAssetAsync<UnityEngine.Object>(AssetLoad.Directory + path);
            wait.WaitForCompletion();
            UnityEngine.Object ret = UnityEngine.Object.Instantiate(wait.Result);
            Addressables.Release(wait.Result);
            return ret;
        }

        public override TaskAwaiter<UnityEngine.Object> LoadAsync(string path)
        {
            TaskAwaiter<UnityEngine.Object> task = new(path);
            getTaskAndWait(path, task);
            return task;
        }

        public override void Release(UnityEngine.Object target)
        {
            UnityEngine.Object.Destroy(target);
        }

        async void getTaskAndWait(string path, TaskAwaiter<UnityEngine.Object> task)
        {
            var wait = Addressables.LoadAssetAsync<UnityEngine.Object>(AssetLoad.Directory + path);

            await wait.Task;

            //如果状态是没完成 但是被释放了 说明异步被中途取消
            if (!task.IsCompleted && !task.Disposed)
            {
                UnityEngine.Object ret = UnityEngine.Object.Instantiate(wait.Result);
                Addressables.Release(wait.Result);
                task.TrySetResult(ret);
            }
            else
                Addressables.Release(wait.Result);
        }
    }
}
