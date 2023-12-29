using UnityEngine;
using Main;
using System.IO;
using Game;
using System.Collections.Generic;

public class Init
{
    public static void Main()
    {
        if (SConstDefM.isILRuntime)
        {
            //主工程不是debug模式 loger会被剪裁 导致热更debug模式访问loger报错
            if (!SConstDefM.Debug && SConstDefL.Debug)
                Loger.Error("主工程不是debug模式 热更是debug模式");
        }

        DG.Tweening.DOTween.Init();
        SGameM.Init();
        SGameL.Init();
        SGameM.Event.RunEvent(new EC_GameInit());
        Types.ClearStaticMethodsCache();
        SGameM.Event.RunEvent(new EC_GameStart());
    }
}
