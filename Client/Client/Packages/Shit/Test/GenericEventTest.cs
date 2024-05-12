using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class GenericEventTest
{
    public static void test()
    {
        v = 0;
        Client.World.Event.RunGenericEvent(typeof(Event<>), new a());
        if (v != 1)
            throw new Exception();
        Client.World.Event.RunGenericEvent(typeof(Event<>), new b());
        if (v != 2)
            throw new Exception();

        Client.World.Event.RunGenericEventAndBaseType(typeof(Event<>), new b());
        if (v != 4)
            throw new Exception();
    }

    class a { }
    class b : a { }
    class Event<T> : GenericEvent
    {
        public T t { get; }
        public Event(T t) => this.t = t;
    }

    static int v = 0;
    [Event]
    static void event1(Event<a> t) => v++;
    [Event]
    static void event1(Event<b> t) => v++;
}
