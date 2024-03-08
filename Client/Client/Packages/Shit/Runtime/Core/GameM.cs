using Event;
using Game;
using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    public static async STask Init(params IEnumerable<Type>[] other)
    {
        var selfTypes = typeof(GameM).Assembly.GetTypes();
        int len = selfTypes.Length;
        for (int i = 0; i < other.Length; i++)
            len += other[i].Count();
        List<Type> types = new(len);
        types.AddRange(selfTypes);
        for (int i = 0; i < other.Length; i++)
            types.AddRange(other[i]);

        EventSystem.Check(types);
        STimer.Check(types);

        var methods = Types.Parse(types);
        Event = new EventSystem();
        Event.RigisteAllStaticEvent(methods);
        Net = new NetSystem();
        World = new SWorld();
        Data = new SData();
        SSystem.Init(methods, types);
        STimer.Init(methods);

#if UNITY_EDITOR
        if (Application.isPlaying)
            GameObject.DontDestroyOnLoad(new GameObject($"[{nameof(Engine)}]").AddComponent<Engine>());
        else
            UnityEditor.EditorApplication.update += Update;
#else

        GameObject.DontDestroyOnLoad(new GameObject($"[{nameof(Engine)}]").AddComponent<Engine>());
#endif

        List<(int, MethodInfo)> inits = new();
        for (int i = 0; i < methods.Count; i++)
        {
            if (methods[i].attribute is Init init && methods[i].method.IsStatic)
                inits.Add(new(init.SortOrder, methods[i].method));
        }
        inits.Sort((x, y) => x.Item1 - y.Item1);
        for (int i = 0; i < inits.Count; i++)
        {
            if (inits[i].Item2.Invoke(null, null) is STask task)
                await task;
        }
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

    class MethodSort
    {
        public MethodInfo mi;
        public int sort;
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
