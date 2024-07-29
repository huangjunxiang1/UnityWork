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
        SObject o = new() { rpc = 5 };
        SObject o2 = new() { rpc = 6 };
        Client.World.Root.AddChild(o);
        Client.World.Root.AddChild(o2);
        Evt e = new();

        o.AddComponent<c1>();
        o2.AddComponent<c1>();

        Client.World.Event.RunEvent(e, rpc: 5);
        if (e.v != 1) throw new Exception();

        Client.World.Event.RunEvent(e, rpc: 4);
        if (e.v != 1) throw new Exception();

        o.AddComponent<c2>();
        o2.AddComponent<c1>();

        Client.World.Event.RunEvent(e, rpc: 5);
        if (e.v != 4) throw new Exception();
        Client.World.Event.RunEvent(e, rpc: 4);
        if (e.v != 4) throw new Exception();

        Client.World.Event.RunEvent(e);
        if (e.v != 8) throw new Exception();

        Client.World.Event.RunEvent(e, gid: o.gid);
        if (e.v != 11) throw new Exception();

        Client.World.Event.RunEvent(e, gid: o2.gid);
        if (e.v != 12) throw new Exception();

        Client.World.Event.RunEvent(e, gid: 4);
        if (e.v != 12) throw new Exception();
        Client.World.Event.RunEvent(e, gid: 5);
        if (e.v != 12) throw new Exception();

        Client.World.Event.RunEvent(e, gid: o.gid, type: 1);
        Client.World.Event.RunEvent(e, gid: o.gid, type: 2);
        if (e.v != 13) throw new Exception();
        Client.World.Event.RunEvent(e, rpc: 5, type: 1);
        Client.World.Event.RunEvent(e, rpc: 5, type: 2);
        if (e.v != 14) throw new Exception();
    }

    class Evt
    {
        public int v;
    }

    class c1 : SComponent { }
    class c2 : SComponent { }

    [Event]
    static void t1(EventWatcher<Evt, c1> t) => t.t.v++;
    [Event(Type = 1)]
    static void t11(EventWatcher<Evt, c1> t) => t.t.v++;
    [Event]
    static void t1(EventWatcher<Evt, c2> t) => t.t.v++;
    [Event]
    static void t1(EventWatcher<Evt, c1, c2> t) => t.t.v++;
}
