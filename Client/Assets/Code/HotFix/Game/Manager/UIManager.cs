using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Game;

class UIManager : LManager<UIManager>
{
    public UIManager()
    {
        GameObject uiRoot = new GameObject("UIRoot");
        uiRoot.layer = LayerMask.NameToLayer("UI");
        _uiRoot = new WObject((int)ObjectType.UIRoot, uiRoot);
        WRoot.Inst.AddChild(_uiRoot);
    }

    WObject _uiRoot;
    List<UUIBase> _uiLst = new List<UUIBase>();

    public T Open<T>(object data = null) where T : UUIBase, new()
    {
        if (!UIConfig.UIConfigMap.TryGetValue(typeof(T), out UIConfig config))
        {
            Loger.Error("没有UI配置 class：" + typeof(T));
            return default;
        }

        T ui = new T();
        _uiLst.Add(ui);
        ui.Init(config);
        ui.Binding();
        ui.UI.transform.SetParent(_uiRoot.GameObject.transform);
        ui.UI.transform.localPosition = default;
        ui.UI.transform.localScale = Vector3.one;
        ui.UI.transform.rotation = default;
        ui.Enter(data);
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
        {
            _uiLst.Remove(ui);
            ui.Dispose();
        }
    }
}
