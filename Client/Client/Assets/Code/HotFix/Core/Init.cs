using UnityEngine;
using Main;
using Game;

public class Init
{
    public static async void Main()
    {
        GameM.Init();
        GameL.Init();
        await GameM.Event.RunEventAsync(new EC_GameInit());
        GameM.Event.RunEvent(new EC_GameStart());
    }
}
