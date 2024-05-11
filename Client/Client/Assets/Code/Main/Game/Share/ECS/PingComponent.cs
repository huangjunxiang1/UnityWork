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

    static async void Awake(In<NetComponent> t)
    {
        if (t.t.isClient) return;
        var ping = t.t.Entity.AddComponent<PingComponent>();
        while (true)
        {
            await Task.Delay(3000);
            if (!ping.Disposed)
            {
                if (ping.counter > 5)
                {
                    ping.Dispose();
                    t.t.Session.DisConnect();
                    break;
                }
                t.t.Send(s_p);
                ping.counter++;
            }
            else break;
        }
    }

    [Event]
    static void watcher(EventWatcher<C2S_Ping, PingComponent, NetComponent> t)
    {
        if (t.t3.isClient) return;
        t.t2.counter = 0;
    }
    [Event]
    static void ping(EventWatcher<S2C_Ping, NetComponent> t)
    {
        if (!t.t2.isClient) return;
        t.t2.Send(c_p);
    }
}