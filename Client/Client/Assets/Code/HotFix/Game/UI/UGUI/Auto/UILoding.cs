using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;
using UnityEngine.UI;
using Game;

partial class UILoding : UUIBase
{
    public UnityEngine.UI.Image _fillImage;
    public UnityEngine.UI.Text _txtText;

    protected override void Binding()
    {
        this._fillImage = this.UI.transform.Find("_fill").GetComponent(typeof(UnityEngine.UI.Image)) as UnityEngine.UI.Image;
        this._txtText = this.UI.transform.Find("_txt").GetComponent(typeof(UnityEngine.UI.Text)) as UnityEngine.UI.Text;

    }
}