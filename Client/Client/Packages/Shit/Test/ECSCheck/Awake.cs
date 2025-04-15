using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class Awake
{
    public static void test()
    {
        v = 0;

        Client.World.Root.AddComponent<c1>();
        if (v != 1)
            throw new Exception();

        Client.World.Root.RemoveComponent<c1>();

        var o = new o1();
        Client.World.Root.AddChild(o);

        if (v != 6)
            throw new Exception();

        o.Dispose();
    }

    class c1 : SComponent { }
    class o1 : SObject { }

    static int v = 0;
    [AwakeSystem]
    static void awake(c1 t) => v++;
    [AwakeSystem]
    static void awake(SObject t) => v = 5;
    [AwakeSystem]
    static void awake(o1 t) => v++;
}
