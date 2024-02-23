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

class Event
{
    public static void test()
    {
        testValue = 0;
        isEnd = false;

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
                GameM.World.DisposeAllChildren();
                GameM.Close();
            }
        }
        test();

        if (testValue != 5) throw new System.Exception();
    }
    class EC_Event
    {
        public int value = 0;
    }
    class EC_Event2 { }
    class EC_Event3 { }

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
    class ObjTest2_b : SObject
    {
        [Event] private EC_Event e11;
        [Event] public EC_Event e22 { get; private set; }

        protected void test_b()
        {
            if (e11 == null || e22 == null) throw new System.Exception();
        }
    }
    class ObjTest2 : ObjTest2_b
    {
        public ObjTest3 ss;

        [Event] private static EC_Event e0;
        [Event] public static EC_Event e00 { get; private set; }
        [Event] private EC_Event e1;
        [Event] public EC_Event e2 { get; private set; }
        [Event] private EC_Event e3 { get; }

        public void test()
        {
            if (e0 == null) throw new System.Exception();
            if (e00 == null) throw new System.Exception();
            if (e1 == null || e2 == null) throw new System.Exception();
            base.test_b();
            if (e3 != null) throw new System.Exception();
            if (ss.e1 == null || ss.e2 == null) throw new System.Exception();
            if (ss.e3 != null) throw new System.Exception();
            ss.Dispose();
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

}
