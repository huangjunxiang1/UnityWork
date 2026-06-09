using Game;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

class YooPackageLoader
{
    public YooPackageLoader(ResourcePackage pkg, string group)
    {
        this.Pkg = pkg;
        this.Group = group;
    }
    public ResourcePackage Pkg { get; private set; }
    public string Group { get; private set; }

    public GameObject LoadGameObject(string url) => SAsset.LoadGameObject($"{Group}_{url}");
    public UnityEngine.SceneManagement.Scene LoadScene(string url, LoadSceneMode mode) => SAsset.LoadScene($"{Group}_{url}", mode);
    public T LoadObject<T>(string url) where T : UnityEngine.Object => SAsset.Load<T>($"{Group}_{url}");

    public STask<GameObject> LoadGameObjectAsync(string url) => SAsset.LoadGameObjectAsync($"{Group}_{url}");
    public STask<UnityEngine.SceneManagement.Scene> LoadSceneAsync(string url, LoadSceneMode mode) => SAsset.LoadSceneAsync($"{Group}_{url}", mode);
    public STask<T> LoadObjectAsync<T>(string url) where T : UnityEngine.Object => SAsset.LoadAsync<T>($"{Group}_{url}");
}
