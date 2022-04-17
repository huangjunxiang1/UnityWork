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
    public abstract Canvas Canvas { get; }

    public abstract RectTransform UI { get; }

    public override int SortOrder
    {
        get { return this.Canvas.sortingOrder; }
        set { this.Canvas.sortingOrder = value; }
    }

    public override bool IsShow
    {
        get { return this.UI.gameObject.activeSelf; }
        set { this.UI.gameObject.SetActive(value); }
    }

    public sealed override void Dispose()
    {
        base.Dispose();
        //ui不做池管理
        AssetLoad.ReleaseTextureRef(this.UI.gameObject);
        AssetLoad.Release(this.UI.gameObject);
    }
}