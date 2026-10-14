using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

public sealed class World : STree
{
    public World() : base()
    {
        gameObject = new("World");
        if (Application.isPlaying)
        {
            GameObject.DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<OnBeforeUpdate>();
            gameObject.AddComponent<OnUpdate>();
        }
        else
            gameObject.hideFlags |= HideFlags.HideInHierarchy;
        transform = gameObject.transform;
    }
    public GameObject gameObject { get; private set; }
    public Transform transform { get; private set; }

    public override void Dispose()
    {
        base.Dispose();
        GameObject.DestroyImmediate(gameObject);
    }
}

public sealed class Game : SCore<Game>
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    static void Init()
    {
        UnityEditor.Compilation.CompilationPipeline.compilationFinished -= Reload;
        UnityEditor.Compilation.CompilationPipeline.compilationFinished += Reload;
    }
    static void Reload(object o)
    {
        if (World == null) return;
        Event.RunEvent(new EC_QuitGame());
    }
#endif
    public static World World { get; private set; }
    public static SocketManager Socket { get; private set; }
    public static DataManager Data { get; private set; }
    public static Scene Scene { get; private set; }
    public static UIManager UI { get; private set; }
    public static SLoader Loader { get; private set; }

    public static void Initialize(List<Type> types)
    {
        InitializeCore(types);
        World = new();
        World.Initialize();
        Socket = new();
        Data = new();
        World.AddChild(UI = new());
        Loader = new("Game", null);

        Event.RunEvent(new EC_ClientLanucher());
    }
    public static void Close()
    {
        if (World == null) return;
        UnInitialize();
        var w = World;
        World = null;
        w.Dispose();
    }

    public static async STask InScene<T>(params object[] os) where T : Scene, new()
    {
        var attr = typeof(T).GetCustomAttributes(typeof(SceneAttribute), true).FirstOrDefault() as SceneAttribute ?? throw new Exception($"Scene {typeof(T).FullName} need SceneAttribute");
        if (string.IsNullOrEmpty(attr.name))
            throw new Exception($"Scene {typeof(T).FullName} SceneAttribute name is null or empty");

        Scene?.Dispose();
        Scene = null;
        Game.UI.CloseUI();
        await Game.Event.RunEventAsync(new EC_OutScene { });

        await Game.Loader.LoadSceneAsync($"scene_{attr.name}");
        await Task.Delay(100);

        //不能提前初始化并注册Scene  否则 scene的timer之类的自动注册运行逻辑 会在没有执行onEnter的时候运行  会有问题
        Scene = new T() { _paramObjects = os };
        Game.World.AddChild(Scene);
        Scene.OnEnter();

        await Game.Event.RunEventAsync(new EC_InScene { });
    }
}
