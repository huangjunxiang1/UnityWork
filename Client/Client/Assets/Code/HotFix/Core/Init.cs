using UnityEngine;
using Main;
using System.IO;
using Game;

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
        STimer.RigisterStaticTimer();
        SGameM.Init();
        SGameL.Init();
        SGameM.Event.RunEvent(new EC_HotFixInit());
    }
}
