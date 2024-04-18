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
        SObject o = new(5);
        SObject o2 = new(6);
        Client.World.Root.AddChild(o);
        Client.World.Root.AddChild(o2);
        Evt e = new();

        o.AddComponent<c1>();
        o2.AddComponent<c1>();

        Client.World.Event.RunRPCEvent(5, e);
        if (e.v != 1) throw new Exception();

        Client.World.Event.RunRPCEvent(4, e);
        if (e.v != 1) throw new Exception();

        o.AddComponent<c2>();
        o2.AddComponent<c1>();

        Client.World.Event.RunRPCEvent(5, e);
        if (e.v != 4) throw new Exception();
        Client.World.Event.RunRPCEvent(4, e);
        if (e.v != 4) throw new Exception();
    }

    class Evt
    {
        public int v;
    }

    class c1 : SComponent { }
    class c2 : SComponent { }

    [Event]
    static void t1(EventWatcher<Evt, c1> t) => t.t.v++;
    [Event]
    static void t1(EventWatcher<Evt, c2> t) => t.t.v++;
    [Event]
    static void t1(EventWatcher<Evt, c1, c2> t) => t.t.v++;
}
