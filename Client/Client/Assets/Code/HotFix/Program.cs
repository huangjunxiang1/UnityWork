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
        UnityEngine.Debug.Log("框架初始化成功");
        UnityEngine.Debug.Log($"耗时:{(tick2 - tick) / 10000}ms");

        await Client.World.Event.RunEventAsync(new EC_GameStart());

        long tick3 = DateTime.Now.Ticks;
        UnityEngine.Debug.Log("游戏初始化成功");
        UnityEngine.Debug.Log($"耗时:{(tick3 - tick2) / 10000}ms");

        await Client.Scene.InLoginScene();
    }
}
