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

        o.AddComponent<c_1>();
        GameM.Update();
        if (o.v != 1) throw new System.Exception();

        o.AddComponent<c_2>();
        GameM.Update();
        if (o.v != 3) throw new System.Exception();

        o.GetComponent<c_1>().SetChange();
        GameM.Update();
        if (o.v != 5) throw new System.Exception();

        o.RemoveComponent<c_1>();
        GameM.Update();
        if (o.v != 5) throw new System.Exception();

        o.Dispose();
        GameM.Update();
        if (o.v != 5) throw new System.Exception();
    }

    class c_1 : SComponent { }
    class c_2 : SComponent { }
    class change_TestObj : SUnityObject
    {
        public int v = 0;
        [Event]
        void change_obj(Change<c_1> a) => v++;
        [Event]
        void change_obj(Change<c_2> a) => v++;
        [Event]
        void change_obj(Change<c_1, c_2> a) => v++;
        [Event]
        void change_obj(Change<c_1, c_2, AttributeComponent> a) => v++;
    }

}
