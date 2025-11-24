using Game;
using UnityEngine;

#if UGUI
public abstract class UUI3D : UUIBase
{
    RectTransform _ui;
    Canvas _canvas;
    STask _task;
    UIStatus _states;

    public sealed override UIStatus uiStates => _states;
    public sealed override Canvas Canvas => this._canvas;
    public sealed override RectTransform ui => this._ui;
    public sealed override STask onTask => _task;

    public sealed override async STask LoadConfig(UIConfig config, STask completed)
    {
        await base.LoadConfig(config, completed);

        this.OnAwake();
        this._states = UIStatus.Loading;
        this._ui = (RectTransform)SAsset.LoadGameObject(url, ReleaseMode.Destroy).transform;
        this._ui.SetParent(Client.UI.UGUIRoot);
        this._ui.localScale = Vector3.one;
        this._ui.rotation = Quaternion.identity;
        this._ui.anchoredPosition = default;
        this._canvas = this._ui.GetComponentInChildren<Canvas>();

        this.Binding();
        this._states = UIStatus.OnTask;
        await (_task = this.OnTask());
        if (this.Disposed) return;
        this._states = UIStatus.Success;
        this.OnEnter();
    }
    public sealed override async STask LoadConfigAsync(UIConfig config, STask completed)
    {
        await base.LoadConfigAsync(config, completed);

        this.OnAwake();
        this._states = UIStatus.Loading;
        GameObject ui = await SAsset.LoadGameObjectAsync(url, ReleaseMode.Destroy);
        this._ui = (RectTransform)ui.transform;
        this._ui.SetParent(Client.UI.UGUIRoot);
        this._ui.localScale = Vector3.one;
        this._ui.rotation = Quaternion.identity;
        this._ui.anchoredPosition = default;
        this._canvas = this._ui.GetComponentInChildren<Canvas>();

        this.Binding();
        this._states = UIStatus.OnTask;
        await (_task = this.OnTask());
        if (this.Disposed) return;
        this._states = UIStatus.Success;
        this.OnEnter();
    }
}
#endif