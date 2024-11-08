﻿using System;
using UnityEngine;
using Game;

#if UGUI
public abstract class UUI : UUIBase
{
    RectTransform _ui;
    Canvas canvas;
    STask task;
    UIStates states;

    public sealed override UIStates uiStates => states;
    public sealed override Canvas Canvas => this.canvas;
    public sealed override RectTransform ui => this.ui;
    public sealed override STask onTask => task;

    public sealed override async STask LoadConfig(UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this._ui = (RectTransform)SAsset.LoadGameObject(url, ReleaseMode.Destroy).transform;
        this._ui.gameObject.SetActive(false);
        this.canvas = this._ui.GetComponent<Canvas>();
        this.Binding();
        this.setConfig();
        this.states = UIStates.OnTask;
        await (task = this.OnTask(data));
        if (this.Disposed) return;
        this.states = UIStates.Success;
        this._ui.gameObject.SetActive(this.isShow);
        this.OnEnter(data);
    }
    public sealed override async STask LoadConfigAsync(UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this._ui = (RectTransform)(await SAsset.LoadGameObjectAsync(url, ReleaseMode.Destroy)).transform;
        this._ui.gameObject.SetActive(false);
        this.canvas = this._ui.GetComponent<Canvas>();
        this.Binding();
        this.setConfig();
        this.states = UIStates.OnTask;
        await (task = this.OnTask(data));
        if (this.Disposed) return;
        this.states = UIStates.Success;
        this._ui.gameObject.SetActive(this.isShow);
        this.OnEnter(data);
    }

    void setConfig()
    {
        this._ui.SetParent(Client.UI.UGUIRoot);
        this._ui.localScale = Vector3.one;
        this._ui.rotation = Quaternion.identity;
        this._ui.sizeDelta = Client.UI.UGUIRoot.sizeDelta;
        this._ui.anchorMin = default;
        this._ui.anchorMax = Vector2.one;
        this._ui.anchoredPosition = default;

        const int max = 3;
        if (this.Parent is not UIBase)
        {
            int layer = 0;
            this.canvas.sortingOrder = (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, max - layer);
        }
        else
        {
            int layer = this.CrucialLayer;
            if (layer > max)
                Loger.Error("层级太深 layer=" + layer);
            this.canvas.sortingOrder = ((UIBase)Parent).sortOrder + (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, max - layer);
        }
    }
}
#endif