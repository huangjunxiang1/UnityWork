using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game;

abstract class UUI : UUIBase
{
    RectTransform ui;
    Canvas canvas;
    TaskAwaiter task;
    UIStates states;

    public sealed override UIStates uiStates => states;
    public sealed override Canvas Canvas => this.canvas;
    public sealed override RectTransform UI => this.ui;
    public sealed override TaskAwaiter onTask => task;

    public sealed override TaskAwaiter LoadConfig(UIConfig config, TaskAwaiter completed, params object[] data)
    {
        base.LoadConfig(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        this.ui = (RectTransform)AssetLoad.LoadGameObject(url).transform;
        this.canvas = this.ui.GetComponent<Canvas>();
        this.Binding();
        this.setConfig();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        this.states = UIStates.Success;
        this.OnEnter(data);
        return TaskAwaiter.Completed;
    }
    public sealed override async TaskAwaiter LoadConfigAsync(UIConfig config, TaskAwaiter completed, params object[] data)
    {
        _ = base.LoadConfigAsync(config, completed, data);

        this.OnAwake(data);
        this.states = UIStates.Loading;
        GameObject g = await AssetLoad.LoadGameObjectAsync(url, TaskManager);
        g.SetActive(false);
        this.ui = (RectTransform)g.transform;
        this.canvas = this.ui.GetComponent<Canvas>();
        this.Binding();
        this.setConfig();
        this.states = UIStates.OnTask;
        task = this.OnTask(data);
        task.AddEvent(() =>
        {
            this.states = UIStates.Success;
            g.SetActive(true);
        });
        this.OnEnter(data);
    }

    void setConfig()
    {
        this.ui.SetParent(GameL.UI.UGUIRoot);
        this.ui.localScale = Vector3.one;
        this.ui.rotation = default;
        this.ui.sizeDelta = GameL.UI.UGUIRoot.sizeDelta;
        this.ui.anchorMin = default;
        this.ui.anchorMax = Vector2.one;
        this.ui.anchoredPosition = default;

        int layer = this.Layer;
        if (layer > 3)
            Loger.Error("层级太深");
        if (this.Parent == null)
            this.canvas.sortingOrder = (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, 3 - layer);
        else
            this.canvas.sortingOrder = Parent.sortOrder + (this.uiConfig.SortOrder + 100) * (int)Math.Pow(100, 3 - layer);
    }
}
