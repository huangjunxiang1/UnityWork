using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Game;
using FairyGUI;
using Main;

enum UIModel
{
    UGUI,
    FGUI,
}
static class UIS
{
    static UIS()
    {
        GameObject root = new("UIRoot", typeof(RectTransform));
        root.layer = LayerMask.NameToLayer("UI");
        GameObject.DontDestroyOnLoad(root);
        UGUIRoot = root.transform as RectTransform;
        UGUIRoot.sizeDelta = new Vector2(Screen.width, Screen.height);
        UGUIRoot.pivot = Vector2.zero;
        UGUIRoot.position = Vector3.zero;
        UGUICamera = GameObject.Find("UGUICamera").GetComponent<Camera>();
    }

    static readonly List<UIBase> _uiLst = new();

    /// <summary>
    /// UGUI模式有效
    /// </summary>
    public static RectTransform UGUIRoot { get; }
    public static Camera UGUICamera { get; }

    public static async TaskAwaiter Init()
    {
        //ugui init
        {
            UGUIRoot.gameObject.SetActive(GameSetting.UIModel == UIModel.UGUI);
            UGUICamera.gameObject.SetActive(GameSetting.UIModel == UIModel.UGUI);
        }

        //fgui init
        {
            GRoot.inst.visible = GameSetting.UIModel == UIModel.FGUI;
            StageCamera.main.gameObject.SetActive(GameSetting.UIModel == UIModel.FGUI);
            UIPackage.AddPackage((await AssetLoad.TextAssetLoader.LoadAsync("UI/FUI/ComPkg/ComPkg_fui.bytes")).bytes, "ComPkg", fguiLoader);
            UIPackage.AddPackage((await AssetLoad.TextAssetLoader.LoadAsync("UI/FUI/ResPkg/ResPkg_fui.bytes")).bytes, "ResPkg", fguiLoader);
            NTexture.CustomDestroyMethod += textureUnLoad;
            NAudioClip.CustomDestroyMethod += audioUnLoad;
        }
    }
    async static void fguiLoader(string name, string extension, System.Type type, PackageItem item)
    {
        switch (item.type)
        {
            case PackageItemType.Sound:
                {
                    var task = AssetLoad.AudioLoader.LoadAsync($"UI/FUI/{item.owner.name}/{name}{extension}");
                    await task;
                    item.owner.SetItemAsset(item, task.GetResult(), DestroyMethod.Custom);
                }
                break;
            case PackageItemType.Atlas:
                {
                    var task = AssetLoad.TextureLoader.LoadAsync($"UI/FUI/{item.owner.name}/{name}{extension}");
                    await task;
                    item.owner.SetItemAsset(item, task.GetResult(), DestroyMethod.Custom);
                }
                break;
            default:
                Loger.Error("未定义加载->" + item.type);
                break;
        }
    }
    static void textureUnLoad(Texture texture)
    {
        AssetLoad.TextureLoader.Release(texture);
    }
    static void audioUnLoad(AudioClip audio)
    {
        AssetLoad.AudioLoader.Release(audio);
    }

    public static T Open<T>(params object[] data) where T : UIBase, new()
    {
        if (!UIConfig.UIConfigMap.TryGetValue(typeof(T), out UIConfig cfg))
            cfg = UIConfig.Default;

        T ui = new();
        _uiLst.Add(ui);

        ui.LoadConfig(cfg, data);
        return ui;
    }
    public static T OpenAsync<T>(params object[] data) where T : UIBase, new()
    {
        if (!UIConfig.UIConfigMap.TryGetValue(typeof(T), out UIConfig cfg))
            cfg = UIConfig.Default;

        T ui = new();
        _uiLst.Add(ui);

        ui.LoadConfigAsync(cfg, data);
        return ui;
    }
    public static T Get<T>() where T : UIBase
    {
        return _uiLst.Find(t => t is T) as T;
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void Close<T>() where T : UIBase
    {
        T ui = Get<T>();
        if (ui != null)
            ui.Dispose();
    }

    /// <summary>
    /// Dispose的时候会回调到Remove
    /// </summary>
    public static void CloseAll()
    {
        while (_uiLst.Count > 0)
            _uiLst[0].Dispose();
    }

    /// <summary>
    /// 只移除 Close的时候会回调到这里
    /// </summary>
    /// <param name="ui"></param>
    public static void Remove(UIBase ui)
    {
        _uiLst.Remove(ui);
    }
}
