using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

abstract class UUI3D : UUIBase
{
    public sealed override Canvas Canvas => this.canvas;
    public sealed override RectTransform UI => this.ui;

    RectTransform ui;
    Canvas canvas;

    public sealed override void LoadConfig(UIConfig config, params object[] data)
    {
        base.LoadConfig(config, data);

        this.OnInit(data);
        this.ui = (RectTransform)AssetLoad.LoadGameObject(url).transform;
        this.ui.SetParent(UIS.UGUIRoot);
        this.ui.localScale = Vector3.one;
        this.ui.rotation = default;
        this.ui.anchoredPosition = default;
        this.canvas = this.ui.GetComponentInChildren<Canvas>();
        this.canvas.sortingOrder = config.SortOrder;

        this.Binding();
        this.OnEnter(data);
    }
    public sealed override async void LoadConfigAsync(UIConfig config, params object[] data)
    {
        base.LoadConfigAsync(config, data);

        this.OnInit(data);
        GameObject ui = await AssetLoad.LoadGameObjectAsync(url, TaskCreater);
        this.ui = (RectTransform)ui.transform;
        this.ui.SetParent(UIS.UGUIRoot);
        this.ui.localScale = Vector3.one;
        this.ui.rotation = default;
        this.ui.anchoredPosition = default;
        this.canvas = this.ui.GetComponentInChildren<Canvas>();
        this.canvas.sortingOrder = config.SortOrder;

        this.Binding();
        this.OnEnter(data);
        this.LoadWaiter.TrySetResult();
    }
}
