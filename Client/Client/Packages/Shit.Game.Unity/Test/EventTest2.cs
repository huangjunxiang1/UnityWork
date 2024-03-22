using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class EventTest2
{
    public static void test()
    {
        GameWorld.World.Event.RigisteEvent(new Action<Event1>(e1));
        GameWorld.World.Event.RigisteEvent(new Action<Event1, EventHandler>(e1));
        GameWorld.World.Event.RigisteEvent(new Func<Event1, STask>(e11));
        GameWorld.World.Event.RigisteEvent(new Func<Event1, EventHandler, STask>(e11));

        var a = new a();
        GameWorld.World.Event.RigisteEvent((Delegate)new Action<Event1>(a.e1));
        GameWorld.World.Event.RigisteEvent((Delegate)new Action<Event1, EventHandler>(a.e1));
        GameWorld.World.Event.RigisteEvent((Delegate)new Func<Event1, STask>(a.e11));
        GameWorld.World.Event.RigisteEvent((Delegate)new Func<Event1, EventHandler, STask>(a.e11));

        Event1 e = new();
        GameWorld.World.Event.RunEvent(e);

        if (e.v != 8)
            throw new Exception();
    }

    class Event1
    {
        public int v;
    }

    static void e1(Event1 e) => e.v++;
    static void e1(Event1 e, EventHandler eh) => e.v++;
    static async STask e11(Event1 e) => e.v++;
    static async STask e11(Event1 e, EventHandler eh) => e.v++;

    class a
    {
        public void e1(Event1 e) => e.v++;
        public void e1(Event1 e, EventHandler eh) => e.v++;
        public async STask e11(Event1 e) => e.v++;
        public async STask e11(Event1 e, EventHandler eh) => e.v++;
    }
}