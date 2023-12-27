using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

[Main.SUIConfig(50, UIType = Main.UIType.GlobalUI)]
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
    }

    [STimer(0, -1)]
    void timer()
    {
        if (cur < max)
        {
            cur += Time.deltaTime / 3;
            cur = Math.Min(cur, max);
            _fillImageBinding.value = cur;
            if (cur >= 1)
            {
                STimer.Add(2, 1, () =>
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
