using Game;
using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

abstract class UUI3D : UUIBase
{
    RectTransform ui;
    Canvas canvas;
    STask task;
    UIStates states;

    public sealed override UIStates uiStates => states;
    public sealed override Canvas Canvas => this.canvas;
    public sealed override RectTransform UI => this.ui;
    public sealed override STask onTask => task;

    public sealed override STask LoadConfig(SUIConfig config, STask completed, params object[] data)
    {
        base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this.ui = (RectTransform)SAsset.LoadGameObject(url).transform;
        this.ui.SetParent(SGameL.UI.UGUIRoot);
        this.ui.localScale = Vector3.one;
        this.ui.rotation = default;
        this.ui.anchoredPosition = default;
        this.canvas = this.ui.GetComponentInChildren<Canvas>();

        this.Binding();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        this.states = UIStates.Success;
        this.OnEnter(data);
        return STask.Completed;
    }
    public sealed override async STask LoadConfigAsync(SUIConfig config, STask completed, params object[] data)
    {
        _ = base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        GameObject ui = await SAsset.LoadGameObjectAsync(url);
        this.ui = (RectTransform)ui.transform;
        this.ui.SetParent(SGameL.UI.UGUIRoot);
        this.ui.localScale = Vector3.one;
        this.ui.rotation = default;
        this.ui.anchoredPosition = default;
        this.canvas = this.ui.GetComponentInChildren<Canvas>();

        this.Binding();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        task.AddEvent(() => this.states = UIStates.Success);
        this.OnEnter(data);
    }
}
