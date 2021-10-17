using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

partial class UILoding
{
    float cur = 0;
    float max = 0.1f;
    protected override void OnEnter(params object[] data)
    {
        max = Convert.ToSingle(data[0]);
        refView();
        _fillImage.fillAmount = 0;
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
            _fillImage.fillAmount = cur;
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
