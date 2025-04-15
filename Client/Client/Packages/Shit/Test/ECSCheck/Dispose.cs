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

        var o = new o1();
        o.AddComponent<c1>();
        Client.World.Root.AddChild(o);

        if (v != 1)
            throw new Exception();

        o.Dispose();

        if (v != 4)
            throw new Exception();
    }
    class c1 : SComponent { }
    class o1 : SObject { }

    static int v = 0;
    [DisposeSystem]
    static void awake(c1 t) => v++;
    [DisposeSystem]
    static void awake(SObject t) => v++;
    [DisposeSystem]
    static void awake(o1 t) => v++;
}
