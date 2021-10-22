using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using UnityEngine;
using UnityEngine.UI;
using Game;

partial class UUILogin : UUIBase
{
    public UnityEngine.UI.InputField _acInputField;
    public UnityEngine.UI.InputField _pwInputField;
    public UnityEngine.UI.Button _loginButton;
    public UnityEngine.UI.Dropdown _DropdownGameTypeDropdown;
    public UnityEngine.UI.Dropdown _DropdownUITypeDropdown;

    protected override void Binding()
    {
        this._acInputField = this.UI.transform.Find("_ac").GetComponent(typeof(UnityEngine.UI.InputField)) as UnityEngine.UI.InputField;
        this._pwInputField = this.UI.transform.Find("_pw").GetComponent(typeof(UnityEngine.UI.InputField)) as UnityEngine.UI.InputField;
        this._loginButton = this.UI.transform.Find("_login").GetComponent(typeof(UnityEngine.UI.Button)) as UnityEngine.UI.Button;
        this._DropdownGameTypeDropdown = this.UI.transform.Find("_DropdownGameType").GetComponent(typeof(UnityEngine.UI.Dropdown)) as UnityEngine.UI.Dropdown;
        this._DropdownUITypeDropdown = this.UI.transform.Find("_DropdownUIType").GetComponent(typeof(UnityEngine.UI.Dropdown)) as UnityEngine.UI.Dropdown;

    }
}