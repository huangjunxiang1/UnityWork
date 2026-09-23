using Event;
using System;
using System.Collections.Generic;

public static class Program
{
    public async static void Main()
    {
        long tick = DateTime.Now.Ticks;
        List<Type> types = Types.ReflectionAllTypes();
        MessageParser.Parse(types);
        Game.Initialize(types);

        long tick2 = DateTime.Now.Ticks;
        UnityEngine.Debug.Log("框架初始化成功");
        UnityEngine.Debug.Log($"耗时:{(tick2 - tick) / 10000}ms");

        await Game.Event.RunEventAsync(new EC_GameStart());

        long tick3 = DateTime.Now.Ticks;
        UnityEngine.Debug.Log("游戏初始化成功");
        UnityEngine.Debug.Log($"耗时:{(tick3 - tick2) / 10000}ms");

        //先运行单元测试
        if (GameStart.Inst.Debug)
        {
            long tick4 = DateTime.Now.Ticks;
            Game.Event.RunEvent(new EC_ModuleTest());
            UnityEngine.Debug.Log("单元测试完成");
            UnityEngine.Debug.Log($"耗时:{(tick4 - tick3) / 10000}ms");
        }

        await Game.Scene.InScene<LoginScene>();
    }
}
