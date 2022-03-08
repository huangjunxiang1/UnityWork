using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;
using Game;
abstract class UUIBase : UIBase
{
    Canvas _uiCanvas;

    public RectTransform UI { get; private set; }

    public override int SortOrder
    {
        get { return this._uiCanvas.sortingOrder; }
        set { this._uiCanvas.sortingOrder = value; }
    }

    public override bool isHide
    {
        get { return this.UI.gameObject.activeSelf; }
        set { this.UI.gameObject.SetActive(value); }
    }

    public override void LoadConfig(UIConfig config, params object[] data)
    {
        base.LoadConfig(config, data);

        this.UI = (RectTransform)AssetLoad.PrefabLoader.Load("UI/UUI/UIPrefab/" + this.GetType().Name + ".prefab").transform;
        this.UI.SetParent(UIS.UGUIRoot);
        this.UI.localScale = Vector3.one;
        this.UI.rotation = default;
        this.UI.sizeDelta = UIS.UGUIRoot.sizeDelta;
        this.UI.anchorMin = default;
        this.UI.anchorMax = Vector2.one;
        this.UI.anchoredPosition = default;
        this._uiCanvas = this.UI.GetComponent<Canvas>();
        this._uiCanvas.sortingOrder = config.SortOrder;

        this.Binding();
        this.OnEnter(data);
    }
    public override async void LoadConfigAsync(UIConfig config, params object[] data)
    {
        base.LoadConfigAsync(config, data);

        GameObject ui = await AssetLoad.PrefabLoader.LoadAsync("UI/UUI/UIPrefab/" + this.GetType().Name + ".prefab", TaskCreater.Create<GameObject>());
        if (this.Disposed)
        {
            AssetLoad.PrefabLoader.ReleaseDontReturnPool(ui);
            return;
        }
        this.UI = (RectTransform)ui.transform;
        this.UI.SetParent(UIS.UGUIRoot);
        this.UI.localScale = Vector3.one;
        this.UI.rotation = default;
        this.UI.sizeDelta = UIS.UGUIRoot.sizeDelta;
        this.UI.anchorMin = default;
        this.UI.anchorMax = Vector2.one;
        this.UI.anchoredPosition = default;
        this._uiCanvas = this.UI.GetComponent<Canvas>();
        this._uiCanvas.sortingOrder = config.SortOrder;

        this.Binding();
        this.OnEnter(data);
        this.LoadWaiter.TrySetResult();
    }

    public override void Dispose()
    {
        //先执行退出逻辑
        this.OnExit();
        base.Dispose();
        //ui不做池管理
        AssetLoad.PrefabLoader.ReleaseAllTexture(this.UI.gameObject);
        AssetLoad.PrefabLoader.ReleaseDontReturnPool(this.UI.gameObject);
        this.UI = null;
    }
}