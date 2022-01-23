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
    public class AssetPrimitiveLoader<T> : AssetBaseLoader<T> where T : UnityEngine.Object
    {
        public override T Load(string path)
        {
            var wait = Addressables.LoadAssetAsync<T>(AssetLoad.Directory + path);
            wait.WaitForCompletion();
            return wait.Result;
        }

        public override TaskAwaiter<T> LoadAsync(string path)
        {
            TaskAwaiter<T> task = new TaskAwaiter<T>(path);
            getTaskAndWait(path, task);
            return task;
        }

        public override TaskAwaiter<T> LoadAsyncRef(string path, ref TaskAwaiter<T> task)
        {
            if (task == null || task.IsCompleted || task.IsDisposed)
            {
                task = LoadAsync(path);
            }
            else
            {
                if (!path.Equals(task.Tag))
                {
                    task.TryCancel();
                    task = LoadAsync(path);
                }
                else
                    task.Reset();
            }
            return task;
        }

        public override void Release(T target)
        {
            Addressables.Release(target);
        }
        async void getTaskAndWait(string path, TaskAwaiter<T> task)
        {
            var wait = Addressables.LoadAssetAsync<T>(AssetLoad.Directory + path);

            await wait.Task;

            //如果状态是没完成 但是被释放了 说明异步被中途取消
            if (!task.IsCompleted && !task.IsDisposed)
                task.TrySetResult(wait.Result);
            else
                this.Release(wait.Result);
        }
    }
}
