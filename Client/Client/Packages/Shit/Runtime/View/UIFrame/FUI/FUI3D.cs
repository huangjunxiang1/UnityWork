using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using FairyGUI;
using Game;

public abstract class FUI3D : FUIBase
{
    STask task;
    UIStates states;

    public sealed override UIStates uiStates => states;
    public sealed override GComponent ui => this.Panel.ui;
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

    public sealed override STask LoadConfig(Game.UIConfig config, STask completed, params object[] data)
    {
        base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this.goRoot = SAsset.LoadGameObject(url, ReleaseMode.Destroy);
        this.goRoot.transform.SetParent(Client.transform);
        this.Panel = this.goRoot.GetComponentInChildren<UIPanel>();

        this.Binding();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        task.AddEvent(() =>
        {
            this.states = UIStates.Success;
            this.OnEnter(data);
        });
        return STask.Completed;
    }
    public sealed override async STask LoadConfigAsync(Game.UIConfig config, STask completed, params object[] data)
    {
        _ = base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this.goRoot = await SAsset.LoadGameObjectAsync(url, ReleaseMode.Destroy);
        this.goRoot.transform.SetParent(Client.transform);
        this.Panel = this.goRoot.GetComponentInChildren<UIPanel>();

        this.Binding();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        task.AddEvent(() =>
        {
            this.states = UIStates.Success;
            this.OnEnter(data);
        });
    }
    public override void Dispose()
    {
        if (this.goRoot != null)
            this.Hide(true, () => SAsset.Release(this.goRoot));
        base.Dispose();
    }
}
