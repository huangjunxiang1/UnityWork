using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
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
    public static SceneManager Scene { get; private set; }
    public static UIManager UI { get; private set; }
    public static SLoader Loader { get; private set; }

    public static void Initialize(List<Type> types)
    {
        Initializes(types);
        World = new();
        World.Initialize();
        Socket = new();
        Data = new();
        Scene = new SceneManager();
        if (Application.isPlaying)
            World.AddChild(UI = new());
        Loader = new("Game", null);

        Event.RunEvent(new EC_ClientLanucher());
    }
    public static void Close()
    {
        if (World == null) return;
        UnInitializes();
        var w = World;
        World = null;
        w.Dispose();
    }
}
