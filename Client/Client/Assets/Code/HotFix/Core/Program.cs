using Core;
using Event;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class Program
{
    public static async void Main()
    {
        long tick = DateTime.Now.Ticks;

        await GameWorld.Init();

        long tick2 = DateTime.Now.Ticks;
        UnityEngine.Debug.Log("��ܳ�ʼ���ɹ�");
        UnityEngine.Debug.Log($"��ʱ:{(tick2 - tick) / 10000}ms");

        await UI.Inst.Load();
        await GameWorld.World.Event.RunEventAsync(new EC_GameStart());

        long tick3 = DateTime.Now.Ticks;
        UnityEngine.Debug.Log("��Ϸ��ʼ���ɹ�");
        UnityEngine.Debug.Log($"��ʱ:{(tick3 - tick2) / 10000}ms");

        await GameWorld.World.Scene.InLoginScene();
    }
}
