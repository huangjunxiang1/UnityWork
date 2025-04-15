using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class EventWatcher
{
    public static void test()
    {
        SObject o = new() { ActorId = 5 };
        SObject o2 = new() { ActorId = 6 };
        Client.World.Root.AddChild(o);
        Client.World.Root.AddChild(o2);
        Evt e = new();

        o.AddComponent<c1>();
        o2.AddComponent<c1>();

        Client.World.Event.RunEvent(e, actorId: 5);
        if (e.v != 2) throw new Exception();

        Client.World.Event.RunEvent(e, actorId: 4);
        if (e.v != 2) throw new Exception();

        o.AddComponent<c2>();
        o2.AddComponent<c1>();

        Client.World.Event.RunEvent(e, actorId: 5);
        if (e.v != 6) throw new Exception();
        Client.World.Event.RunEvent(e, actorId: 4);
        if (e.v != 6) throw new Exception();

        Client.World.Event.RunEvent(e);
        if (e.v != 12) throw new Exception();

        Client.World.Event.RunEvent(e, gid: o.gid);
        if (e.v != 16) throw new Exception();

        Client.World.Event.RunEvent(e, gid: o2.gid);
        if (e.v != 18) throw new Exception();

        Client.World.Event.RunEvent(e, gid: 4);
        if (e.v != 18) throw new Exception();
        Client.World.Event.RunEvent(e, gid: 5);
        if (e.v != 18) throw new Exception();

        Client.World.Event.RunEvent(e, gid: o.gid, type: 1);
        Client.World.Event.RunEvent(e, gid: o.gid, type: 2);
        if (e.v != 26) throw new Exception();
        Client.World.Event.RunEvent(e, actorId: 5, type: 1);
        Client.World.Event.RunEvent(e, actorId: 5, type: 2);
        if (e.v != 34) throw new Exception();

        o.GetComponent<c2>().Enable = false;
        Client.World.Event.RunEvent(e, gid: o.gid);
        if (e.v != 36) throw new Exception();

        o.GetComponent<c2>().Enable = true;
        Client.World.Event.RunEvent(e);
        if (e.v != 42) throw new Exception();

        o.GetComponent<c2>().Dispose();
        Client.World.Event.RunEvent(e);
        if (e.v != 46) throw new Exception();
    }

    class Evt
    {
        public int v;
    }

    class c1 : SComponent { }
    class c2 : SComponent { }

    [EventWatcherSystem]
    static void t1(Evt a, c1 t) => a.v++;
    [EventWatcherSystem]
    static void t11(Evt a, c1 t) => a.v++;
    [EventWatcherSystem]
    static void t1(Evt a, c2 t) => a.v++;
    [EventWatcherSystem]
    static void t1(Evt a, c1 b, c2 t) => a.v++;
}
