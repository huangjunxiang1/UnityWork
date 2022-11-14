using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

abstract class FUI : FUIBase
{
    public sealed override GComponent UI => this.ui;

    public override int SortOrder
    {
        get { return this.ui.sortingOrder; }
        set { this.ui.sortingOrder = value; }
    }

    public override bool IsShow
    {
        get { return this.ui.visible; }
        set { this.ui.visible = value; }
    }

    GComponent ui;

    public sealed override void LoadConfig(UIConfig config, params object[] data)
    {
        base.LoadConfig(config, data);

        this.OnAwake(data);
        this.ui = UIConfig.ComPkg.CreateObject(this.url).asCom;
        GRoot.inst.AddChild(this.ui);
        this.ui.MakeFullScreen();
        this.ui.AddRelation(GRoot.inst, RelationType.Size);
        this.ui.AddRelation(GRoot.inst, RelationType.Center_Center);
        this.ui.fairyBatching = true;
        this.ui.sortingOrder = config.SortOrder;

        this.Binding();
        this.OnEnter(data);
        this.EnterWaiter = this.OnEnterAsync(data);
    }
    public sealed override void LoadConfigAsync(UIConfig config, params object[] data)
    {
        base.LoadConfigAsync(config, data);

        this.OnAwake(data);
        UIConfig.ComPkg.CreateObjectAsync(this.url, obj =>
        {
            if (this.Disposed)
            {
                obj.Dispose();
                return;
            }
            this.ui = obj.asCom;
            GRoot.inst.AddChild(this.ui);
            this.ui.MakeFullScreen();
            this.ui.AddRelation(GRoot.inst, RelationType.Size);
            this.ui.AddRelation(GRoot.inst, RelationType.Center_Center);
            this.ui.fairyBatching = true;
            this.ui.sortingOrder = config.SortOrder;

            this.Binding();
            this.OnEnter(data);
            this.EnterWaiter = this.OnEnterAsync(data);
            this.LoadWaiter.TrySetResult();
        });
    }

    public sealed override void Dispose()
    {
        base.Dispose();
        this.ui?.Dispose();
        this.ui = null;
    }
}
