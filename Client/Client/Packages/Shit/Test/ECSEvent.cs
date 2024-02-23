using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

static class ECSEvent
{
    public static void test()
    {
        dispose_v = 0;
        change_value1 = 0;
        change_value2 = 0;
        change_value = 0;

        var o = new o1();
        var o2 = new o2();
        change_TestObj o3 = new();

        o.AddComponent<c_1>();
        EditorApplication.update?.Invoke();
        if (change_value1 != 1) throw new System.Exception();
        if (o3.change_value1 != 1) throw new System.Exception();

        o.AddComponent<c_2>();
        EditorApplication.update?.Invoke();
        if (change_value2 != 1) throw new System.Exception();
        if (change_value != 1) throw new System.Exception();
        if (o3.change_value2 != 1) throw new System.Exception();
        if (o3.change_value != 1) throw new System.Exception();

        o.GetComponent<c_1>().SetChange();
        EditorApplication.update?.Invoke();
        if (change_value1 != 2) throw new System.Exception();
        if (change_value != 2) throw new System.Exception();
        if (o3.change_value1 != 2) throw new System.Exception();
        if (o3.change_value != 2) throw new System.Exception();
        o.RemoveComponent<c_1>();

        var c = o.AddComponent<c_1>();
        if (c.v != 5) throw new System.Exception();

        c.MoveToEntity(o2);
        if (c.v != 6) throw new System.Exception();

        o2.Dispose();
        if (c.v != 7) throw new System.Exception();
        if (dispose_v != 2) throw new System.Exception();

        enable_test.test();

        o.Dispose();
        o3.Dispose();
    }

    class c_1 : SComponent
    {
        public int v;
    }
    class c_2 : SComponent
    {
        public int v;
    }
    class o1 : SObject { }
    class o2 : o1 { }


    [Event]
    static void awake_c1(Awake<c_1> a) => a.target.v = 5;
    [Event]
    static void move_c1(Move<c_1> a) => a.target.v = 6;
    [Event]
    static void dispose_c1(Dispose<c_1> a) => a.target.v = 7;

    static int dispose_v = 0;
    [Event]
    static void dispose_Obj(Dispose<o1> a) => dispose_v++;
    [Event]
    static void dispose_Obj(Dispose<SObject> a) => dispose_v++;
    [Event]
    static void dispose_Obj(Dispose<object> a) => dispose_v++;

    static int change_value1 = 0;
    static int change_value2 = 0;
    static int change_value = 0;
    [Event]
    static void change_obj(Change<c_1> a) => change_value1++;
    [Event]
    static void change_obj(Change<c_2> a) => change_value2++;
    [Event]
    static void change_obj(Change<c_1, c_2> a) => change_value++;

    class change_TestObj : SObject
    {
        public int change_value1 = 0;
        public int change_value2 = 0;
        public int change_value = 0;
        [Event]
        void change_obj(Change<c_1> a) => change_value1++;
        [Event]
        void change_obj(Change<c_2> a) => change_value2++;
        [Event]
        void change_obj(Change<c_1, c_2> a) => change_value++;
        [Event]
        void change_obj(Change<c_1, c_2, AttributeComponent> a) => change_value++;
    }

    class enable_test
    {
        class a : SComponent
        {
            public int v;
        }

        public static void test()
        {
            var obj = new SObject().AddComponent<a>();
            obj.Enable = false;
            if (obj.v != 1) throw new Exception();
            obj.Enable = true;
            EditorApplication.update?.Invoke();
            if (obj.v != 3) throw new Exception();
            obj.Entity.Dispose();
        }
        [Event]
        static void enable(Enable<a> e)
        {
            e.target.v++;
        }
        [Event]
        static void change(Change<a> e)
        {
            e.target.v++;
        }
    }
}
