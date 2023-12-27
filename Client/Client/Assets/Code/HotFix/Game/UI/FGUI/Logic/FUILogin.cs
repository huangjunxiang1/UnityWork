using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using FairyGUI;
using Main;
using System.Threading.Tasks;
using System;

partial class FUILogin
{
    static int demoIdx = 0;

    protected override async STask OnTask(params object[] data)
    {
        await this._bg.SetTexture("UI/Texture/BG/bg.jpg");
    }
    protected override void OnEnter(params object[] data)
    {
        _btnLogin.onClick.Add(login);
        _gameTypeCB.items = new string[]
        {
            "Inner",
            "Outer",
        };
        _gameTypeCB.selectedIndex = 1;
        _demo.selectedIndex = demoIdx;
        _uiType.onChanged.Add(onUIModel);
        _uiType.selectedIndex = 0;
        _acc.text = "t1";
        _pw.text = "1";
    }

    protected override void OnExit()
    {
        demoIdx = _demo.selectedIndex;
    }

    [Event]
    void connectRet(EC_NetError e)
    {
    }

    async void onUIModel()
    {
        if (_uiType.selectedIndex == 0)
            SSetting.UIModel = UIModel.FGUI;
        else
        {
            SSetting.UIModel = UIModel.UGUI;
            await SGameL.UI.OpenAsync<UUILogin>();
            this.Dispose();
        }
    }
   
    async void login()
    {
        await SGameL.Scene.InScene(10001);
        if (_demo.selectedIndex == 0)
            await SGameL.UI.OpenAsync<FUIFighting>();
        else if (_demo.selectedIndex == 1)
            await SGameL.UI.OpenAsync<FUIFighting2>();
        else if (_demo.selectedIndex == 2)
            await SGameL.UI.OpenAsync<FUIFighting3>();
        else if (_demo.selectedIndex == 3)
            await SGameL.UI.OpenAsync<FUIFighting4>();

        /*//链接服务器演示
        if (_gameTypeCB.selectedIndex == 0)
        {
            await GameM.Net.Connect(ServerType.TCP, Util.ToIPEndPoint(ConstDefM.LoginAddressInner));
        }
        else if (_gameTypeCB.selectedIndex == 1)
        {
            await GameM.Net.Connect(ServerType.TCP, Util.ToIPEndPoint(ConstDefM.LoginAddressOuter));
        }*/
    }
}
