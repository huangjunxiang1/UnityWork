using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class In
{
    public static void test()
    {
        {
            v = 0;

            var o = new o1();
            Client.World.Root.AddChild(o);

            if (v != 2)
                throw new System.Exception();

            var c = o.AddComponent<c_1>();
            if (v!= 3)
                throw new System.Exception();
            c.Dispose();

            o.AddComponent<c_2>();
            if (v != 3)
                throw new System.Exception();
            o.AddComponent<c_1>();
            if (v != 5)
                throw new System.Exception();

            o.RemoveComponent<c_1>();
            o.RemoveComponent<c_2>();

            o.AddComponent<c_1>();
            o.AddComponent<c_2>();
            if (v != 7)
                throw new System.Exception();

            o.Dispose();
        }

        {
            var o = new o1();

            o.AddComponent<c_1>();
            Client.World.Update();
            if (v != 7)
                throw new System.Exception();
            Client.World.Root.AddChild(o);
            if (v != 10)
                throw new System.Exception();
            o.Dispose();
        }
    }
    class c_1 : SComponent { }
    class c_2 : SComponent { }
    class o1 : SObject { }

    static int v = 0;
    [InSystem]
    static void awake(o1 a) => v++;
    [InSystem]
    static void awake(SObject a) => v++;

    [InSystem]
    static void awake(c_1 a) => v++;
    [InSystem]
    static void awake(c_1 a, c_2 b) => v++;
}
