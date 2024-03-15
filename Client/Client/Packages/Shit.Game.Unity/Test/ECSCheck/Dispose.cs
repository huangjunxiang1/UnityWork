using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Dispose
{
    public static void test()
    {
        v = 0;

        var o = new o1();
        o.Dispose();
        GameM.Update();
        if (v != 2)
            throw new Exception();

        o = new();
        var c = o.AddComponent<c_1>();
        c.Dispose();
        if (c.v != 1)
            throw new Exception();

        c = o.AddComponent<c_1>();
        o.AddComponent<c_2>();
        o.GetComponent<c_2>().Dispose();
        if (c.v != 0)
            throw new Exception();

        o.AddComponent<c_2>();
        c.Dispose();
        if (c.v != 3)
            throw new Exception();

        o.Dispose();
    }

    class c_1 : SComponent
    {
        public int v;
    }
    class c_2 : SComponent { }
    class o1 : SUnityObject { }


    static int v = 0;
    [Event]
    static void e(Dispose<o1> a) => v++;
    [Event]
    static void e(Dispose<SObject> a) => v++;
    [Event]
    static void e(Dispose<object> a) => v++;


    [Event]
    static void e(Dispose<c_1> a) => a.t.v = 1;
    [Event]
    static void e(Dispose<c_2> a) { }
    [Event]
    static void e(Dispose<c_1, c_2> a) => a.t.v = 3;

}
