using UnityEngine;
using Main;
using Game;

public class Init
{
    public static async void Main()
    {
        if (ConstDefM.isILRuntime)
        {
            //�����̲���debugģʽ loger�ᱻ���� �����ȸ�debugģʽ����loger����
            if (!ConstDefM.Debug && ConstDefL.Debug)
                Loger.Error("�����̲���debugģʽ �ȸ���debugģʽ");
        }

        GameM.Init();
        GameL.Init();
        await GameM.Event.RunEventAsync(new EC_GameInit());
        GameM.Event.RunEvent(new EC_GameStart());
    }
}
