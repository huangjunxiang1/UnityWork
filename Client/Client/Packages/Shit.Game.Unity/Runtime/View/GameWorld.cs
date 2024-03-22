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
    public partial class GameWorld : CoreWorld<GameWorld>
    {
        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }
        public NetSystem Net { get; private set; }
        public Data Data { get; private set; }
        public Scene Scene { get; private set; }

        public override void Dispose()
        {
            base.Dispose();
            World.Net.Dispose();
        }

        public static STask Init()
        {
            List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(t => t.IsDefined(typeof(AssemblyIncludedToShitRuntime))).ToList();
            assemblies.Sort((x, y) => x.GetCustomAttribute<AssemblyIncludedToShitRuntime>().SortOrder - y.GetCustomAttribute<AssemblyIncludedToShitRuntime>().SortOrder);
            return Init(assemblies);
        }
        public static async STask Init(List<Assembly> assemblies)
        {
            World = new();
            var methods = World.Load(assemblies);

            Root = new();
            Root.World = World;

            World.Net = new(World);
            World.Data = new(World);

            Root.AddChild(World.Scene = new());

            MessageParser.Parse(World.Types.types);

            if (Application.isPlaying)
            {
                World.gameObject = new(nameof(GameWorld));
                World.gameObject.AddComponent<Engine>();
                World.transform = World.gameObject.transform;
                UnityEngine.Object.DontDestroyOnLoad(World.gameObject);

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
        }

        class Engine : MonoBehaviour
        {
            void Update()
            {
                Profiler.BeginSample($"{nameof(GameWorld)}.{nameof(GameWorld.Update)}");
                World.Update(Time.deltaTime);
                Profiler.EndSample();
            }
            void OnApplicationQuit() => World.Event.RunEvent(new EC_QuitGame());
        }
    }
}
