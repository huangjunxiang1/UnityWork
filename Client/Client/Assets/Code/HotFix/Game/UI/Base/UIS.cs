using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Game;
using FairyGUI;

enum UIModel
{
    UGUI,
    FGUI,
}
static class UIS
{
    static UIS()
    {
        //ugui init
        {
            GameObject root = new GameObject("UIRoot", typeof(RectTransform));
            root.layer = LayerMask.NameToLayer("UI");
            GameObject.DontDestroyOnLoad(root);
            UGUIRoot = root.transform as RectTransform;
            UGUIRoot.sizeDelta = new Vector2(Screen.width, Screen.height);
            UGUIRoot.pivot = Vector2.zero;
            UGUIRoot.position = Vector3.zero;
            UGUICamera = GameObject.Find("UGUICamera").GetComponent<Camera>();

            UGUIRoot.gameObject.SetActive(GameSetting.UIModel == UIModel.UGUI);
            UGUICamera.gameObject.SetActive(GameSetting.UIModel == UIModel.UGUI);
        }
      
        //fgui init
        {
            //加载图集包和组件包
            UIPackage.AddPackage("Assets/Res/UI/FUI/ResPkg/ResPkg");
            UIPackage.AddPackage("Assets/Res/UI/FUI/ComPkg/ComPkg");

            GRoot.inst.visible = GameSetting.UIModel == UIModel.FGUI;
            StageCamera.main.gameObject.SetActive(GameSetting.UIModel == UIModel.FGUI);
        }
    }

    static List<UIBase> _uiLst = new List<UIBase>();

    /// <summary>
    /// UGUI模式有效
    /// </summary>
    public static RectTransform UGUIRoot { get; }
    public static Camera UGUICamera { get; }

    public static T Open<T>(params object[] data) where T : UIBase, new()
    {
        if (!UIConfig.UIConfigMap.TryGetValue(typeof(T), out UIConfig config))
        {
            Loger.Error("没有UI配置 class：" + typeof(T));
            return default;
        }

        T ui = new T();
        _uiLst.Add(ui);
        ui.InitConfig(config, data);
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
