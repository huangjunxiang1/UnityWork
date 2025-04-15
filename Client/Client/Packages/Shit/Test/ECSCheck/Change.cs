using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Change
{
    public static void test()
    {
        v = 0;
        change_TestObj o = new();
        Client.World.Root.AddChild(o);

        o.AddComponent<c_1>();
        Client.World.Update();
        if (v != 1) throw new System.Exception();

        o.AddComponent<c_2>();
        Client.World.Update();
        if (v != 3) throw new System.Exception();

        o.GetComponent<c_1>().SetChangeFlag();
        Client.World.Update();
        if (v != 5) throw new System.Exception();

        o.GetComponent<c_2>().SetChangeFlag();
        Client.World.Update();
        if (v != 6) throw new System.Exception();

        o.RemoveComponent<c_1>();
        o.GetComponent<c_2>().SetChangeFlag();
        Client.World.Update();
        if (v != 7) throw new System.Exception();

        o.AddComponent<c_1>();
        Client.World.Update();
        if (v != 9) throw new System.Exception();

        o.GetComponent<c_1>().SetChangeFlag();
        o.RemoveComponent<c_2>();
        Client.World.Update();
        if (v != 10) throw new System.Exception();

        o.AddComponent<c_2>();
        Client.World.Update();
        if (v != 12) throw new System.Exception();

        o.GetComponent<c_1>().SetChangeFlag();
        o.GetComponent<c_1>().Enable = false;
        Client.World.Update();
        if (v != 12) throw new System.Exception();

        o.GetComponent<c_1>().Enable = true;
        Client.World.Update();
        if (v != 14) throw new System.Exception();

        o.GetComponent<c_2>().Enable = false;
        o.GetComponent<c_2>().SetChangeFlag();
        Client.World.Update();
        if (v != 14) throw new System.Exception();

        o.GetComponent<c_2>().Enable = true;
        Client.World.Update();
        if (v != 15) throw new System.Exception();

        o.Dispose();
        Client.World.Update();
        if (v != 15) throw new System.Exception();

    }

    class c_1 : SComponent { }
    class c_2 : SComponent { }
    static int v = 0;
    class change_TestObj : SObject
    {
        [ChangeSystem]
        static void change_obj(c_1 a) => v++;
        [ChangeSystem]
        static void change_obj(c_2 a) => v++;
        [ChangeSystem]
        static void change_obj(c_1 a, c_2 b) => v++;
        [ChangeSystem]
        static void change_obj(c_1 a, c_2 b, KVComponent c) => v++;
    }
}
