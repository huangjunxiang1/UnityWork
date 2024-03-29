using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class KVWatcherTest
{
    public static void test()
    {
        a = 0;
        SObject o = new();
        Client.World.Root.AddChild(o);

        o.World.Update(0);
        if (a != 0) throw new Exception();

        var kv = o.AddComponent<KVComponent>();
        o.World.Update(0);
        if (a != 0) throw new Exception();

        kv.Set((int)KType.MoveSpeed, 5);
        o.World.Update(0);
        if (a != 1) throw new Exception();
        kv.Set((int)KType.MoveSpeed, 5);
        o.World.Update(0);
        if (a != 1) throw new Exception();

        o.AddComponent<c1>();
        kv.Set((int)KType.MoveSpeed, 6);
        o.World.Update(0);
        if (a != 3) throw new Exception();

        o.Dispose();
        o.World.Update(0);
    }

    static int a = 0;

    class c1 : SComponent { }
    class c2 : SComponent { }

    [Event(Type = (int)KType.MoveSpeed)]
    static void e1(KVWatcher t) => a++;
    [Event(Type = (int)KType.RotateSpeed)]
    static void e2(KVWatcher t) => a++;

    [Event(Type = (int)KType.MoveSpeed)]
    static void e1(KVWatcher<c1> t) => a++;
    [Event(Type = (int)KType.RotateSpeed)]
    static void e2(KVWatcher<c1> t) => a++;

    [Event(Type = (int)KType.MoveSpeed)]
    static void e1(KVWatcher<c1, c2> t) => a++;
    [Event(Type = (int)KType.RotateSpeed)]
    static void e2(KVWatcher<c1, c2> t) => a++;
}
