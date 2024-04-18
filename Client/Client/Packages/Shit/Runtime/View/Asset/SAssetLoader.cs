using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public abstract class SAssetLoader
    {
        public abstract GameObject LoadGameObject(string url);
        public abstract UnityEngine.SceneManagement.Scene LoadScene(string url, LoadSceneMode mode);
        public abstract UnityEngine.Object LoadObject(string url);

        public abstract STask<GameObject> LoadGameObjectAsync(string url);
        public abstract STask<UnityEngine.SceneManagement.Scene> LoadSceneAsync(string url, LoadSceneMode mode);
        public abstract STask<UnityEngine.Object> LoadObjectAsync(string url);

        public abstract void Release(GameObject obj);
        public abstract void Release(UnityEngine.SceneManagement.Scene obj);
        public abstract void Release(UnityEngine.Object obj);
    }
}
