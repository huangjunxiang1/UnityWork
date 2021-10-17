using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace Main
{
    public static class AssetLoad
    {
        public static UnityEngine.Object Load(string path)
        {
            UnityEngine.Object o = AssetDatabase.LoadMainAssetAtPath("Assets/Res/" + path);
            if (o is GameObject go)
                return GameObject.Instantiate(go);
            return o;
        }
        public static T Load<T>(string path) where T : UnityEngine.Object
        {
            T o = AssetDatabase.LoadAssetAtPath<T>("Assets/Res/" + path);
            if (o is GameObject go)
                return GameObject.Instantiate(go) as T;
            return o;
        }
        public static void Return(UnityEngine.Object obj)
        {
            if (obj is GameObject)
                GameObject.DestroyImmediate(obj);
        }

        public static byte[] LoadConfigBytes(string name)
        {
            return File.ReadAllBytes(Application.dataPath + "/Res/Config/" + name);
        }
    }
}
