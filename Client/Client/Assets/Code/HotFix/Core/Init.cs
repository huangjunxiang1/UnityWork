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
            //�����̲���debugģʽ loger�ᱻ���� �����ȸ�debugģʽ����loger����
            if (!SConstDefM.Debug && SConstDefL.Debug)
                Loger.Error("�����̲���debugģʽ �ȸ���debugģʽ");
        }

        DG.Tweening.DOTween.Init();
        SGameM.Init();
        SGameL.Init();
        SGameM.Event.RunEvent(new EC_GameInit());
        Types.ClearStaticMethodsCache();
        SGameM.Event.RunEvent(new EC_GameStart());
    }
}
