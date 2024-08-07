﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

#if Addressables
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
#endif

namespace Game
{
#if Addressables
    class AddressablesLoader : SAssetLoader
    {
        Dictionary<UnityEngine.SceneManagement.Scene, SceneInstance> sceneMap = new();
        public override GameObject LoadGameObject(string url)
        {
            var wait = Addressables.InstantiateAsync(url, parent: Client.transform);
            wait.WaitForCompletion();
            return wait.Result;
        }

        public override STask<GameObject> LoadGameObjectAsync(string url) => Addressables.InstantiateAsync(url, parent: Client.transform).AsTask();

        public override UnityEngine.Object LoadObject(string url)
        {
            var wait = Addressables.LoadAssetAsync<UnityEngine.Object>(url);
            wait.WaitForCompletion();
            return wait.Result;
        }

        public override STask<UnityEngine.Object> LoadObjectAsync(string url) => Addressables.LoadAssetAsync<UnityEngine.Object>(url).AsTask();

        public override UnityEngine.SceneManagement.Scene LoadScene(string url, LoadSceneMode mode)
        {
            var wait = Addressables.LoadSceneAsync(url, mode);
            wait.WaitForCompletion();
            if (mode == LoadSceneMode.Additive)
                sceneMap[wait.Result.Scene] = wait.Result;
            return wait.Result.Scene;
        }

        public override async STask<UnityEngine.SceneManagement.Scene> LoadSceneAsync(string url, LoadSceneMode mode)
        {
            SceneInstance ret = await Addressables.LoadSceneAsync(url, mode).AsTask();
            if (mode == LoadSceneMode.Additive)
                sceneMap[ret.Scene] = ret;
            return ret.Scene;
        }

        public override void Release(GameObject obj) => Addressables.ReleaseInstance(obj);

        public override void Release(UnityEngine.SceneManagement.Scene obj)
        {
            if (!sceneMap.TryGetValue(obj, out var instance))
            {
                Loger.Error("未包含 " + obj);
                return;
            }
            sceneMap.Remove(obj);
            Addressables.UnloadSceneAsync(instance);
        }

        public override void Release(UnityEngine.Object obj) => Addressables.Release(obj);

        public override STask ReleaseAllUnuseObjects()
        {
            return STask.Completed;
        }
    }
#endif
}
