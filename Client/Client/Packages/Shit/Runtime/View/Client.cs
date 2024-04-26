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
        static Client()
        {
#if UNITY_EDITOR
            UnityEditor.Compilation.CompilationPipeline.compilationFinished += o =>
            {
                if (World == null) return;
                World.Event.RunEvent(new EC_QuitGame());
            };
#endif
        }
        public static World World { get; private set; }
        public static GameObject gameObject { get; private set; }
        public static Transform transform { get; private set; }
        public static Data Data { get; private set; }
        public static Scene Scene { get; private set; }

        public static void Load(List<Type> types)
        {
            World = new(types);

            Data = new(World);
            World.Root.AddChild(Scene = new());

            if (Application.isPlaying)
            {
                gameObject = new(nameof(Client));
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
            w.Thread.Post(w.Dispose);
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
            void OnApplicationQuit() => World.Event.RunEvent(new EC_QuitGame());
        }
    }
}
