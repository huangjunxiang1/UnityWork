using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;
using Game;
using FairyGUI;

abstract class FUIBase : UIBase
{
    public GComponent UI { get; private set; }

    public override int SortOrder
    {
        get { return this.UI.sortingOrder; }
        set { this.UI.sortingOrder = value; }
    }

    public override bool isHide
    {
        get { return this.UI.visible; }
        set { this.UI.visible = value; }
    }

    public override void LoadConfig(UIConfig config,params object[] data)
    {
        base.LoadConfig(config, data);

        this.UI = UIPackage.CreateObject("ComPkg", this.GetType().Name).asCom;
        GRoot.inst.AddChild(this.UI);
        this.UI.MakeFullScreen();
        this.UI.AddRelation(GRoot.inst, RelationType.Size);
        this.UI.AddRelation(GRoot.inst, RelationType.Center_Center);
        this.UI.fairyBatching = true;
        this.UI.sortingOrder = config.SortOrder;

        this.Binding();
        this.OnEnter(data);
    }
    public override void LoadConfigAsync(UIConfig config, params object[] data)
    {
        base.LoadConfigAsync(config, data);

        UIPackage.CreateObjectAsync("ComPkg", this.GetType().Name, obj =>
         {
             if (this.Disposed)
             {
                 obj.Dispose();
                 return;
             }
             this.UI = obj.asCom;
             GRoot.inst.AddChild(this.UI);
             this.UI.MakeFullScreen();
             this.UI.AddRelation(GRoot.inst, RelationType.Size);
             this.UI.AddRelation(GRoot.inst, RelationType.Center_Center);
             this.UI.fairyBatching = true;
             this.UI.sortingOrder = config.SortOrder;

             this.Binding();
             this.OnEnter(data);
             this.LoadWaiter.TrySetResult();
         });
    }

    public override void Dispose()
    {
        //ÏÈÖ´ÐÐÍË³öÂß¼­
        this.OnExit();
        base.Dispose();
        this.UI.Dispose();
        this.UI = null;
    }
}
