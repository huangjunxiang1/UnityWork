using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System.Threading.Tasks;
using System;
using Event;
using main;

partial class FUILogin
{
    protected override void OnEnter()
    {
        _acc.text = SettingL.Account;
        _acc.GetTextField().asTextInput.onChanged.Add(acc);
        _pw.text = SettingL.Password;
        _pw.GetTextField().asTextInput.onChanged.Add(pw);
        _btnLogin.onClick.Add(login);
    }

    void acc() => SettingL.Account = _acc.text;
    void pw() => SettingL.Password = _pw.text;

    async void login()
    {
        await Game.Scene.InScene<WorldScene>();
    }
}
