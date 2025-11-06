using Core;
using Game;
using System.Threading.Tasks;

class EventTest
{
    class aa
    {
        public static int bb;
        [Event]
        static void test(EC_Event t)
        {
            bb++;
        }
    }
    public static void test()
    {
        testValue = 0;
        isEnd = false;

        var o = new ObjTest();
        var o2 = new ObjTest2();
        o2.ss = new();

        Client.World.Root.AddChild(o);
        Client.World.Root.AddChild(o2);
        Client.World.Root.AddChild(o2.ss);
        var c = Client.World.Root.AddComponent<c1>();

        Client.World.Event.RigisteEvent<EC_Event>(t0, 2);

        aa.bb = 0;
        Client.World.Event.RunEvent(new EC_Event());
        if (aa.bb != 1)
            throw new System.Exception();

        o2.test();
        c.test();

        async void test()
        {
            try
            {
                var e2 = new EC_Event2();
                await Client.World.Event.RunEventAsync(e2);
                if (e2.v1 != 1) throw new System.Exception();
                var e3 = new EC_Event3();
                await Client.World.Event.RunEventAsync(e3);
                if (e3.v1 != 3) throw new System.Exception();
                isEnd = true;
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                await Task.Delay(100);
                Client.World.Dispose();
            }
        }
        test();

        if (testValue != 4) throw new System.Exception();
    }
    class EC_Event
    {
        public int value = 0;
    }
    class EC_Event2
    {
        public int v1;
    }
    class EC_Event3
    {
        public int v1;
    }
    class c1 : SComponent
    {
        class e_test { }
        [Event]
        e_test e;

        e_test eee;

        [Event]
        void ee(e_test e) => eee = e;

        public void test()
        {
            this.World.Event.RunEvent(new e_test());
            if (e == null) throw new System.Exception();
            if (eee == null) throw new System.Exception();
            e = null;
            eee = null;
            this.Dispose();
            this.World.Event.RunEvent(new e_test());
            if (e != null) throw new System.Exception();
            if (eee != null) throw new System.Exception();

            SObject s = new() { ActorId = 999 };
            var c = s.AddComponent<c1>();
            Client.World.Root.AddChild(s);
            c.test2();
            s.Dispose();
        }
        void test2()
        {
            this.World.Event.RunEvent(new e_test(), 5);
            if (e != null) throw new System.Exception();
            if (eee != null) throw new System.Exception();
            this.World.Event.RunEvent(new e_test(), 999);
            if (e == null) throw new System.Exception();
            if (eee == null) throw new System.Exception();
        }
    }

    static int testValue;
    static bool isEnd;

    static void t0(EC_Event e)
    {
        if (e.value != 4) throw new System.Exception();
        e.value++;
    }
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
    [Event]
    static void r1(EC_Event2 e)
    {
        e.v1 = 1;
    }
    [Event(Queue = true)]
    static async STask r2(EC_Event3 e)
    {
        await Task.Delay(100);
        if (isEnd) throw new System.Exception();
    }
    [Event]
    static void r3(EC_Event3 e)
    {
        e.v1 = 3;
        if (isEnd) throw new System.Exception();
    }
}
