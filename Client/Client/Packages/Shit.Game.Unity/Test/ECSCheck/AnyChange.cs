using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class AnyChange
{
    public static void test()
    {
        change_TestObj o = new();
        GameWorld.Root.AddChild(o);

        o.AddComponent<c_1>();
        GameWorld.World.Update(0);
        if (o.v != 0) throw new System.Exception();

        o.AddComponent<c_2>();
        GameWorld.World.Update(0);
        if (o.v != 1) throw new System.Exception();

        o.GetComponent<c_1>().SetChange();
        GameWorld.World.Update(0);
        if (o.v != 2) throw new System.Exception();

        o.GetComponent<c_2>().SetChange();
        GameWorld.World.Update(0);
        if (o.v != 3) throw new System.Exception();

        o.Dispose();
        GameWorld.World.Update(0);
        if (o.v != 3) throw new System.Exception();
    }

    class c_1 : SComponent { }
    class c_2 : SComponent { }
    class change_TestObj : SObject
    {
        public int v = 0;
        [Event]
        void change_obj(AnyChange<c_1, c_2> a) => v++;
        [Event]
        void change_obj(AnyChange<c_1, c_2, AttributeComponent> a) => v++;
    }
}
