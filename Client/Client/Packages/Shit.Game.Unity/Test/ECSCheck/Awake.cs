using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Awake : SUnityObject
{
    public static void test()
    {
        Awake a = new();
        GameM.Update();
        a.v = 0;

        var o = new o1();
        a.AddChild(o);
        GameM.Update();

        if (a.v != 2)
            throw new System.Exception();

        var c = o.AddComponent<c_1>();
        if (c.v != 1)
            throw new System.Exception();
        c.Dispose();

        o.AddComponent<c_2>();
        if (c.v != 1)
            throw new System.Exception();
        c = o.AddComponent<c_1>();
        if (c.v != 2)
            throw new System.Exception();

        a.Dispose();
    }
    class c_1 : SComponent
    {
        public int v;
    }
    class c_2 : SComponent
    {
        public int v;
    }
    class o1 : SUnityObject { }

    int v = 0;
    [Event]
    void awake(Awake<o1> a) => v++;
    [Event]
    void awake(Awake<SObject> a) => v++;
    [Event]
    void awake(Awake<object> a) => v++;

    [Event]
    void awake(Awake<c_1> a) => a.t.v = 1;
    [Event]
    void awake(Awake<c_1, c_2> a) => a.t.v = 2;
}
