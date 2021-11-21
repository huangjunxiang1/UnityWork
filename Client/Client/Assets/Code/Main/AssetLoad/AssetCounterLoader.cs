using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Main
{
    /// <summary>
    /// 计数加载器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AssetCounterLoader<T> : AssetBaseLoader<T> where T : UnityEngine.Object
    {
        class Temp
        {
            public T target;
            public int count;
            public bool isLoading;
            public AsyncOperationHandle<T> wait;
        }
        Dictionary<string, Temp> counter = new Dictionary<string, Temp>(997);
        Dictionary<T, string> pathMap = new Dictionary<T, string>(997);

        public override T Load(string path)
        {
            if (!counter.TryGetValue(path, out Temp value))
            {
                value = new Temp();
                counter[path] = value;
                value.wait = Addressables.LoadAssetAsync<T>(AssetLoad.Directory + path);
                value.wait.WaitForCompletion();
                value.target = value.wait.Result;
                value.wait = default;
                pathMap[value.target] = path;
            }
            else
            {
                //正在异步加载中 则同步等待
                if (value.isLoading)
                {
                    //打印一个error  最好不要在异步加载时又同步加载
                    Loger.Error("此单位正在异步加载中 " + path);
                    value.wait.WaitForCompletion();
                    value.target = value.wait.Result;
                    value.isLoading = false;
                    pathMap[value.target] = path;
                }
            }
            value.count++;
            return value.target;
        }

        public override TaskAwaiter<T> LoadAsync(string path)
        {
            TaskAwaiter<T> task = new TaskAwaiter<T>(path);
            if (counter.TryGetValue(path, out Temp value))
            {
                value.count++;
                task.TrySetResult(value.target);
            }
            else
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
            if (!pathMap.TryGetValue(target, out string path))
            {
                Loger.Error("未知资源 " + target.name);
                return;
            }
            counter.TryGetValue(path, out Temp cnter);
            if (cnter.count > 1)
                cnter.count--;
            else
            {
                counter.Remove(path);
                pathMap.Remove(target);
                Addressables.Release(target);
            }
        }

        async void getTaskAndWait(string path, TaskAwaiter<T> task)
        {
            if (!counter.TryGetValue(path, out Temp value))
            {
                value = new Temp();
                counter[path] = value;
                value.wait = Addressables.LoadAssetAsync<T>(AssetLoad.Directory + path);
            }
            value.isLoading = true;

            await value.wait.Task;
            value.target = value.wait.Result;
            value.isLoading = false;
            value.wait = default;
            pathMap[value.target] = path;

            //如果状态是没完成 但是被释放了 说明异步被中途取消
            if (!task.IsCompleted && !task.IsDisposed)
            {
                value.count++;
                task.TrySetResult(value.target);
            }
            else
                this.Release(value.target);
        }
    }
}
