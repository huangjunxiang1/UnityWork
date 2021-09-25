using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Main
{
    public static class AssetLoad
    {
        public static UnityEngine.Object Load(string path)
        {
            UnityEngine.Object o = AssetDatabase.LoadMainAssetAtPath(path);
            if (o is GameObject go)
                return GameObject.Instantiate(go);
            return o;
        }
        public static T Load<T>(string path) where T : UnityEngine.Object
        {
            T o = AssetDatabase.LoadAssetAtPath<T>(path);
            if (o is GameObject go)
                return GameObject.Instantiate(go) as T;
            return o;
        }
        public static void Return(UnityEngine.Object obj)
        {
            GameObject.DestroyImmediate(obj);
        }
    }
}
