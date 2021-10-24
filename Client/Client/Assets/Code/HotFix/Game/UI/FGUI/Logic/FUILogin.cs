using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using FairyGUI;
using Main;

partial class FUILogin
{
    protected override void OnEnter(params object[] data)
    {
        _btnLogin.onClick.Add(login);
        _gameTypeCB.values = new string[]
        {
            "单机模式",
            "联网模式",
        };
        _uiType.onChanged.Add(onUIModel);
        _uiType.selectedIndex = 0;
    }

    protected override void OnExit()
    {

    }

    [Msg(CMDL.R2C_Login)]
    void R2C_Login(IMessage message)
    {
        R2C_Login rep = message as R2C_Login;
        if (rep.Error == 0)
        {
            this.Dispose();
            UIS.Open<UUILoading>(1);
        }
    }
    [Event((int)EIDM.NetError)]
    void connectRet(EventerContent e)
    {
        int error = e.ValueInt;

    }

    void onUIModel()
    {
        if (_uiType.selectedIndex == 0)
            GameSetting.UIModel = UIModel.FGUI;
        else
        {
            GameSetting.UIModel = UIModel.UGUI;
            UIS.Open<UUILogin>();
            this.Dispose();
        }
    }
    void login()
    {
        if (_gameTypeCB.selectedIndex == 0)
        {
            this.Dispose();
            if (GameSetting.UIModel == UIModel.UGUI)
            {
                 //ugui 只做个展示  实际使用fgui
            }
            else
            {
                _ = SceneMgr.Inst.InScene(10001);
            }
        }
        else if (_gameTypeCB.selectedIndex == 1)
        {
            SysNet.Connect(NetType.KCP, Util.ToIPEndPoint(AppSetting.LoginAddress));
            var msg = new C2R_Login()
            {
                Account = _acc.text,
                Password = _pw.text
            };
            SysNet.Send(msg);
        }
    }
}
