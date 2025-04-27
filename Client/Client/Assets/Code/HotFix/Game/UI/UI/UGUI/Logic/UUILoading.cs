using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Event;
using Game;
using Core;

[UIConfig(50, HideIfOpenOtherUI = false)]
partial class UUILoading
{
    //[Event(-100, Queue = true)]
    static async STask EC_OutScene(EC_OutScene e)
    {
        await Client.UI.OpenAsync<UUILoading>();
    }
    [Event(100, Queue = true)]
    static async STask EC_InScene(EC_InScene e)
    {
        var ui = Client.UI.GetChild<UUILoading>();
        if (ui != null)
        {
            ui.cur = ui.max;
            //ui._fillImage.fi = 1;
            await STask.Delay(1000);
            ui.Dispose();
        }
    }

    float cur = 0;
    float max = 0.7f;

    protected override void OnEnter(params object[] data)
    {
        refView();
    }

    [Timer(0, -1)]
    void timer()
    {
        if (cur < max)
        {
            cur += Time.deltaTime / 3;
            cur = Math.Min(cur, max);
            //_fillImage.fillAmount = cur;
            if (cur >= 1)
            {
                World.Timer.Add(2, 1, () =>
                {
                    this.Dispose();
                });
            }
        }
    }

    void refView()
    {

    }
}
