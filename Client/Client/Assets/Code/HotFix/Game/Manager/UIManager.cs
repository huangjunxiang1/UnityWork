using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Game;

class UIManager : ManagerL<UIManager>
{
    public UIManager()
    {
        GameObject root = new GameObject("UIRoot");
        root.layer = LayerMask.NameToLayer("UI");
        GameObject.DontDestroyOnLoad(root);
        this.UIRoot = root.transform;
    }

    List<UUIBase> _uiLst = new List<UUIBase>();

    public Transform UIRoot { get; }

    public T Open<T>(params object[] data) where T : UUIBase, new()
    {
        if (!UIConfig.UIConfigMap.TryGetValue(typeof(T), out UIConfig config))
        {
            Loger.Error("没有UI配置 class：" + typeof(T));
            return default;
        }

        T ui = new T();
        _uiLst.Add(ui);
        ui.Init(config, data);
        return ui;
    }
    public T Get<T>() where T : UUIBase
    {
        return _uiLst.Find(t => t is T) as T;
    }
    public void Close<T>() where T : UUIBase
    {
        T ui = Get<T>();
        if (ui != null)
            ui.Dispose();
    }
    public void Remove(UUIBase ui)
    {
        _uiLst.Remove(ui);
    }
}
