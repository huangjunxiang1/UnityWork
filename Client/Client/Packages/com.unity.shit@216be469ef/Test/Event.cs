using Game;
using Main;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class Event
{
    [Test]
    public static void testEvent()
    {
        typeof(Types).GetMethod("EditorClear", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, null);
        Types.RigisterTypes(typeof(GameM).Assembly.GetTypes());
        Types.RigisterTypes(typeof(Event).Assembly.GetTypes());

        GameM.Init();

        testValue = 0;
        isEnd = false;
        dispose_v = 0;
        change_value1 = 0;
        change_value2 = 0;
        change_value = 0;

        var o = new ObjTest();
        var o2 = new ObjTest2();
        o2.ss = new();
        GameM.World.AddChild(o);
        GameM.World.AddChild(o2);

        GameM.Event.RunEvent(new EC_Event());
        GameM.Event.RunEvent(new Awake<int>(1));

        o2.test();

        async void test()
        {
            try
            {
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

                var v1 = await GameM.Event.RunEventAsync<int>(new EC_Event2());
                if (v1 != 1) throw new System.Exception();
                v1 = await GameM.Event.RunEventAsync<int>(new EC_Event3());
                if (v1 != 3) throw new System.Exception();
                isEnd = true;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                await Task.Delay(100);
                GameM.Close();
            }
        }
        test();

        var c = o.AddComponent<c_1>();
        if (c.v != 5) throw new System.Exception();

        c.MoveToEntity(o2);
        if (c.v != 6) throw new System.Exception();

        o2.Dispose();
        if (c.v != 7) throw new System.Exception();
        if (dispose_v != 2) throw new System.Exception();

        if (testValue != 5) throw new System.Exception();

        enable_test.test();
    }
    class EC_Event
    {
        public int value = 0;
    }
    class EC_Event2 { }
    class EC_Event3 { }
    class c_1 : SComponent
    {
        public int v;
    }
    class c_2 : SComponent
    {
        public int v;
    }
    class c_3 : SComponent { }

    static int testValue;
    static bool isEnd;

    [Event(1)]
    static void t1(EC_Event e)
    {
        if (e.value != 2) throw new System.Exception();
        e.value++;
        testValue++;
    }
    [Event(-1)]
    static void t2(EC_Event e)
    {
        if (e.value != 0) throw new System.Exception();
        e.value++;
        testValue++;
    }
    class ObjTest : SObject
    {
        [Event(1)]
        void t1(EC_Event e)
        {
            if (e.value != 3) throw new System.Exception();
            e.value++;
            testValue++;
        }
        [Event(-1)]
        void t2(EC_Event e)
        {
            if (e.value != 1) throw new System.Exception();
            e.value++;
            testValue++;
        }
    }
    class ObjTest2 : SObject
    {
        public ObjTest3 ss;

        [Event] private EC_Event e1;
        [Event] public EC_Event e2 { get; private set; }
        [Event] private EC_Event e3 { get; }

        public void test()
        {
            if (e1 == null || e2 == null) throw new System.Exception();
            if (e3 != null) throw new System.Exception();
            if (ss.e1 == null || ss.e2 == null) throw new System.Exception();
            if (ss.e3 != null) throw new System.Exception();
        }
    }
    class ObjTest3 : ObjTest2 { }

    [Event]
    static void awake(Awake<int> a)
    {
        if (a.target != 1) throw new System.Exception();
        testValue++;
    }
    [Event]
    static void awake(Awake<string> a)
    {
        testValue++;
    }

    [Event]
    static int r1(EC_Event2 e)
    {
        return 1;
    }
    [Event(Queue = true)]
    static async STask<string> r2(EC_Event3 e)
    {
        await Task.Delay(100);
        if (isEnd) throw new System.Exception();
        return "8";
    }
    [Event]
    static async STask<int> r3(EC_Event3 e)
    {
        if (isEnd) throw new System.Exception();
        return 3;
    }
    [Event]
    static String r4(EC_Event3 e)
    {
        if (isEnd) throw new System.Exception();
        return "99";
    }

    [Event]
    static void awake_c1(Awake<c_1> a) => a.target.v = 5;
    [Event]
    static void move_c1(Move<c_1> a) => a.target.v = 6;
    [Event]
    static void dispose_c1(Dispose<c_1> a) => a.target.v = 7;

    static int dispose_v = 0;
    [Event]
    static void dispose_Obj(Dispose<ObjTest2> a) => dispose_v++;
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
