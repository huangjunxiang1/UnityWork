using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using FairyGUI;
using System;
using Event;
using Core;

[Game.UIConfig(50, CloseIfChangeScene = false, HideIfOpenOtherUI = false)]
partial class FUILoading
{
    [Event(-100, Queue = true)]
    static async STask EC_OutScene(EC_OutScene e)
    {
        if (SettingL.UIModel == UIModel.FGUI)
            await UI.Inst.OpenAsync<FUILoading>();
    }
    [Event(100, Queue = true)]
    static async STask EC_InScene(EC_InScene e)
    {
        var ui = UI.Inst.GetChild<FUILoading>();
        if (ui != null)
        {
            ui.cur = ui.max;
            ui._loadingBar.value = 1;
            await STask.Delay(1000);
            ui.Dispose();
        }
    }

    float cur = 0;
    public float max = 0.7f;
    protected override void OnEnter(params object[] data)
    {
        if (data.Length >= 1)
            max = Convert.ToSingle(data[0]);
        refView();
        _loadingBar.value = 0;
        _loadingBar.max = 1;
    }

    [STimer(0, -1)]
    void timer()
    {
        if (cur < max)
        {
            cur += Time.deltaTime / 2;
            cur = Math.Min(cur, max);
            _loadingBar.value = cur;
            if (cur >= 1)
            {
                World.Timer.Add(1, 1, this.Dispose);
            }
        }
    }
    void refView()
    {

    }
}