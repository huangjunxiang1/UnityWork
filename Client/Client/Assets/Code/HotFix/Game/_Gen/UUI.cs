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
    public override string url => "UI/UUI/Prefab/UUILoading.prefab";
    public UnityEngine.UI.Image _fillImage;
    public UnityEngine.UI.Text _txtText;

    protected override void Binding()
    {
        this._fillImage = (UnityEngine.UI.Image)this.UI.transform.Find("_fill").GetComponent(typeof(UnityEngine.UI.Image));
        this._txtText = (UnityEngine.UI.Text)this.UI.transform.Find("_txt").GetComponent(typeof(UnityEngine.UI.Text));

    }
}
partial class UUILogin : UUI
{
    public override string url => "UI/UUI/Prefab/UUILogin.prefab";
    public UnityEngine.UI.InputField _acInputField;
    public UnityEngine.UI.InputField _pwInputField;
    public UnityEngine.UI.Button _loginButton;
    public UnityEngine.UI.Dropdown _DropdownUITypeDropdown;
    public UnityEngine.UI.Dropdown _DropdownGameTypeDropdown;

    protected override void Binding()
    {
        this._acInputField = (UnityEngine.UI.InputField)this.UI.transform.Find("_ac").GetComponent(typeof(UnityEngine.UI.InputField));
        this._pwInputField = (UnityEngine.UI.InputField)this.UI.transform.Find("_pw").GetComponent(typeof(UnityEngine.UI.InputField));
        this._loginButton = (UnityEngine.UI.Button)this.UI.transform.Find("_login").GetComponent(typeof(UnityEngine.UI.Button));
        this._DropdownUITypeDropdown = (UnityEngine.UI.Dropdown)this.UI.transform.Find("_DropdownUIType").GetComponent(typeof(UnityEngine.UI.Dropdown));
        this._DropdownGameTypeDropdown = (UnityEngine.UI.Dropdown)this.UI.transform.Find("_DropdownGameType").GetComponent(typeof(UnityEngine.UI.Dropdown));

    }
}