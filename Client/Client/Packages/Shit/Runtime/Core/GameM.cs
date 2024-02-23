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
#if UNITY_EDITOR
        //Types.EditorClear();
        SSystem.EditorClear();
        STimer.EditorClear();
#endif
        var methods = Types.Parse();
        Event = new EventSystem();
        Event.RigisteAllStaticEvent(methods);
        Net = new NetSystem();
        World = new SWorld();
        Data = new SData();
        SSystem.Init(methods); 
        STimer.Init(methods);

#if UNITY_EDITOR
        if (Application.isPlaying)
            GameObject.DontDestroyOnLoad(new GameObject($"[{nameof(Engine)}]").AddComponent<Engine>());
        else
            UnityEditor.EditorApplication.update += Update;
#else

        GameObject.DontDestroyOnLoad(new GameObject($"[{nameof(Engine)}]").AddComponent<Engine>());
#endif
    }
    public static void Close()
    {
        Net.Dispose();

#if UNITY_EDITOR
        if (!Application.isPlaying)
            UnityEditor.EditorApplication.update -= Update;
#endif
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
        Event.AfterUpdate();
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
