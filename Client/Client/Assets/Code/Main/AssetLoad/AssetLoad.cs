using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using FairyGUI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Main
{
    public static class AssetLoad
    {
        public const string Directory = "Assets/Res/";

#if UNITY_EDITOR
        //Editor模式加载的是原始资源  所以要用加载出来的拷贝一份再用
        public static AssetBaseLoader<GameObject> PrefabLoader { get; } = new AssetCopyLoader<GameObject>();
#else
        public static AssetBaseLoader<GameObject> PrefabLoader { get; } = new AssetPrimitiveLoader<GameObject>();
#endif
        public static AssetBaseLoader<Texture> TextureLoader { get; } = new AssetCounterLoader<Texture>();
        public static AssetBaseLoader<TextAsset> TextAssetLoader { get; } = new AssetPrimitiveLoader<TextAsset>();
        public static AssetBaseLoader<AudioClip> AudioLoader { get; } = new AssetPrimitiveLoader<AudioClip>();
        public static AssetBaseLoader<ScriptableObject> ScriptObjectLoader { get; } = new AssetPrimitiveLoader<ScriptableObject>();

        //默认保存一个通用加载器  
        public static AssetBaseLoader<UnityEngine.Object> DefaultLoader { get; } = new AssetPrimitiveLoader<UnityEngine.Object>();
    }
}
