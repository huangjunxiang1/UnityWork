using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Main;
using FairyGUI;
using Game;

abstract class FUI3D : FUIBase
{
    public sealed override GComponent UI => this.Panel.ui;

    public override int SortOrder
    {
        get { return this.Panel.sortingOrder; }
        set { this.Panel.sortingOrder = value; }
    }

    public override bool IsShow
    {
        get { return this.Root.activeSelf; }
        set { this.Root.SetActive(value); }
    }

    public GameObject Root { get; private set; }
    public UIPanel Panel { get; private set; }

    public sealed override void LoadConfig(UIConfig config, params object[] data)
    {
        base.LoadConfig(config, data);

        this.OnAwake(data);
        this.Root = AssetLoad.LoadGameObject(url);
        this.Root.transform.SetParent(GameM.World.Root.transform);
        this.Panel = this.Root.GetComponentInChildren<UIPanel>();
        this.Panel.sortingOrder = config.SortOrder;

        this.Binding();
        this.OnEnter(data);
        this.EnterWaiter = this.OnEnterAsync(data);
    }
    public sealed override async void LoadConfigAsync(UIConfig config, params object[] data)
    {
        base.LoadConfigAsync(config, data);

        this.OnAwake(data);
        this.Root = await AssetLoad.LoadGameObjectAsync(url, TaskCreater);
        this.Root.transform.SetParent(GameM.World.Root.transform);
        this.Panel = this.Root.GetComponentInChildren<UIPanel>();
        this.Panel.sortingOrder = config.SortOrder;

        this.Binding();
        this.OnEnter(data);
        this.EnterWaiter = this.OnEnterAsync(data);
        this.LoadWaiter.TrySetResult();
    }
    public sealed override void Dispose()
    {
        base.Dispose();
        AssetLoad.Release(this.Root);
    }
}
