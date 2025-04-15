using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Out
{
    public static void test()
    {
        v = 0;

        var o = new o1();
        Client.World.Root.AddChild(o);

        if (v != 0)
            throw new Exception();
        o.Dispose();
        if (v != 2)
            throw new Exception();

        o = new();
        Client.World.Root.AddChild(o);

        var c = o.AddComponent<c_1>();
        c.Dispose();
        if (v != 3)
            throw new Exception();

        c = o.AddComponent<c_1>();
        o.AddComponent<c_2>();
        o.GetComponent<c_2>().Dispose();
        if (v != 5)
            throw new Exception();

        o.AddComponent<c_2>();
        c.Dispose();
        if (v != 7)
            throw new Exception();

        o.AddComponent<c_1>();

        o.GetComponent<c_2>().Enable = false;
        if (v != 9)
            throw new Exception();

        o.Dispose();
        if (v != 12)
            throw new Exception();
    }

    class c_1 : SComponent { }
    class c_2 : SComponent { }
    class o1 : SObject { }


    static int v = 0;
    [OutSystem]
    static void e(o1 a) => v++;
    [OutSystem]
    static void e(SObject a) => v++;


    [OutSystem]
    static void e(c_1 a) => v++;
    [OutSystem]
    static void e(c_2 a) => v++;
    [OutSystem]
    static void e(c_1 a, c_2 b) => v++;

}
