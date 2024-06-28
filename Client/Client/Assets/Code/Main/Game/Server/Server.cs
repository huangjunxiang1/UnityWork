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
        public static STask Load()
        {
            return Load(Types.ReflectionAllTypes());
        }
        public static STask Load(List<Type> types)
        {
#if Server
            Run(types);
            return STask.Completed;
#else
            int id = System.Environment.CurrentManagedThreadId;
            STask task = new();
            new Thread(() => Run(types, () =>
            {
                ThreadSynchronizationContext.GetOrCreate(id).Post(() => task.TrySetResult());
            }))
            { IsBackground = true }.Start();
            return task;
#endif
        }
        static void Run(List<Type> types, Action callBack = null)
        {
            World = new(types, "Server");
#if Server
            STask.DelayHandle -= delayHandle;
            STask.DelayHandle += delayHandle;
#endif

            var w = World;
            w.Timer.utc = w.Timer.utc;
            w.Event.RunEvent(new EC_ServerLanucher());

            long tick, tick2;
            tick2 = DateTime.Now.Ticks;
            Loger.Log("服务器启动成功");
            callBack?.Invoke();
            while (!w.Root.Disposed)
            {
                tick = tick2;
                tick2 = DateTime.Now.Ticks;
                try
                {
                    float time = (tick2 - tick) / 10000000f;
                    time = Math.Min(time, 0.3f);
                    w.Update(time);
                }
                catch (Exception ex)
                {
                    Loger.Error($"update error " + ex);
                }
                Thread.Sleep(20);
            }
        }
        public static void Close()
        {
            if (World == null) return;
            var w = World;
            World = null;
            w.Dispose();
        }

#if Server
        static void delayHandle(int ms, STask task)
        {
            if (World == null || World.Root.Disposed)
                throw new Exception("World is Close");
            World.Timer.Add(ms / 1000f, 1, task.TrySetResult);
        }
#endif
    }
}
