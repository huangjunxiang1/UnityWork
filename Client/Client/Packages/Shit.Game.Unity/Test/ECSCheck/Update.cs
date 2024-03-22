using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Update
{
    public static void test()
    {
        update_value = 0;

        var o = new SObject();
        GameWorld.Root.AddChild(o);

        o.AddComponent<update_c1>();
        GameWorld.World.Update(0);
        if (update_value != 1) throw new System.Exception();

        o.AddComponent<update_c2>();
        GameWorld.World.Update(0);
        if (update_value != 4) throw new System.Exception();

        o.GetComponent<update_c1>().Enable = false;
        GameWorld.World.Update(0);
        if (update_value != 5) throw new System.Exception();

        o.GetComponent<update_c1>().Enable = true;
        o.RemoveComponent<update_c2>();
        GameWorld.World.Update(0);
        if (update_value != 6) throw new System.Exception();

        o.Dispose();
        GameWorld.World.Update(0);
        if (update_value != 6) throw new System.Exception();
    }

    class update_c1 : SComponent { }
    class update_c2 : SComponent { }
    static int update_value = 0;
    [Event]
    static void update1(Update<update_c1> a) => update_value++;
    [Event]
    static void update1(Update<update_c2> a) => update_value++;
    [Event]
    static void update1(Update<update_c1, update_c2> a) => update_value++;
}
