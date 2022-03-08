using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Main
{
    public class AssetScriptableObjectLoader : AssetBaseLoader<ScriptableObject> 
    {
        public override ScriptableObject Load(string path)
        {
            
            var wait = Addressables.LoadAssetAsync<ScriptableObject>(AssetLoad.Directory + path);
            wait.WaitForCompletion();
            return wait.Result;
        }

        public override TaskAwaiter<ScriptableObject> LoadAsync(string path)
        {
            TaskAwaiter<ScriptableObject> task = new TaskAwaiter<ScriptableObject>(path);
            getTaskAndWait(path, task);
            return task;
        }
        public override TaskAwaiter<ScriptableObject> LoadAsync(string path, TaskAwaiter<ScriptableObject> customTask)
        {
            getTaskAndWait(path, customTask);
            return customTask;
        }

        public override void Release(ScriptableObject target)
        {
            Addressables.Release(target);
        }
        async void getTaskAndWait(string path, TaskAwaiter<ScriptableObject> task)
        {
            var wait = Addressables.LoadAssetAsync<ScriptableObject>(AssetLoad.Directory + path);

            await wait.Task;

            //如果状态是没完成 但是被释放了 说明异步被中途取消
            if (!task.IsCompleted && !task.IsDisposed)
                task.TrySetResult(wait.Result);
            else
                this.Release(wait.Result);
        }
    }
}