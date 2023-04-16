using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

[Main.UIConfig(50, UIType = Main.UIType.GlobalUI)]
partial class UUILoading
{
    float cur = 0;
    float max = 0.1f;

    protected override void VMBinding()
    {
        base.VMBinding();
        _fillImageBinding.Binding(() => _fillImageBinding.ui.fillAmount = _fillImageBinding.value);
    }
    protected override void OnEnter(params object[] data)
    {
        max = Convert.ToSingle(data[0]);
        refView();
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
            _fillImageBinding.value = cur;
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
