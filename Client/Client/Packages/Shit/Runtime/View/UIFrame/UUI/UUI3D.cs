using Game;
using UnityEngine;

public abstract class UUI3D : UUIBase
{
    RectTransform _ui;
    Canvas canvas;
    STask task;
    UIStates states;

    public sealed override UIStates uiStates => states;
    public sealed override Canvas Canvas => this.canvas;
    public sealed override RectTransform ui => this._ui;
    public sealed override STask onTask => task;

    public sealed override STask LoadConfig(UIConfig config, STask completed, params object[] data)
    {
        base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this._ui = (RectTransform)SAsset.LoadGameObject(url, ReleaseMode.Destroy).transform;
        this._ui.SetParent(Client.UI.UGUIRoot);
        this._ui.localScale = Vector3.one;
        this._ui.rotation = default;
        this._ui.anchoredPosition = default;
        this.canvas = this._ui.GetComponentInChildren<Canvas>();

        this.Binding();
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
        GameObject ui = await SAsset.LoadGameObjectAsync(url, ReleaseMode.Destroy);
        this._ui = (RectTransform)ui.transform;
        this._ui.SetParent(Client.UI.UGUIRoot);
        this._ui.localScale = Vector3.one;
        this._ui.rotation = default;
        this._ui.anchoredPosition = default;
        this.canvas = this._ui.GetComponentInChildren<Canvas>();

        this.Binding();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        task.AddEvent(() =>
        {
            this.states = UIStates.Success;
            this.OnEnter(data);
        });
    }
}
