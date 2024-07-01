using System;

#if FairyGUI
using FairyGUI;
using Game;
public abstract class FUI : FUIBase
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

    public sealed override async STask LoadConfig(Game.UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this._ui = FUIBase.CreateUI(this, this.url);
        (Close = this._ui.GetChild("Close"))?.onClick.Add(this.Dispose);
        if (Close == null)
            (Close = this._ui.GetChild("Lable")?.asCom?.GetChild("Close"))?.onClick.Add(this.Dispose);
        if (Close == null)
            (Close = this._ui.GetChild("_Lable")?.asCom?.GetChild("Close"))?.onClick.Add(this.Dispose);
        this._ui.visible = false;
        this.Binding();
        this.setConfig();
        this.states = UIStates.OnTask;
        await (task = this.OnTask(data));
        if (this.Disposed) return;
        this.states = UIStates.Success;
        this._ui.visible = true;
        this.OnEnter(data);
    }
    public sealed override async STask LoadConfigAsync(Game.UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;

        this._ui = await FUIBase.CreateUIAsync(this, this.url);
        if (this.Disposed)
        {
            this._ui.Dispose();
            return;
        }
        (Close = this._ui.GetChild("Close"))?.onClick.Add(this.Dispose);
        if (Close == null)
            (Close = this._ui.GetChild("Lable")?.asCom?.GetChild("Close"))?.onClick.Add(this.Dispose);
        if (Close == null)
            (Close = this._ui.GetChild("_Lable")?.asCom?.GetChild("Close"))?.onClick.Add(this.Dispose);
        this._ui.visible = false;
        this.Binding();
        this.setConfig();
        this.states = UIStates.OnTask;
        await (task = this.OnTask(data));
        if (this.Disposed) return;
        this.states = UIStates.Success;
        this._ui.visible = true;
        this.OnEnter(data);
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

        int layer = this.CrucialLayer;
        if (layer > 3)
            Loger.Error("层级太深 layer=" + layer);
        if (this.Parent is not UIBase)
            this._ui.sortingOrder = (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, 3 - layer);
        else
            this._ui.sortingOrder = ((UIBase)Parent).sortOrder + (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, 3 - layer);
    }
}
#endif