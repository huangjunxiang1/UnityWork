using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Enable
{
    public static void test()
    {
        enable_test.test();
    }


    class enable_test
    {
        class a : SComponent
        {
            public int v;
        }

        public static void test()
        {
            var o = new SObject();
            GameWorld.Root.AddChild(o);

            var obj = o.AddComponent<a>();
            obj.Enable = false;
            if (obj.v != 1) throw new Exception();
            obj.Enable = true;
            GameWorld.World.Update(0);
            if (obj.v != 3) throw new Exception();
            obj.Entity.Dispose();
        }
        [Event]
        static void enable(Enable<a> e)
        {
            e.t.v++;
        }
        [Event]
        static void change(Change<a> e)
        {
            e.t.v++;
        }
    }
}