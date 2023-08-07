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
    public abstract class AssetBaseLoader
    {
        public abstract UnityEngine.Object Load(string path);
        public abstract TaskAwaiter<UnityEngine.Object> LoadAsync(string path);
        public abstract void Release(UnityEngine.Object target);
    }
}
