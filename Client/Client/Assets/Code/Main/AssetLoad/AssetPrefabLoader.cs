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
    /// prefab比较特殊  作为一个单独加载器
    /// </summary>
    public class AssetPrefabLoader : AssetBaseLoader<GameObject>
    {
        class AssetRef : MonoBehaviour
        {
            [NonSerialized]
            public string path;
        }
        public AssetPrefabLoader()
        {
            _poolRoot = new GameObject("PoolRoot");
            GameObject.DontDestroyOnLoad(_poolRoot);
            _poolRoot.gameObject.SetActive(false);
        }

        GameObject _poolRoot;
        Dictionary<string, List<GameObject>> _pool = new Dictionary<string, List<GameObject>>(997);

        public override GameObject Load(string path)
        {
            if (_pool.TryGetValue(path, out var pool))
            {
                int cnt = pool.Count;
                if (cnt > 0)
                {
                    var go = pool[cnt - 1];
                    if (cnt == 1)
                        _pool.Remove(path);
                    else
                        pool.RemoveAt(cnt - 1);
                    return go;
                }
            }
            var wait = Addressables.InstantiateAsync(AssetLoad.Directory + path);
            wait.WaitForCompletion();
            var aref = wait.Result.AddComponent<AssetRef>();
            aref.path = path;
            return aref.gameObject;
        }

        public override TaskAwaiter<GameObject> LoadAsync(string path)
        {
            TaskAwaiter<GameObject> task = new TaskAwaiter<GameObject>();
            if (_pool.TryGetValue(path, out var pool))
            {
                int cnt = pool.Count;
                if (cnt > 0)
                {
                    var go = pool[cnt - 1];
                    if (cnt == 1)
                        _pool.Remove(path);
                    else
                        pool.RemoveAt(cnt - 1);
                    task.TrySetResult(go);
                    return task;
                }
            }
            getTaskAndWait(path, task);
            return task;
        }

        public override TaskAwaiter<GameObject> LoadAsyncRef(string path, ref TaskAwaiter<GameObject> task)
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

        public override void Release(GameObject target)
        {
            var aref = target.GetComponent<AssetRef>();
            if (aref == null)
            {
                Loger.Error("没有AssetRef组件 " + target);
                GameObject.Destroy(target);
                return;
            }
            if (!_pool.TryGetValue(aref.path, out var lst))
            {
                lst = new List<GameObject>();
                _pool[aref.path] = lst;
            }
            lst.Add(target);
            target.transform.SetParent(_poolRoot.transform);
        }
        public void ReleaseDontReturnPool(GameObject target)
        {
            Addressables.ReleaseInstance(target);
        }

        async void getTaskAndWait(string path, TaskAwaiter<GameObject> task)
        {
            var wait = Addressables.InstantiateAsync(AssetLoad.Directory + path);

            await wait.Task;

            var aref = wait.Result.AddComponent<AssetRef>();
            aref.path = path;
            //如果状态是没完成 但是被释放了 说明异步被中途取消
            if (!task.IsCompleted && !task.IsDisposed)
            {
                task.TrySetResult(aref.gameObject);
            }
            else
            {
                if (!_pool.TryGetValue(aref.path, out var lst))
                {
                    lst = new List<GameObject>();
                    _pool[aref.path] = lst;
                }
                lst.Add(aref.gameObject);
                aref.transform.SetParent(_poolRoot.transform);
            }
        }
    }
}
