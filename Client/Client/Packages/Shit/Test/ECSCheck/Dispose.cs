using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Dispose
{
    public static void test()
    {
        v = 0;

        Client.World.Root.AddComponent<c1>();
        if (v != 0)
            throw new Exception();

        Client.World.Root.RemoveComponent<c1>();

        if (v != 1)
            throw new Exception();

        var o = new SObject();
        o.AddComponent<c1>();
        Client.World.Root.AddChild(o);

        if (v != 1)
            throw new Exception();

        o.Dispose();

        if (v != 3)
            throw new Exception();
    }
    class c1 : SComponent { }

    static int v = 0;
    [Event]
    static void awake(Dispose<c1> t) => v++;
    [Event]
    static void awake(Dispose<SObject> t) => v++;
}
