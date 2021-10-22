using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using FairyGUI;
using System;

partial class FUILoading
{
    float cur = 0;
    float max = 0.1f;
    protected override void OnEnter(params object[] data)
    {
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
                Timer.Add(2, 1, () =>
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