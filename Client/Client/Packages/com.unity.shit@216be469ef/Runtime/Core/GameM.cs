using Game;
using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

public static class GameM
{
    public static EventSystem Event { get; private set; }
    public static NetSystem Net { get; private set; }
    public static SWorld World { get; private set; }
    public static SData Data { get; private set; }

    public static void Init()
    {
        var methods = Types.Parse();
        Event = new EventSystem();
        Event.RigisteAllStaticEvent(methods);
        Net = new NetSystem();
        World = new SWorld();
        Data = new SData();
        SSystem.Init(methods); 
        STimer.Init(methods);
        GameObject.DontDestroyOnLoad(new GameObject($"[{nameof(Engine)}]").AddComponent<Engine>());
    }
    public static void Close()
    {
        Net.Dispose();
    }
    static void Update()
    {
        Profiler.BeginSample($"{nameof(GameM)}.{nameof(Update)}");
        long tick = DateTime.Now.Ticks;
        ThreadSynchronizationContext.Instance.Update();
        Net.Update(tick);
        SObject.Update();
        SSystem.Update();
        STimer.Update();
        SSystem.AfterUpdate();
        STimer.AfterUpdate();
        Profiler.EndSample();
    }

    class Engine : MonoBehaviour
    {
        void Update()
        {
            GameM.Update();
        }
        void OnApplicationQuit()
        {
            GameM.Event.RunEvent(new EC_QuitGame());
        }
    }
}
