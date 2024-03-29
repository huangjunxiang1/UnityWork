using FairyGUI;
using System;
using Game;

abstract class FUI : FUIBase
{
    GComponent _ui;
    STask task;
    UIStates states;

    public sealed override UIStates uiStates => states;
    public sealed override GComponent ui => this._ui;
    public sealed override int sortOrder
    {
        get { return this._ui.sortingOrder; }
        set { this._ui.sortingOrder = value; }
    }
    public sealed override bool isShow
    {
        get { return this._ui.visible; }
        set { this._ui.visible = value; }
    }
    public sealed override STask onTask => task;
    public GObject Close { get; private set; }

    public sealed override STask LoadConfig(Game.UIConfig config, STask completed, params object[] data)
    {
        base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this._ui = UIPkg.ComPkg.CreateObject(this.url).asCom;
        (Close = this._ui.GetChild("Close"))?.onClick.Add(this.Dispose);
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
    public sealed override STask LoadConfigAsync(Game.UIConfig config, STask completed, params object[] data)
    {
        base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        STask loadTask = new STask();
        this.states = UIStates.Loading;
        UIPkg.ComPkg.CreateObjectAsync(this.url, obj =>
        {
            if (this.Disposed)
            {
                obj.Dispose();
                return;
            }
            this._ui = obj.asCom;
            (Close = this._ui.GetChild("Close"))?.onClick.Add(this.Dispose);
            this._ui.visible = false;
            this.Binding();
            this.setConfig();
            this.states = UIStates.OnTask;
            task = this.OnTask(data);
            task.AddEvent(() =>
            {
                this.states = UIStates.Success;
                this._ui.visible = true;
                this.OnEnter(data);
            });
            loadTask.TrySetResult();
        });
        return loadTask;
    }
    public override void Dispose()
    {
        if (this._ui != null && this.uiStates == UIStates.Success)
            this.Hide(true, this._ui.Dispose);
        base.Dispose();
    }

    void setConfig()
    {
        GRoot.inst.AddChild(this._ui);
        this._ui.MakeFullScreen();
        this._ui.AddRelation(GRoot.inst, RelationType.Size);
        this._ui.AddRelation(GRoot.inst, RelationType.Center_Center);
        this._ui.fairyBatching = true;

        int layer = this.Layer - 2;
        if (layer > 3)
            Loger.Error("层级太深");
        if (this.Parent is not UIBase)
            this._ui.sortingOrder = (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, 3 - layer);
        else
            this._ui.sortingOrder = ((UIBase)Parent).sortOrder + (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, 3 - layer);
    }
}
