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
    public UnityEngine.UI.Image _fillImage;
    public UnityEngine.UI.Text _txtText;

    protected sealed override void Binding()
    {
        RectTransform ui = this.UI;
        Transform c;
        c = ui.GetChild(0);
        this._fillImage = (UnityEngine.UI.Image)c.GetComponent(typeof(UnityEngine.UI.Image));
        c = ui.GetChild(1);
        this._txtText = (UnityEngine.UI.Text)c.GetComponent(typeof(UnityEngine.UI.Text));
    }
}
partial class UUILogin : UUI
{
    public sealed override string url => "UI/UUI/Prefab/UUILogin.prefab";
    public UnityEngine.UI.InputField _acInputField;
    public UnityEngine.UI.InputField _pwInputField;
    public UnityEngine.UI.Button _loginButton;
    public UnityEngine.UI.Dropdown _DropdownUITypeDropdown;
    public UnityEngine.UI.Dropdown _DropdownGameTypeDropdown;

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
        this._DropdownUITypeDropdown = (UnityEngine.UI.Dropdown)c.GetComponent(typeof(UnityEngine.UI.Dropdown));
        c = ui.GetChild(4);
        this._DropdownGameTypeDropdown = (UnityEngine.UI.Dropdown)c.GetComponent(typeof(UnityEngine.UI.Dropdown));
    }
}