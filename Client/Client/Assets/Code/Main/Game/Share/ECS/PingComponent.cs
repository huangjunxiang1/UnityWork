using Core;
using game;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class PingComponent : SComponent
{
    static C2S_Ping c_p = new();
    static S2C_Ping s_p = new();
    int counter;

    [AwakeSystem]
    static async void Awake(NetComponent t)
    {
        if (t.isClient) return;
        var ping = t.Entity.AddComponent<PingComponent>();
        while (true)
        {
            await STask.Delay(3000);
            if (!ping.Disposed)
            {
                if (ping.counter > 5)
                {
                    ping.Dispose();
                    t.Session.DisConnect();
                    break;
                }
                t.Send(s_p);
                ping.counter++;
            }
            else break;
        }
    }

    [EventWatcherSystem]
    static void watcher(C2S_Ping a, PingComponent b, NetComponent c)
    {
        if (c.isClient) return;
        b.counter = 0;
    }
    [EventWatcherSystem]
    static void ping(S2C_Ping a, NetComponent b)
    {
        if (!b.isClient) return;
        b.Send(c_p);
    }
}