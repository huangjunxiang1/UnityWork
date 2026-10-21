using Event;
using FairyGUI;
using main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

partial class FUILogin
{
    List<Type> _sceneTypes;
    protected override void OnEnter()
    {
        _acc.text = SettingL.Account;
        _acc.GetTextField().asTextInput.onChanged.Add(acc);
        _pw.text = SettingL.Password;
        _pw.GetTextField().asTextInput.onChanged.Add(pw);
        _btnLogin.onClick.Add(login);

        var baseType = typeof(Scene);
        _sceneTypes = this.GetType().Assembly.GetTypes().Where(t => t != null
                        && t.IsClass
                        && !t.IsAbstract
                        && t != baseType
                        && baseType.IsAssignableFrom(t)
                        && t != typeof(LoginScene))
            .ToList();
        _scenes.items = _sceneTypes.Select(t => t.Name).ToArray();
        _scenes.selectedIndex = PlayerPrefs.GetInt("_sceneTypes");
        _scenes.onChanged.Add(() =>
        {
            PlayerPrefs.SetInt("_sceneTypes", _scenes.selectedIndex);
        });
    }

    void acc() => SettingL.Account = _acc.text;
    void pw() => SettingL.Password = _pw.text;

    async void login()
    {
        var type = _sceneTypes[_scenes.selectedIndex];

        var method = typeof(Game)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .First(m => m.Name == nameof(Game.InScene));

        await (STask)method.MakeGenericMethod(type).Invoke(null, new object[] { new object[0] });
    }
}
