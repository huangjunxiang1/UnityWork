using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using FairyGUI;
using Main;

partial class FUILogin
{
    static int demoIdx = 0;
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

    [Msg(CMDL.R2C_Login)]
    void R2C_Login(IMessage message)
    {
        R2C_Login rep = message as R2C_Login;
        if (rep.Error == 0)
        {
            this.Dispose();
            GameL.UI.Open<FUILoading>(0.9);
        }
    }
    [Event((int)EventIDM.NetError)]
    void connectRet(EventerContent e)
    {
        int error = e.Value;

    }

    void onUIModel()
    {
        if (_uiType.selectedIndex == 0)
            GameL.Setting.UIModel = UIModel.FGUI;
        else
        {
            GameL.Setting.UIModel = UIModel.UGUI;
            GameL.UI.Open<UUILogin>();
            this.Dispose();
        }
    }
    async void login()
    {
        await GameL.Scene.InScene(10001);
        if (_demo.selectedIndex == 0)
            await GameL.UI.OpenAsync<FUIFighting>();
        else if (_demo.selectedIndex == 1)
            await GameL.UI.OpenAsync<FUIFighting2>();
        else if (_demo.selectedIndex == 2)
            await GameL.UI.OpenAsync<FUIFighting3>();
        else if (_demo.selectedIndex == 3)
            await GameL.UI.OpenAsync<FUIFighting4>();
        return;

        //链接服务器演示
        if (_gameTypeCB.selectedIndex == 0)
        {
            GameM.Net.Connect(NetType.TCP, Util.ToIPEndPoint(ConstDefM.LoginAddressInner));
        }
        else if (_gameTypeCB.selectedIndex == 1)
        {
            GameM.Net.Connect(NetType.TCP, Util.ToIPEndPoint(ConstDefM.LoginAddressOuter));
        }

        var msg = new C2R_Login()
        {
            Account = _acc.text,
            Password = _pw.text
        };
        GameM.Net.Send(msg);
    }
}
