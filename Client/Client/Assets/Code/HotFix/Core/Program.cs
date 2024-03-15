using Event;
using System;

public static class Program
{
    public static async void Main()
    {
        long tick = DateTime.Now.Ticks;

        await GameM.Init();
        GameL.Init();

        long tick2 = DateTime.Now.Ticks;
        await GameL.UI.Init();

        long tick3 = DateTime.Now.Ticks;
        UnityEngine.Debug.Log("��Ϸ��ʼ���ɹ�");
        UnityEngine.Debug.Log($"��ܳ�ʼ����ʱ:{(tick2 - tick) / 10000}ms");
        UnityEngine.Debug.Log($"��Ϸ��ʼ����ʱ:{(tick3 - tick2) / 10000}ms");

        GameM.Event.RunEvent(new EC_GameStart());
    }
}
