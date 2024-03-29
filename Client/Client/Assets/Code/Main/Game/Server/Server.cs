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

namespace Game
{
    public static class Server
    {
        public static World World { get; private set; }
        public static void Load()
        {
            Load(Types.ReflectionAllTypes());
        }
        public static void Load(List<Type> types)
        {
#if Server
            Run(types);
#else
            new Thread(() => Run(types)) { IsBackground = true }.Start();
#endif
        }
        static void Run(List<Type> types)
        {
            World = new(types);

            World.Event.RunEvent(new EC_ServerLanucher());

            long tick, tick2;
            tick2 = DateTime.Now.Ticks;
            Loger.Log("服务器启动成功");
            while (!World.Root.Disposed)
            {
                tick = tick2;
                tick2 = DateTime.Now.Ticks;
                try
                {
                    World.Update((tick2 - tick) / 10000000f);
                }
                catch (Exception ex)
                {
                    Loger.Error($"update error " + ex);
                }
                Thread.Sleep(10);
            }
        }
        public static void Close()
        {
            if (World == null) return;
            World.Thread.Post(World.Dispose);
            World = null;
        }
    }
}
