using Game;
using UnityEngine;

#if UGUI
public abstract class UUI3D : UUIBase
{
    RectTransform _ui;
    Canvas canvas;
    STask task;
    UIStatus states;

    public sealed override UIStatus uiStates => states;
    public sealed override Canvas Canvas => this.canvas;
    public sealed override RectTransform ui => this._ui;
    public sealed override STask onTask => task;

    public sealed override async STask LoadConfig(UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStatus.Loading;
        this._ui = (RectTransform)SAsset.LoadGameObject(url, ReleaseMode.Destroy).transform;
        this._ui.SetParent(Client.UI.UGUIRoot);
        this._ui.localScale = Vector3.one;
        this._ui.rotation = Quaternion.identity;
        this._ui.anchoredPosition = default;
        this.canvas = this._ui.GetComponentInChildren<Canvas>();

        this.Binding();
        this.states = UIStatus.OnTask;
        await (task = this.OnTask(data));
        if (this.Disposed) return;
        this.states = UIStatus.Success;
        this.OnEnter(data);
    }
    public sealed override async STask LoadConfigAsync(UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this.states = UIStatus.Loading;
        GameObject ui = await SAsset.LoadGameObjectAsync(url, ReleaseMode.Destroy);
        this._ui = (RectTransform)ui.transform;
        this._ui.SetParent(Client.UI.UGUIRoot);
        this._ui.localScale = Vector3.one;
        this._ui.rotation = Quaternion.identity;
        this._ui.anchoredPosition = default;
        this.canvas = this._ui.GetComponentInChildren<Canvas>();

        this.Binding();
        this.states = UIStatus.OnTask;
        await (task = this.OnTask(data));
        if (this.Disposed) return;
        this.states = UIStatus.Success;
        this.OnEnter(data);
    }
}
#endif