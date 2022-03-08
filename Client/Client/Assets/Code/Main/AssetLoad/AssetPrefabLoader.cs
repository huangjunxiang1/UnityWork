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
        class TextureRef : MonoBehaviour
        {
            [NonSerialized]
            public Texture texture;
        }

        public AssetPrefabLoader()
        {
            _poolRoot = new GameObject("PoolRoot");
            GameObject.DontDestroyOnLoad(_poolRoot);
            _poolRoot.SetActive(false);
        }

        readonly GameObject _poolRoot;
        readonly Dictionary<string, List<GameObject>> _pool = new(50);

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
            AssetRef aref = wait.Result.AddComponent<AssetRef>();
            aref.path = path;
            return aref.gameObject;
        }

        public override TaskAwaiter<GameObject> LoadAsync(string path)
        {
            TaskAwaiter<GameObject> task = new();
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
        public override TaskAwaiter<GameObject> LoadAsync(string path, TaskAwaiter<GameObject> customTask)
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
                    customTask.TrySetResult(go);
                    return customTask;
                }
            }
            getTaskAndWait(path, customTask);
            return customTask;
        }

        public override void Release(GameObject target)
        {
            AssetRef ar = target.GetComponent<AssetRef>();
            if (ar == null)
            {
                Loger.Error("没有AssetRef组件 " + target);
                GameObject.Destroy(target);
                return;
            }
            releaseAssetRef(ar);
        }
        public void ReleaseAllAssetRef(GameObject target)
        {
            AssetRef[] ars = target.GetComponentsInChildren<AssetRef>();
            int len = ars.Length;
            for (int i = 0; i < len; i++)
                releaseAssetRef(ars[i]);
        }
        public void ReleaseDontReturnPool(GameObject target)
        {
            Addressables.ReleaseInstance(target);
        }

        public void AddTextureRef(GameObject target, Texture texture)
        {
            TextureRef tr = target.GetComponent<TextureRef>() ?? target.AddComponent<TextureRef>();
            if (tr.texture)
                AssetLoad.TextureLoader.Release(tr.texture);
            tr.texture = texture;
        }
        public void ReleaseTexture(GameObject target)
        {
            TextureRef tr = target.GetComponent<TextureRef>();
            if (tr == null) return;
            if (tr.texture)
            {
                AssetLoad.TextureLoader.Release(tr.texture);
                tr.texture = null;
            }
        }
        public void ReleaseAllTexture(GameObject target)
        {
            TextureRef[] trs = target.GetComponentsInChildren<TextureRef>();
            int len = trs.Length;
            for (int i = 0; i < len; i++)
            {
                if (trs[i].texture)
                    AssetLoad.TextureLoader.Release(trs[i].texture);
            }
        }


        void releaseAssetRef(AssetRef ar)
        {
            if (!_pool.TryGetValue(ar.path, out var lst))
            {
                lst = new List<GameObject>();
                _pool[ar.path] = lst;
            }
            lst.Add(ar.gameObject);
            ar.gameObject.transform.SetParent(_poolRoot.transform);
        }

        async void getTaskAndWait(string path, TaskAwaiter<GameObject> task)
        {
            var wait = Addressables.InstantiateAsync(AssetLoad.Directory + path);

            await wait.Task;

            //如果状态是没完成 但是被释放了 说明异步被中途取消
            if (!task.IsCompleted && !task.IsDisposed)
            {
                var aref = wait.Result.AddComponent<AssetRef>();
                aref.path = path;
                task.TrySetResult(aref.gameObject);
            }
            else
                ReleaseDontReturnPool(wait.Result);
        }
    }
}
