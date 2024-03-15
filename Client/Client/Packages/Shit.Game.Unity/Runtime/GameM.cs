using Event;
using Game;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

public static partial class GameM
{
    public static SWorld World { get; private set; }
    public static EventSystem Event => World.Event;
    public static NetSystem Net { get; private set; }
    public static SData Data { get; private set; }
    public static STimer Timer => World.Timer;

    public static STask Init()
    {
        List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(t => t.IsDefined(typeof(AssemblyIncludedToRuntimeShit))).ToList();
        assemblies.Sort((x, y) => x.GetCustomAttribute<AssemblyIncludedToRuntimeShit>().SortOrder - y.GetCustomAttribute<AssemblyIncludedToRuntimeShit>().SortOrder);
        return Init(assemblies);
    }
    public static async STask Init(List<Assembly> assemblies)
    {
        List<Type> types = new(assemblies.SelectMany(t => t.GetTypes()));

        var methods = CoreTypes.Parse(types);
        World = new SWorld();
        Event.RigisteAllStaticEvent(methods);
        Net = new NetSystem();
        Data = new SData();
        Timer.Init(methods);

        if (Application.isPlaying)
            GameObject.DontDestroyOnLoad(new GameObject($"[{nameof(Engine)}]").AddComponent<Engine>());

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
    }
    internal static void Update()
    {
        long tick = DateTime.Now.Ticks;

        Profiler.BeginSample($"{nameof(ThreadSynchronizationContext)}.{nameof(ThreadSynchronizationContext.Update)}");
        ThreadSynchronizationContext.Instance.Update();
        Profiler.EndSample();

        Profiler.BeginSample($"{nameof(NetSystem)}.{nameof(NetSystem.Update)}");
        Net.Update(tick);
        Profiler.EndSample();

        Profiler.BeginSample($"{nameof(World)}.{nameof(World.Update)}");
        World.Update(Time.deltaTime);
        Profiler.EndSample();

        Profiler.BeginSample($"{nameof(World)}.{nameof(World.AfterUpdate)}");
        World.AfterUpdate();
        Profiler.EndSample();
    }

    class Engine : MonoBehaviour
    {
        void Update() => GameM.Update();
        void OnApplicationQuit() => GameM.Event.RunEvent(new EC_QuitGame());
    }
}
