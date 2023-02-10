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
    TaskAwaiter task;
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
    public sealed override TaskAwaiter onTask => task;
    public GameObject goRoot { get; private set; }
    public UIPanel Panel { get; private set; }

    public sealed override TaskAwaiter LoadConfig(Main.UIConfig config, TaskAwaiter completed, params object[] data)
    {
        base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this.goRoot = AssetLoad.LoadGameObject(url);
        this.goRoot.transform.SetParent(GameM.World.goRoot.transform);
        this.Panel = this.goRoot.GetComponentInChildren<UIPanel>();

        this.Binding();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        this.states = UIStates.Success;
        this.OnEnter(data);
        return TaskAwaiter.Completed;
    }
    public sealed override async TaskAwaiter LoadConfigAsync(Main.UIConfig config, TaskAwaiter completed, params object[] data)
    {
        _ = base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this.goRoot = await AssetLoad.LoadGameObjectAsync(url, TaskManager);
        this.goRoot.transform.SetParent(GameM.World.goRoot.transform);
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
                AssetLoad.Release(this.goRoot);
            });
        }
    }
}
