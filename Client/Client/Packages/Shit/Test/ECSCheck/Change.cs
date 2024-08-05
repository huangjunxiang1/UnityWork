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
        change_TestObj o = new();
        Client.World.Root.AddChild(o);

        o.AddComponent<c_1>();
        Client.World.Update(0);
        if (o.v != 1) throw new System.Exception();

        o.AddComponent<c_2>();
        Client.World.Update(0);
        if (o.v != 3) throw new System.Exception();

        o.GetComponent<c_1>().SetChangeFlag();
        Client.World.Update(0);
        if (o.v != 5) throw new System.Exception();

        o.GetComponent<c_2>().SetChangeFlag();
        Client.World.Update(0);
        if (o.v != 6) throw new System.Exception();

        o.RemoveComponent<c_1>();
        o.GetComponent<c_2>().SetChangeFlag();
        Client.World.Update(0);
        if (o.v != 7) throw new System.Exception();

        o.AddComponent<c_1>();
        Client.World.Update(0);
        if (o.v != 9) throw new System.Exception();

        o.GetComponent<c_1>().SetChangeFlag();
        o.RemoveComponent<c_2>();
        Client.World.Update(0);
        if (o.v != 10) throw new System.Exception();

        o.Dispose();
        Client.World.Update(0);
        if (o.v != 10) throw new System.Exception();

        {
            o = new();
            Client.World.Root.AddChild(o);

            o.AddComponent<c_1>();
            o.AddComponent<c_2>();
            Client.World.Update(0);
            if (o.v != 3) throw new System.Exception();

            o.GetComponent<c_1>().SetChangeFlag();
            if (o.v != 3) throw new System.Exception();
            o.GetComponent<c_1>().SetChange();
            if (o.v != 5) throw new System.Exception();
            Client.World.Update(0);
            if (o.v != 5) throw new System.Exception();

            o.GetComponent<c_1>().SetChangeFlag();
            o.GetComponent<c_2>().SetChangeFlag();
            o.GetComponent<c_2>().SetChange();
            if (o.v != 6) throw new System.Exception();
            Client.World.Update(0);
            if (o.v != 8) throw new System.Exception();
        }
    }

    class c_1 : SComponent { }
    class c_2 : SComponent { }
    class change_TestObj : SObject
    {
        public int v = 0;
        [Event]
        void change_obj(Change<c_1> a) => v++;
        [Event]
        void change_obj(Change<c_2> a) => v++;
        [Event]
        void change_obj(Change<c_1, c_2> a) => v++;
        [Event]
        void change_obj(Change<c_1, c_2, KVComponent> a) => v++;
    }
}
