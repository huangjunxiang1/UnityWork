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


    public override void InitConfig(UIConfig config, params object[] data)
    {
        base.InitConfig(config);

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

    public override void Dispose()
    {
        //��ִ���˳��߼�
        this.OnExit();
        base.Dispose();
        this.UI.Dispose();
        this.UI = null;
    }
}