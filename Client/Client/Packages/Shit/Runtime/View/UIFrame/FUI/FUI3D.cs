using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
 
#if FairyGUI
using FairyGUI;
using Game;

public abstract class FUI3D : FUIBase
{
    STask task;
    UIStatus states;

    public sealed override UIStatus uiStates => states;
    public sealed override GComponent ui => this.Panel.ui;
    public sealed override int sortOrder
    {
        get { return this.Panel.sortingOrder; }
        set { this.Panel.SetSortingOrder(value, true); }
    }
    public sealed override bool isShow
    {
        get { return base.isShow; }
        set
        {
            base.isShow = value;
            if (this.goRoot)
                this.goRoot.SetActive(value);
        }
    }
    public sealed override STask onTask => task;
    public GameObject goRoot { get; private set; }
    public UIPanel Panel { get; private set; }

    public sealed override async STask LoadConfig(Game.UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStatus.Loading;
        this.goRoot = SAsset.LoadGameObject(url, ReleaseMode.Destroy);
        this.goRoot.transform.SetParent(Client.transform);
        this.Panel = this.goRoot.GetComponentInChildren<UIPanel>();

        this.Binding();
        this.states = UIStatus.OnTask;
        await (task = this.OnTask(data));
        if (this.Disposed) return;
        this.states = UIStatus.Success;
        this.goRoot.SetActive(this.isShow);
        this.OnEnter(data);
    }
    public sealed override async STask LoadConfigAsync(Game.UIConfig config, STask completed, params object[] data)
    {
        await base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this.states = UIStatus.Loading;
        this.goRoot = await SAsset.LoadGameObjectAsync(url, ReleaseMode.Destroy);
        this.goRoot.transform.SetParent(Client.transform);
        this.Panel = this.goRoot.GetComponentInChildren<UIPanel>();

        this.Binding();
        this.states = UIStatus.OnTask;
        await (task = this.OnTask(data));
        if (this.Disposed) return;
        this.states = UIStatus.Success;
        this.goRoot.SetActive(this.isShow);
        this.OnEnter(data);
    }
    public override void Dispose()
    {
        if (this.goRoot)
        {
            if (this.uiStates == UIStatus.Success)
                this.Hide(true, () => SAsset.Release(this.goRoot));
            else
                SAsset.Release(this.goRoot);
        }
        base.Dispose();
    }
}
#endif