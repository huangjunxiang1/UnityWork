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
    /// 原始资源加载器
    /// </summary>
    public class AssetPrimitiveLoader : AssetBaseLoader
    {
        public override UnityEngine.Object Load(string path)
        {
            var wait = Addressables.LoadAssetAsync<UnityEngine.Object>(AssetLoad.Directory + path);
            wait.WaitForCompletion();
            return wait.Result;
        }

        public override TaskAwaiter<UnityEngine.Object> LoadAsync(string path)
        {
            TaskAwaiter<UnityEngine.Object> task = new(path);
            getTaskAndWait(path, task);
            return task;
        }

        public override TaskAwaiter<UnityEngine.Object> LoadAsync(string path, TaskAwaiter<UnityEngine.Object> customTask)
        {
            getTaskAndWait(path, customTask);
            return customTask;
        }

        public override void Release(UnityEngine.Object target)
        {
            Addressables.Release(target);
        }
        async void getTaskAndWait(string path, TaskAwaiter<UnityEngine.Object> task)
        {
            var wait = Addressables.LoadAssetAsync<UnityEngine.Object>(AssetLoad.Directory + path);

             await wait.Task;

            //如果状态是没完成 但是被释放了 说明异步被中途取消
            if (!task.IsCompleted && !task.IsDisposed)
                task.TrySetResult(wait.Result);
            else
                this.Release(wait.Result);
        }
    }
}
