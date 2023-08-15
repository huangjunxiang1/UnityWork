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
    /// 计数加载器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AssetCounterLoader : AssetBaseLoader
    {
        class Temp
        {
            public UnityEngine.Object target;
            public int count;
            public bool isLoading;
            public AsyncOperationHandle<UnityEngine.Object> wait;
        }
        Dictionary<string, Temp> counter = new(50);
        Dictionary<UnityEngine.Object, string> pathMap = new(50);

        public override UnityEngine.Object Load(string path)
        {
            if (!counter.TryGetValue(path, out Temp value))
            {
                value = new Temp();
                counter[path] = value;
                value.wait = Addressables.LoadAssetAsync<UnityEngine.Object>(AssetLoad.Directory + path);
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

        public override TaskAwaiter<UnityEngine.Object> LoadAsync(string path)
        {
            TaskAwaiter<UnityEngine.Object> task = new(path);
            if (counter.TryGetValue(path, out Temp value))
            {
                value.count++;
                task.TrySetResult(value.target);
            }
            else
                getTaskAndWait(path, task);
            return task;
        }

        public override void Release(UnityEngine.Object target)
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

        async void getTaskAndWait(string path, TaskAwaiter<UnityEngine.Object> task)
        {
            if (!counter.TryGetValue(path, out Temp value))
            {
                value = new Temp();
                counter[path] = value;
                value.wait = Addressables.LoadAssetAsync<UnityEngine.Object>(AssetLoad.Directory + path);
            }
            value.isLoading = true;

            await value.wait.Task;
            value.target = value.wait.Result;
            value.isLoading = false;
            value.wait = default;
            pathMap[value.target] = path;

            //如果状态是没完成 但是被释放了 说明异步被中途取消
            if (!task.IsCompleted && !task.Disposed)
            {
                value.count++;
                task.TrySetResult(value.target);
            }
            else
                this.Release(value.target);
        }
    }
}
