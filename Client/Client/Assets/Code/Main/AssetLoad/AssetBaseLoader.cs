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
    public abstract class AssetBaseLoader<T> where T : UnityEngine.Object
    {
        public abstract T Load(string path);
        public abstract TaskAwaiter<T> LoadAsync(string path);
        public abstract TaskAwaiter<T> LoadAsyncRef(string path, ref TaskAwaiter<T> task);
        public abstract void Release(T target);
    }
}
