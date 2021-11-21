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
    public class AssetCopyLoader<T> : AssetBaseLoader<T> where T : UnityEngine.Object
    {
        public override T Load(string path)
        {
            var wait = Addressables.LoadAssetAsync<T>(AssetLoad.Directory + path);
            wait.WaitForCompletion();
            T ret = UnityEngine.Object.Instantiate(wait.Result);
            Addressables.Release(wait.Result);
            return ret;
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
                if (!path.Equals(task.Token))
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
            UnityEngine.Object.Destroy(target);
        }

        async void getTaskAndWait(string path, TaskAwaiter<T> task)
        {
            var wait = Addressables.LoadAssetAsync<T>(AssetLoad.Directory + path);

            await wait.Task;

            //如果状态是没完成 但是被释放了 说明异步被中途取消
            if (!task.IsCompleted && !task.IsDisposed)
            {
                T ret = UnityEngine.Object.Instantiate(wait.Result);
                Addressables.Release(wait.Result);
                task.TrySetResult(ret);
            }
            else
                Addressables.Release(wait.Result);
        }
    }
}
