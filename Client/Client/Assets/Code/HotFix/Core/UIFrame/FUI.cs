using FairyGUI;
using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

abstract class FUI : FUIBase
{
    GComponent ui;
    STask task;
    UIStates states;

    public sealed override UIStates uiStates => states;
    public sealed override GComponent UI => this.ui;
    public sealed override int sortOrder
    {
        get { return this.ui.sortingOrder; }
        set { this.ui.sortingOrder = value; }
    }
    public sealed override bool isShow
    {
        get { return this.ui.visible; }
        set { this.ui.visible = value; }
    }
    public sealed override STask onTask => task;

    public sealed override STask LoadConfig(Main.UIConfig config, STask completed, params object[] data)
    {
        base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this.ui = UIPkg.ComPkg.CreateObject(this.url).asCom;
        this.ui.GetChild("Close")?.onClick.Add(this.Dispose);
        this.Binding();
        this.setConfig();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        this.states = UIStates.Success;
        this.OnEnter(data);
        return STask.Completed;
    }
    public sealed override STask LoadConfigAsync(Main.UIConfig config, STask completed, params object[] data)
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
            this.ui = obj.asCom;
            this.ui.GetChild("Close")?.onClick.Add(this.Dispose);
            this.ui.visible = false;
            this.Binding();
            this.setConfig();
            this.states = UIStates.OnTask;
            task = this.OnTask(data);
            task.AddEvent(() =>
            {
                this.states = UIStates.Success;
                this.ui.visible = true;
            });
            this.OnEnter(data);
            loadTask.TrySetResult();
        });
        return loadTask;
    }
    public sealed override void Dispose()
    {
        base.Dispose();

        if (this.ui != null && this.uiStates == UIStates.Success)
        {
            this.Hide(true, () =>
            {
                this.ui.Dispose();
                this.ui = null;
            });
        }
    }

    void setConfig()
    {
        GRoot.inst.AddChild(this.ui);
        this.ui.MakeFullScreen();
        this.ui.AddRelation(GRoot.inst, RelationType.Size);
        this.ui.AddRelation(GRoot.inst, RelationType.Center_Center);
        this.ui.fairyBatching = true;

        int layer = this.Layer;
        if (layer > 3)
            Loger.Error("层级太深");
        if (this.Parent == null)
            this.ui.sortingOrder = (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, 3 - layer);
        else
            this.ui.sortingOrder = Parent.sortOrder + (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, 3 - layer);
    }
}
