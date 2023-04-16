using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;
using UnityEngine.UI;
using Game;

partial class UUILoading : UUI
{
    public sealed override string url => "UI/UUI/Prefab/UUILoading.prefab";
    public PropertyBinding<UnityEngine.UI.Image, float> _fillImageBinding;
    public PropertyBinding<UnityEngine.UI.Text, float> _txtTextBinding;

    protected sealed override void Binding()
    {
        RectTransform ui = this.UI;
        Transform c;
        c = ui.GetChild(0);
        this._fillImageBinding = new PropertyBinding<UnityEngine.UI.Image, float>((UnityEngine.UI.Image)c.GetComponent(typeof(UnityEngine.UI.Image)));
        c = ui.GetChild(1);
        this._txtTextBinding = new PropertyBinding<UnityEngine.UI.Text, float>((UnityEngine.UI.Text)c.GetComponent(typeof(UnityEngine.UI.Text)));
        this.VMBinding();
        this._fillImageBinding.CallEvent();
        this._txtTextBinding.CallEvent();
    }
}
partial class UUILogin : UUI
{
    public sealed override string url => "UI/UUI/Prefab/UUILogin.prefab";
    public UnityEngine.UI.InputField _acInputField;
    public UnityEngine.UI.InputField _pwInputField;
    public UnityEngine.UI.Button _loginButton;
    public UnityEngine.UI.Dropdown _UITypeDropdown;
    public UnityEngine.UI.Dropdown _GameTypeDropdown;

    protected sealed override void Binding()
    {
        RectTransform ui = this.UI;
        Transform c;
        c = ui.GetChild(0);
        this._acInputField = (UnityEngine.UI.InputField)c.GetComponent(typeof(UnityEngine.UI.InputField));
        c = ui.GetChild(1);
        this._pwInputField = (UnityEngine.UI.InputField)c.GetComponent(typeof(UnityEngine.UI.InputField));
        c = ui.GetChild(2);
        this._loginButton = (UnityEngine.UI.Button)c.GetComponent(typeof(UnityEngine.UI.Button));
        c = ui.GetChild(3);
        this._UITypeDropdown = (UnityEngine.UI.Dropdown)c.GetComponent(typeof(UnityEngine.UI.Dropdown));
        c = ui.GetChild(4);
        this._GameTypeDropdown = (UnityEngine.UI.Dropdown)c.GetComponent(typeof(UnityEngine.UI.Dropdown));
        this.VMBinding();
    }
}