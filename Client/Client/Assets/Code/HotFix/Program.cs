using Event;
using Game;
using System;
using System.Collections.Generic;

public static class Program
{
    public static async void Main()
    {
        long tick = DateTime.Now.Ticks;
        List<Type> types = Types.ReflectionAllTypes();
        MessageParser.Parse(types);
        Client.Load(types);

        long tick2 = DateTime.Now.Ticks;
        UnityEngine.Debug.Log("��ܳ�ʼ���ɹ�");
        UnityEngine.Debug.Log($"��ʱ:{(tick2 - tick) / 10000}ms");

        await Client.World.Event.RunEventAsync(new EC_GameStart());

        long tick3 = DateTime.Now.Ticks;
        UnityEngine.Debug.Log("��Ϸ��ʼ���ɹ�");
        UnityEngine.Debug.Log($"��ʱ:{(tick3 - tick2) / 10000}ms");

        await Client.Scene.InLoginScene();
    }
}
