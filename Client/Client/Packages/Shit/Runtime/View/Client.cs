using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Profiling;

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
        public static Scene Scene { get; private set; }
        public static UIManager UI { get; private set; }

        public static void Load(List<Type> types)
        {
            World = new(types, "Client");
            STask.DelayHandle -= delayHandle;
            STask.DelayHandle += delayHandle;

            Data = new(World);
            World.Root.AddChild(Scene = new() { isCrucialRoot = true });

            if (Application.isPlaying)
            {
                World.Root.AddChild(UI = new());
                gameObject = new(nameof(World));
                gameObject.AddComponent<Engine>();
                transform = gameObject.transform;
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
            }
            World.Event.RunEvent(new EC_ClientLanucher());
        }
        public static void Close()
        {
            if (World == null) return;
            var w = World;
            World = null;
            w.Dispose();
        }

        static void delayHandle(int ms, STask task)
        {
            if (World == null || World.Root.Disposed)
                throw new Exception("World is Close");
            World.Timer.Add(ms / 1000f, 1, task.TrySetResult);
        }

        class Engine : MonoBehaviour
        {
            void Update()
            {
                Profiler.BeginSample($"{nameof(World)}.{nameof(World.Update)}");
                try
                {
                    World.Update(Time.deltaTime);
                }
                catch (Exception ex)
                {
                    Loger.Error($"update error " + ex);
                }
                Profiler.EndSample();
            }
            void OnApplicationQuit() => World?.Event?.RunEvent(new EC_QuitGame());
        }
    }
}
