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
    STask task;
    UIStates states;

    public sealed override UIStates uiStates => states;
    public sealed override GComponent UI => this.Panel.ui;
    public sealed override int sortOrder
    {
        get { return this.Panel.sortingOrder; }
        set { this.Panel.SetSortingOrder(value, true); }
    }
    public sealed override bool isShow
    {
        get { return this.goRoot.activeSelf; }
        set { this.goRoot.SetActive(value); }
    }
    public sealed override STask onTask => task;
    public GameObject goRoot { get; private set; }
    public UIPanel Panel { get; private set; }

    public sealed override STask LoadConfig(Main.SUIConfig config, STask completed, params object[] data)
    {
        base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this.goRoot = SAsset.LoadGameObject(url);
        this.goRoot.transform.SetParent(SGameM.World.GameRoot.transform);
        this.Panel = this.goRoot.GetComponentInChildren<UIPanel>();

        this.Binding();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        this.states = UIStates.Success;
        this.OnEnter(data);
        return STask.Completed;
    }
    public sealed override async STask LoadConfigAsync(Main.SUIConfig config, STask completed, params object[] data)
    {
        _ = base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this.goRoot = await SAsset.LoadGameObjectAsync(url);
        this.goRoot.transform.SetParent(SGameM.World.GameRoot.transform);
        this.Panel = this.goRoot.GetComponentInChildren<UIPanel>();

        this.Binding();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        task.AddEvent(() => this.states = UIStates.Success);
        this.OnEnter(data);
    }
    public sealed override void Dispose()
    {
        base.Dispose();

        if (this.goRoot != null)
        {
            this.Hide(true, () =>
            {
                SAsset.Release(this.goRoot);
            });
        }
    }
}
