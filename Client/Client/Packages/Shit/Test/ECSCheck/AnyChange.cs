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
        v = 0;
        change_TestObj o = new();
        Client.World.Root.AddChild(o);

        o.AddComponent<c_1>();
        Client.World.Update();
        if (v != 0) throw new System.Exception();

        o.AddComponent<c_2>();
        Client.World.Update();
        if (v != 1) throw new System.Exception();

        o.GetComponent<c_1>().SetChangeFlag();
        Client.World.Update();
        if (v != 2) throw new System.Exception();

        o.GetComponent<c_2>().SetChangeFlag();
        Client.World.Update();
        if (v != 3) throw new System.Exception();

        o.GetComponent<c_1>().Dispose();
        Client.World.Update();
        if (v != 3) throw new System.Exception();

        o.AddComponent<c_1>();
        Client.World.Update();
        if (v != 4) throw new System.Exception();

        o.GetComponent<c_2>().Dispose();
        Client.World.Update();
        if (v != 4) throw new System.Exception();

        o.AddComponent<c_2>();
        Client.World.Update();
        if (v != 5) throw new System.Exception();

        o.GetComponent<c_1>().Enable = false;
        Client.World.Update();
        if (v != 5) throw new System.Exception();

        o.GetComponent<c_1>().Enable = true;
        Client.World.Update();
        if (v != 6) throw new System.Exception();

        o.GetComponent<c_2>().Enable = false;
        Client.World.Update();
        if (v != 6) throw new System.Exception();

        o.GetComponent<c_2>().Enable = true;
        Client.World.Update();
        if (v != 7) throw new System.Exception();

        o.GetComponent<c_1>().SetChangeFlag();
        o.GetComponent<c_1>().Enable = false;
        Client.World.Update();
        if (v != 7) throw new System.Exception();

        o.GetComponent<c_1>().Enable = true;
        Client.World.Update();
        if (v != 8) throw new System.Exception();

        o.GetComponent<c_2>().SetChangeFlag();
        o.GetComponent<c_2>().Enable = false;
        Client.World.Update();
        if (v != 8) throw new System.Exception();

        o.GetComponent<c_2>().Enable = true;
        Client.World.Update();
        if (v != 9) throw new System.Exception();

        o.Dispose();
        Client.World.Update();
        if (v != 9) throw new System.Exception();
    }

    class c_1 : SComponent { }
    class c_2 : SComponent { }
    static int v = 0;
    class change_TestObj : SObject
    {
        [AnyChangeSystem]
        static void change_obj(c_1 a, c_2 b) => v++;
        [AnyChangeSystem]
        static void change_obj(c_1 a, c_2 b, KVComponent c) => v++;
    }
}
