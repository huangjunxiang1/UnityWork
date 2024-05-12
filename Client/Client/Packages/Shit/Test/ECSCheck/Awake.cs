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

        var o = new SObject();
        Client.World.Root.AddChild(o);

        if (v != 2)
            throw new Exception();

        o.Dispose();
    }

    class c1 : SComponent { }

    static int v = 0;
    [Event]
    static void awake(Awake<c1> t) => v++;
    [Event]
    static void awake(Awake<SObject> t) => v++;
}
