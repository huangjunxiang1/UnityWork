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
            World.Thread.Post(World.Dispose);
            World = null;
        }

        class Engine : MonoBehaviour
        {
            void Update()
            {
                Profiler.BeginSample($"{nameof(World)}.{nameof(World.Update)}");
                World.Update(Time.deltaTime);
                Profiler.EndSample();
            }
            void OnApplicationQuit() => World.Event.RunEvent(new EC_QuitGame());
        }
    }
}
