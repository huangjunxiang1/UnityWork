using Game;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

public class SAssetWarper
{
    public SAssetWarper(ResourcePackage pkg, string format)
    {
        this.Pkg = pkg;
        this.Format = format;
    }
    public ResourcePackage Pkg { get; private set; }
    public string Format { get; private set; }


    public bool IsLocationValid(string name) => Pkg.IsLocationValid(string.Format(Format, name));
    public byte[] LoadRaw(string name)
    {
        if (!IsLocationValid(name))
            return null;
        var handler = Pkg.LoadAssetSync<RawFileObject>(string.Format(Format, name));
        var ret = handler.GetAssetObject<RawFileObject>().GetBytes();
        handler.Dispose();
        return ret;
    }
    public string LoadText(string name)
    {
        if (!IsLocationValid(name))
            return null;
        var handler = Pkg.LoadAssetSync<RawFileObject>(string.Format(Format, name));
        var ret = handler.GetAssetObject<RawFileObject>().GetText();
        handler.Dispose();
        return ret;
    }
    public async STask<byte[]> LoadRawAsync(string name)
    {
        if (!IsLocationValid(name))
            return null;
        var handler = Pkg.LoadAssetAsync<RawFileObject>(string.Format(Format, name));
        await handler;
        var bs = handler.GetAssetObject<RawFileObject>().GetBytes();
        handler.Dispose();
        return bs;
    }
    public async STask<string> LoadTextAsync(string name)
    {
        if (!IsLocationValid(name))
            return null;
        var handler = Pkg.LoadAssetAsync<RawFileObject>(string.Format(Format, name));
        await handler;
        var bs = handler.GetAssetObject<RawFileObject>().GetText();
        handler.Dispose();
        return bs;
    }

    public GameObject LoadGameObject(string name, ReleaseMode releaseMode = ReleaseMode.PutToPool) => SAsset.LoadGameObject(string.Format(Format, name), releaseMode);
    public UnityEngine.SceneManagement.Scene LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single) => SAsset.LoadScene(string.Format(Format, name), mode);
    public T Load<T>(string name) where T : UnityEngine.Object => SAsset.Load<T>(string.Format(Format, name));

    public STask<GameObject> LoadGameObjectAsync(string name, ReleaseMode releaseMode = ReleaseMode.PutToPool) => SAsset.LoadGameObjectAsync(string.Format(Format, name), releaseMode);
    public STask<UnityEngine.SceneManagement.Scene> LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single) => SAsset.LoadSceneAsync(string.Format(Format, name), mode);
    public STask<T> LoadAsync<T>(string name) where T : UnityEngine.Object => SAsset.LoadAsync<T>(string.Format(Format, name));
}
