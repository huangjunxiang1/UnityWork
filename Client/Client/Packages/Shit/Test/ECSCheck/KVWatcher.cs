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

        o.World.Update();
        if (a != 0) throw new Exception();

        var kv = o.AddComponent<KVComponent>();
        o.World.Update();
        if (a != 0) throw new Exception();

        kv.Set(1, 5);
        o.World.Update();
        if (a != 1) throw new Exception();
        kv.Set(1, 5);
        o.World.Update();
        if (a != 1) throw new Exception();

        o.AddComponent<c1>();
        kv.Set(1, 6);
        o.World.Update();
        if (a != 3) throw new Exception();

        kv.Clear();
        kv.Set(new Dictionary<int, long>
        { 
            [10002] = 100, 
            [15002] = 5000,
        });
        if (kv.Get(1) != 150)
            throw new Exception();

        {
            kv.Clear();
            kv.Set(1, 10, 3);
            kv.Set(1, 10, 4);
            if (kv.Get(1) != 20)
                throw new Exception();
            kv.Add(10001, 1000, 5);
            if (kv.Get(1) != 22)
                throw new Exception();

            kv.RemoveAllBySource(4);
            if (kv.Get(1) != 11)
                throw new Exception();

            kv.Remove(10001, 5);
            if (kv.Get(1) != 10)
                throw new Exception();

            kv.RemoveAllByID(1);
            if (kv.Get(1) != 0)
                throw new Exception();
        }

        {
            kv.Clear();
            kv.Set(1, 10, 3);
            kv.Set(1, 10, 4);
            if (kv.Get(1) != 20)
                throw new Exception();
            kv.Add(10001, 1000, 3);
            if (kv.Get(1) != 22)
                throw new Exception();

            kv.RemoveAllBySource(3);
            if (kv.Get(1) != 10)
                throw new Exception();
        }

        o.Dispose();
        o.World.Update();
    }

    static int a = 0;

    class c1 : SComponent { }
    class c2 : SComponent { }

    [KVWatcherSystem(1)]
    static void e1(KVComponent t) => a++;
    [KVWatcherSystem(2)]
    static void e2(KVComponent t) => a++;

    [KVWatcherSystem(1)]
    static void e1(c1 t) => a++;
    [KVWatcherSystem(2)]
    static void e2(c1 t) => a++;

    [KVWatcherSystem(1)]
    static void e1(c1 c, c2 b) => a++;
    [KVWatcherSystem(2)]
    static void e2(c1 c, c2 b) => a++;
}
