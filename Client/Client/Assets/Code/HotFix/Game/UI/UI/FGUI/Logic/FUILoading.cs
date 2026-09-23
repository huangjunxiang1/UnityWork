using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FairyGUI;
using System;
using Event;
using Core;

[UIConfig(50,HideIfOpenOtherUI = false)]
partial class FUILoading
{
    [Event(-100, Queue = true)]
    static void EC_OutScene(EC_OutScene e)
    {
        Game.UI.Open<FUILoading>();
    }
    [Event(100, Queue = true)]
    static async STask EC_InScene(EC_InScene e)
    {
        var ui = Game.UI.GetChild<FUILoading>();
        if (ui != null)
        {
            ui.cur = ui.max;
            ui._loadingBar.value = 1;
            await SValueTask.Delay(1000);
            ui.Dispose();
        }
    }

    float cur = 0;
    public float max = 0.7f;
    protected override void OnEnter()
    {
        max = this.GetParam<float>(0);
        refView();
        _loadingBar.value = 0;
        _loadingBar.max = 1;
    }

    [Timer(0, -1)]
    void timer()
    {
        if (cur < max)
        {
            cur += Time.deltaTime / 2;
            cur = Math.Min(cur, max);
            _loadingBar.value = cur;
            if (cur >= 1)
            {
                Game.Timer.Add(1, 1, this.Dispose);
            }
        }
    }
    void refView()
    {

    }
}