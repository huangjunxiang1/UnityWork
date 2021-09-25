using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class LInit
{
    public static void Init()
    {
        var ui = UIManager.Inst.Open<UILoding>(1);
        ui.OnDispose.Add(()=>
        {
            UIManager.Inst.Open<UILogin>();
        });
    }
}
