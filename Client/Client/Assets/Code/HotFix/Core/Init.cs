using UnityEngine;
using Main;
using Game;

public class Init
{
    public static async void Main()
    {
        if (ConstDefM.isILRuntime)
        {
            //主工程不是debug模式 loger会被剪裁 导致热更debug模式访问loger报错
            if (!ConstDefM.Debug && ConstDefL.Debug)
                Loger.Error("主工程不是debug模式 热更是debug模式");
        }

        GameM.Init();
        GameL.Init();
        await GameM.Event.RunEventAsync(new EC_GameInit());
        GameM.Event.RunEvent(new EC_GameStart());
    }
}
