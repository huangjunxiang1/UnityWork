using System;
using UnityEngine;
using Game;

abstract class UUI : UUIBase
{
    RectTransform _ui;
    Canvas canvas;
    STask task;
    UIStates states;

    public sealed override UIStates uiStates => states;
    public sealed override Canvas Canvas => this.canvas;
    public sealed override RectTransform ui => this.ui;
    public sealed override STask onTask => task;

    public sealed override STask LoadConfig(UIConfig config, STask completed, params object[] data)
    {
        base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this._ui = (RectTransform)SAsset.LoadGameObject(url, ReleaseMode.Destroy).transform;
        this.canvas = this._ui.GetComponent<Canvas>();
        this.Binding();
        this.setConfig();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        task.AddEvent(() =>
        {
            this.states = UIStates.Success;
            this.OnEnter(data);
        });
        return STask.Completed;
    }
    public sealed override async STask LoadConfigAsync(UIConfig config, STask completed, params object[] data)
    {
        _ = base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        GameObject g = await SAsset.LoadGameObjectAsync(url, ReleaseMode.Destroy);
        g.SetActive(false);
        this._ui = (RectTransform)g.transform;
        this.canvas = this._ui.GetComponent<Canvas>();
        this.Binding();
        this.setConfig();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        task.AddEvent(() =>
        {
            this.states = UIStates.Success;
            g.SetActive(true);
            this.OnEnter(data);
        });
    }

    void setConfig()
    {
        this._ui.SetParent(UI.Inst.UGUIRoot);
        this._ui.localScale = Vector3.one;
        this._ui.rotation = default;
        this._ui.sizeDelta = UI.Inst.UGUIRoot.sizeDelta;
        this._ui.anchorMin = default;
        this._ui.anchorMax = Vector2.one;
        this._ui.anchoredPosition = default;

        int layer = this.Layer - 2;
        if (layer > 3)
            Loger.Error("层级太深");
        if (this.Parent is not UIBase)
            this.canvas.sortingOrder = (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, 3 - layer);
        else
            this.canvas.sortingOrder = ((UIBase)Parent).sortOrder + (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, 3 - layer);
    }
}
