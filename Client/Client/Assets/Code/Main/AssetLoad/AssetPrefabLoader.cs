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
    public class AssetPrefabLoader : AssetBaseLoader
    {
        public AssetPrefabLoader()
        {
            _poolRoot = new GameObject("PoolRoot");
            GameObject.DontDestroyOnLoad(_poolRoot);
            _poolRoot.SetActive(false);
        }

        readonly GameObject _poolRoot;
        readonly Dictionary<string, List<GameObject>> _pool = new(50);

        public override UnityEngine.Object Load(string path)
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
            var wait = Addressables.InstantiateAsync(AssetLoad.Directory + path, parent: _poolRoot.transform);
            wait.WaitForCompletion();
            return wait.Result;
        }

        public override async TaskAwaiter<UnityEngine.Object> LoadAsync(string path)
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
            return await Addressables.InstantiateAsync(AssetLoad.Directory + path, parent: _poolRoot.transform).Task;
        }

        public override void Release(UnityEngine.Object target)
        {
            Addressables.ReleaseInstance((GameObject)target);
        }
        public void ReleaseToPool(GameObject target, string url)
        {
#if DebugEnable
            if (string.IsNullOrEmpty(url))
            {
                Loger.Error("url is empty");
                return;
            }
#endif
            if (!_pool.TryGetValue(url, out var lst))
            {
                lst = new List<GameObject>();
                _pool[url] = lst;
            }
            lst.Add(target);
            target.transform.SetParent(_poolRoot.transform);
        }
    }
}
