using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using FairyGUI;
using System;

[Main.UIConfig(50, UIType = Main.UIType.GlobalUI)]
partial class FUILoading
{
    float cur = 0;
    public float max = 0.7f;
    protected override void OnEnter(params object[] data)
    {
        if (data.Length >= 1)
            max = Convert.ToSingle(data[0]);
        refView();
        _loadingBar.value = 0;
        _loadingBar.max = 1;
        Timer.Add(0, -1, timer);
    }

    protected override void OnExit()
    {
        Timer.Remove(timer);
    }
    void timer()
    {
        if (cur < max)
        {
            cur += Time.deltaTime / 3;
            cur = Math.Min(cur, max);
            _loadingBar.value = cur;
            if (cur >= 1)
            {
                Timer.Add(2, 1, this.Dispose);
            }
        }
    }
    void refView()
    {

    }
}