using System;

public static class Init
{
    public static async void Main()
    {
        long tick = DateTime.Now.Ticks;
        Types.RigisterTypes(typeof(GameM).Assembly.GetTypes());
        Types.RigisterTypes(typeof(TabM).Assembly.GetTypes());
        Types.RigisterTypes(typeof(GameL).Assembly.GetTypes());

        GameM.Init();
        GameL.Init();

        long tick2 = DateTime.Now.Ticks;
        await GameM.Event.RunEventAsync(new EC_GameInit());
        await GameL.UI.Init();

        long tick3 = DateTime.Now.Ticks;
        UnityEngine.Debug.Log("游戏初始化成功");
        UnityEngine.Debug.Log($"框架初始化耗时:{(tick2 - tick) / 10000}ms");
        UnityEngine.Debug.Log($"游戏初始化耗时:{(tick3 - tick2) / 10000}ms");

        GameM.Event.RunEvent(new EC_GameStart());
    }
}
