using Core;
using Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class Client
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
            World.Event.RunEvent(new EC_QuitGame());
        }
#endif
        public static World World { get; private set; }
        public static GameObject gameObject { get; private set; }
        public static Transform transform { get; private set; }
        public static Data Data { get; private set; }
        public static SceneManager Scene { get; private set; }
        public static UIManager UI { get; private set; }

        public static void Load(List<Type> types)
        {
            World = new(types, "Client");
            SValueTask.DelayHandle -= delayHandle;
            SValueTask.DelayHandle += delayHandle;

            Data = new(World);
            World.Root.AddChild(Scene = new() { isCrucialRoot = true });

            if (Application.isPlaying)
            {
                World.Root.AddChild(UI = new());
                gameObject = new(nameof(World));
                gameObject.AddComponent<OnBeforeUpdate>().world = World;
                gameObject.AddComponent<OnUpdate>().world = World;
                transform = gameObject.transform;
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }
            World.Event.RunEvent(new EC_ClientLanucher());
        }
        public static void Close()
        {
            if (World == null) return;
            SValueTask.DelayHandle -= delayHandle;
            var w = World;
            World = null;
            w.Dispose();
            GameObject.Destroy(gameObject);
        }

        static void delayHandle(int ms, SValueTask task)
        {
            if (World == null || World.Root.Disposed)
                throw new Exception("World is Close");
            World.Timer.Add(ms / 1000f, 1, task.TrySetResult);
        }
    }
}
