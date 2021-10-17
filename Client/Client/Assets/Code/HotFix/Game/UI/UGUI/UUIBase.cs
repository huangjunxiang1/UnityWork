using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;
using Game;
abstract class UUIBase : EntityL
{
    Canvas _uiCanvas;
    Eventer _onDispose;

    public UIConfig uiConfig { get; private set; }

    public GameObject UI { get; private set; }

    public int SortOrder
    {
        get { return this._uiCanvas.sortingOrder; }
        set { this._uiCanvas.sortingOrder = value; }
    }

    public bool isHide
    {
        get { return this.UI.gameObject.activeSelf; }
        set { this.UI.gameObject.SetActive(value); }
    }

    /// <summary>
    /// dispose 监听
    /// </summary>
    public Eventer OnDispose
    {
        get
        {
            if (_onDispose == null) _onDispose = new Eventer(this);
            return _onDispose;
        }
    }

    protected abstract void OnEnter(params object[] data);
    protected abstract void OnExit();
    protected abstract void Binding();


    public void Init(UIConfig config, params object[] data)
    {
        this.uiConfig = config;
        this.UI = AssetLoad.Load<GameObject>("UI/UUI/UIPrefab/" + this.GetType().Name+ ".prefab");
        this._uiCanvas = this.UI.GetComponent<Canvas>();
        this._uiCanvas.sortingOrder = config.SortOrder;

        this.Binding();
        this.UI.transform.SetParent(UIManager.Inst.UIRoot);
        this.UI.transform.localPosition = default;
        this.UI.transform.localScale = Vector3.one;
        this.UI.transform.rotation = default;
        this.OnEnter(data);
    }

    public void Hide()
    {
        this.Hide(false);
    }
    public void Hide(bool playAnimation)
    {
        if (playAnimation) this.isHide = true;
        else this.isHide = true;
    }
    public void Show()
    {
        this.Show(false);
    }
    public void Show(bool playAnimation)
    {
        if (playAnimation) this.isHide = false;
        else this.isHide = false;
    }

    public override void Dispose()
    {
        base.Dispose();
        UIManager.Inst.Remove(this);
        this.OnExit();
        AssetLoad.Return(this.UI);
        if (_onDispose != null) _onDispose.Call();
    }
}