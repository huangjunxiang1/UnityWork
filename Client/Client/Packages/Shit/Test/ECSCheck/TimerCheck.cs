using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class TimerCheck
{
    public static void test()
    {
        v = 0;

        SObject o = new();
        Client.World.Root.AddChild(o);

        o.AddComponent<c1>();
        Client.World.Update();
        if (v != 2) throw new Exception();

        Client.World.Update();
        if (v != 4) throw new Exception();

        Client.World.Update();
        if (v != 5) throw new Exception();

        o.AddComponent<c2>();
        Client.World.Update();
        if (v != 8) throw new Exception();

        Client.World.Update();
        if (v != 11) throw new Exception();

        Client.World.Update();
        if (v != 13) throw new Exception();

        o.Dispose();
    }
    class c1 : SComponent { }
    class c2 : SComponent { }

    static int v = 0;

    [Timer(0, -1)]
    static void t1(Timer<c1> t) => v++;
    [Timer(0, 2)]
    static void t11(Timer<c1> t) => v++;

    [Timer(0, -1)]
    static void t1(Timer<c1, c2> t) => v++;
    [Timer(0, 2)]
    static void t11(Timer<c1, c2> t) => v++;
}
