using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using FairyGUI;
using System.Threading.Tasks;
using System;
using Event;

partial class FUILogin
{
    [Event]
    static async void EC_InScene(EC_InScene e)
    {
        if (e.sceneId == 1)
            await UI.Inst.OpenAsync<FUILogin>();
    }
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
            SettingL.UIModel = UIModel.FGUI;
        else
        {
            SettingL.UIModel = UIModel.UGUI;
            await UI.Inst.OpenAsync<UUILogin>();
            this.Dispose();
        }
    }
   
    async void login()
    {
        if (_demo.selectedIndex == 0)
        {
            int id = 10001;
            await Client.Scene.InScene(id, TabL.GetScene(id).type, TabL.GetScene(id).name);
            await UI.Inst.OpenAsync<FUIFighting>();
        }
        else if (_demo.selectedIndex == 1)
        {
            int id = 10001;
            await Client.Scene.InScene(id, TabL.GetScene(id).type, TabL.GetScene(id).name);
            await UI.Inst.OpenAsync<FUIFighting2>();
        }
        else if (_demo.selectedIndex == 2)
        {
            int id = 10001;
            await Client.Scene.InScene(id, TabL.GetScene(id).type, TabL.GetScene(id).name);
            await UI.Inst.OpenAsync<FUIFighting3>();
        }
        else if (_demo.selectedIndex == 3)
        {
            int id = 10001;
            await Client.Scene.InScene(id, TabL.GetScene(id).type, TabL.GetScene(id).name);
            await UI.Inst.OpenAsync<FUIFighting4>();
        }
        else if (_demo.selectedIndex == 4)
        {
            await UI.Inst.OpenAsync<FUIFighting5>();
        }
    }
}
