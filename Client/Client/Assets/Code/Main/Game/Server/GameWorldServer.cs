using Core;
using Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unity.Entities;
using UnityEngine;

namespace Game
{
    public class GameWorldServer : CoreWorld<GameWorldServer>
    {
        public NetSystem Net { get; private set; }

        public override void Dispose()
        {
            base.Dispose();
            Net.Dispose();
        }

        public static STask Init()
        {
            List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(t => t.IsDefined(typeof(AssemblyIncludedToShitRuntime))).ToList();
            assemblies.Sort((x, y) => x.GetCustomAttribute<AssemblyIncludedToShitRuntime>().SortOrder - y.GetCustomAttribute<AssemblyIncludedToShitRuntime>().SortOrder);
            return Init(assemblies);
        }
        static async STask Init(List<Assembly> assemblies)
        {
            new Thread(() => Run(assemblies)) { IsBackground = true }.Start();
        }
        static void Run(List<Assembly> assemblies)
        {
            World = new();
            World.Load(assemblies);

            Root = new();
            Root.World = World;

            World.Net = new(World);

            var players = new Players();
            var rooms = new Room();

            Root.AddChild(players);
            Root.AddChild(rooms);

            long tick = DateTime.Now.Ticks;
            long tick2 = DateTime.Now.Ticks;
            while (!Root.Disposed)
            {
                tick2 = DateTime.Now.Ticks;
                World.Update((tick2 - tick) / 10000000f);
                System.Threading.Thread.Sleep(10);
                tick = tick2;
            }
        }
        [Event]
        static void Quit(EC_QuitGame e)
        {
            GameWorldServer.Close();
        }
    }
}
