using System;
using UnityEngine;
using Game;

#if UGUI
public abstract class UUI : UUIBase
{
    RectTransform _ui;
    Canvas _canvas;
    STask _task;
    UIStatus _states;
    bool _loadingView = false;

    public sealed override UIStatus uiStates => _states;
    public sealed override Canvas Canvas => this._canvas;
    public sealed override RectTransform ui => this.ui;
    public sealed override STask onTask => _task;

    public sealed override async STask LoadConfig(UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this._states = UIStatus.Loading;
        this._ui = (RectTransform)SAsset.LoadGameObject(url, ReleaseMode.Destroy).transform;
        this._ui.gameObject.SetActive(false);
        this._canvas = this._ui.GetComponent<Canvas>();
        this.Binding();
        this.setConfig();
        this._states = UIStatus.OnTask;
        _taskHandle();
        if (this.uiStates != UIStatus.Success)
        {
            _delay1Handle();
            _delay2Handle();
        }
        await this._task;
    }
    public sealed override async STask LoadConfigAsync(UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this._states = UIStatus.Loading;
        this._ui = (RectTransform)(await SAsset.LoadGameObjectAsync(url, ReleaseMode.Destroy)).transform;
        this._ui.gameObject.SetActive(false);
        this._canvas = this._ui.GetComponent<Canvas>();
        this.Binding();
        this.setConfig();
        this._states = UIStatus.OnTask;
        _taskHandle();
        if (this.uiStates != UIStatus.Success)
        {
            _delay1Handle();
            _delay2Handle();
        }
        await this._task;
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
            this._canvas.sortingOrder = (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, max - layer);
        }
        else
        {
            int layer = this.CrucialLayer;
            if (layer > max)
                Loger.Error("层级太深 layer=" + layer);
            this._canvas.sortingOrder = ((UIBase)Parent).sortOrder + (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, max - layer);
        }
    }
    async void _taskHandle()
    {
        this._task = this.OnTask(this.ParamObjects) ?? STask.Completed;
        await this._task;
        if (this.Disposed) return;
        this._states = UIStatus.Success;
        this._ui.gameObject.SetActive(this.isShow);
        this.OnEnter(this.ParamObjects);
        if (_loadingView)
            UIGlobalConfig.LoadingView(this, false);
    }
    async void _delay1Handle()
    {
        await STask.Delay(UIGlobalConfig.LoadingViewDelay1TimeMs);
        if (this.Disposed || this._states == UIStatus.Success) return;
        _loadingView = true;
        UIGlobalConfig.LoadingView(this, true);
    }
    async void _delay2Handle()
    {
        await STask.Delay(UIGlobalConfig.LoadingViewDelay2TimeMs);
        if (this.Disposed || this._states == UIStatus.Success) return;
        UIGlobalConfig.LoadingView(this, false);
        this.Dispose();
    }
}
#endif