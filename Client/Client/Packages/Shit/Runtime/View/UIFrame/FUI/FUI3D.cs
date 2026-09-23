using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
 
#if FairyGUI
using FairyGUI;

public abstract class FUI3D : FUIBase
{
    STask _task;
    UIStatus _states;

    public sealed override UIStatus uiStates => _states;
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
    public sealed override STask onTask => _task;
    public GameObject goRoot { get; private set; }
    public UIPanel Panel { get; private set; }

    public sealed override async STask LoadConfig(UIConfig config, STask completed)
    {
        await base.LoadConfig(config, completed);

        this.OnAwake();
        this._states = UIStatus.Loading;
        this.goRoot = Game.Scene.Current.Loader.LoadGameObject(this.url, ReleaseMode.Destroy);
        this.goRoot.transform.SetParent(Game.World.transform);
        this.Panel = this.goRoot.GetComponentInChildren<UIPanel>();

        this.Binding();
        this._states = UIStatus.OnTask;
        await (_task = this.OnTask());
        if (this.Disposed) return;
        this._states = UIStatus.Success;
        this.goRoot.SetActive(this.isShow);
        this.OnEnter();
    }
    public sealed override async STask LoadConfigAsync(UIConfig config, STask completed)
    {
        await base.LoadConfigAsync(config, completed);

        this.OnAwake();
        this._states = UIStatus.Loading;
        this.goRoot = await Game.Scene.Current.Loader.LoadGameObjectAsync(this.url, ReleaseMode.Destroy);
        if (this.Disposed) return;
        this.goRoot.transform.SetParent(Game.World.transform);
        this.Panel = this.goRoot.GetComponentInChildren<UIPanel>();

        this.Binding();
        this._states = UIStatus.OnTask;
        await (_task = this.OnTask());
        if (this.Disposed) return;
        this._states = UIStatus.Success;
        this.goRoot.SetActive(this.isShow);
        this.OnEnter();
    }
    public override void Dispose()
    {
        if (this.goRoot)
        {
            if (this.uiStates == UIStatus.Success)
                this.Hide(true, () => Game.Scene.Current.Loader.Release(this.goRoot));
            else
                Game.Scene.Current.Loader.Release(this.goRoot);
        }
        base.Dispose();
    }
}
#endif