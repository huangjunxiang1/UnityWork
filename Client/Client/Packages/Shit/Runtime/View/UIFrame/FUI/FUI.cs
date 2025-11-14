using System;

#if FairyGUI
using FairyGUI;
using Game;
using Unity.Mathematics;

public enum AdaptStyle
{
    FullScreen,
    Center,
    Left,
    Right,
    Top,
    Bottom,
}

public abstract class FUI : FUIBase
{
    GComponent _ui;
    STask _task;
    UIStatus _states;
    bool _loadingView = false;

    public sealed override UIStatus uiStates => _states;
    public sealed override GComponent ui => this._ui;
    public sealed override int sortOrder
    {
        get { return this._ui.sortingOrder; }
        set { this._ui.sortingOrder = value; }
    }
    public sealed override bool isShow
    {
        get { return base.isShow; }
        set
        {
            base.isShow = value;
            if (this._ui != null)
                this._ui.visible = value;
        }
    }
    public sealed override STask onTask => _task;
    public GObject Close { get; private set; }

    public sealed override async STask LoadConfig(Game.UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this._states = UIStatus.Loading;
        this._ui = UIPackage.CreateObjectFromURL(this.url).asCom;
        (Close = this._ui.GetChild("Close")
            ?? this._ui.GetChild("Lable")?.asCom?.GetChild("Close")
            ?? this._ui.GetChild("_Lable")?.asCom?.GetChild("Close"))?.onClick.Add(this.Dispose);
        this._ui.visible = false;
        this.Binding();
        this.setConfig();
        this._states = UIStatus.OnTask;

        this._task = this.OnTask(this.ParamObjects) ?? STask.Completed;
        if (!this._task.IsCompleted)
        {
            _delay1Handle();
            _delay2Handle();
        }
        await this._task;

        if (this.Disposed) return;
        _success();
    }
    public sealed override async STask LoadConfigAsync(Game.UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this._states = UIStatus.Loading;

        STask load = new();
        UIPackage.CreateObjectFromURL(this.url, c =>
        {
            this._ui = c.asCom;

            if (this.Disposed)
            {
                this._ui.Dispose();
                return;
            }
            (Close = this._ui.GetChild("Close")
            ?? this._ui.GetChild("Lable")?.asCom?.GetChild("Close")
            ?? this._ui.GetChild("_Lable")?.asCom?.GetChild("Close"))?.onClick.Add(this.Dispose);
            this._ui.visible = false;
            this.Binding();
            this.setConfig();
            this._states = UIStatus.OnTask;

            load.TrySetResult();
        });

        this._task = this.OnTask(this.ParamObjects) ?? STask.Completed;
        if (!this._task.IsCompleted)
        {
            _delay1Handle();
            _delay2Handle();
        }

        await load;
        await this._task;

        if (this.Disposed) return;
        _success();

    }
    public override void Dispose()
    {
        _task.TryCancel();
        if (this._ui != null)
        {
            if (this.uiStates == UIStatus.Success)
                this.Hide(true, this._ui.Dispose);
            else
                this._ui.Dispose();
        }
        base.Dispose();
    }

    public void SetAdapt(AdaptStyle style)
    {
        this._ui.relations.ClearAll();
        switch (style)
        {
            case AdaptStyle.FullScreen:
                {
                    this._ui.MakeFullScreen();
                    this._ui.xy = new(0, 0);
                    this._ui.AddRelation(this._ui.parent, RelationType.Size);
                    this._ui.AddRelation(this._ui.parent, RelationType.Center_Center);
                }
                break;
            case AdaptStyle.Center:
                this._ui.Center();
                this._ui.AddRelation(this._ui.parent, RelationType.Center_Center);
                break;
            case AdaptStyle.Left:
                this._ui.height = this._ui.parent.height;
                this._ui.xy = new(0, 0);
                this._ui.AddRelation(this._ui.parent, RelationType.Height);
                break;
            case AdaptStyle.Right:
                this._ui.height = this._ui.parent.height;
                this._ui.xy = new(this._ui.parent.width - this._ui.width, 0);
                this._ui.AddRelation(this._ui.parent, RelationType.Height);
                break;
            case AdaptStyle.Top:
                this._ui.width = this._ui.parent.width;
                this._ui.xy = new(0, 0);
                this._ui.AddRelation(this._ui.parent, RelationType.Width);
                break;
            case AdaptStyle.Bottom:
                this._ui.width = this._ui.parent.width;
                this._ui.xy = new(0, this._ui.parent.height - this._ui.height);
                this._ui.AddRelation(this._ui.parent, RelationType.Width);
                break;
            default:
                break;
        }
    }
   
    public void AutoAdapt()
    {
        UIContentScaler scaler = Stage.inst.gameObject.GetComponent<UIContentScaler>();
        float2 abs = math.abs(this.ui.size - new UnityEngine.Vector2(scaler.designResolutionX, scaler.designResolutionY));
        if (abs.x < 0.1f)
        {
            if (abs.y < 0.1f)
                this.SetAdapt(AdaptStyle.FullScreen);
            else
                this.SetAdapt(AdaptStyle.Bottom);
        }
        else
        {
            if (abs.y > 0.1f)
                this.SetAdapt(AdaptStyle.Center);
            else
                this.SetAdapt(AdaptStyle.Left);
        }
    }

    void setConfig()
    {
        this._ui.sortingOrder = this.uiConfig.SortOrder + Game.UIConfig.SortOrderRange;
        Client.UI.FGUIRoot.AddChild(this._ui);
        this.AutoAdapt();
        this._ui.fairyBatching = true;
    }
    void _success()
    {
        this._states = UIStatus.Success;
        this._ui.visible = this.isShow;
        this.OnEnter(this.ParamObjects);
        if (_loadingView)
            UIGlobalConfig.LoadingView(this, false);
    }
    async void _delay1Handle()
    {
        await SValueTask.Delay(UIGlobalConfig.LoadingViewDelayBeginTimeMs);
        if (this.Disposed || this._states == UIStatus.Success) return;
        _loadingView = true;
        UIGlobalConfig.LoadingView(this, true);
    }
    async void _delay2Handle()
    {
        await SValueTask.Delay(UIGlobalConfig.LoadingViewTimeOutTimeMs);
        if (this.Disposed || this._states == UIStatus.Success) return;
        UIGlobalConfig.LoadingView(this, false);
        this.Dispose();
    }
}
#endif