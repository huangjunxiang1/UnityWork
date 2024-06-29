using Core;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class typeLstCheck
{
    public static void test()
    {
        o1 v_o1 = new() { ObjType = 1 };
        o1 v_o2 = new();
        o1 v_o3 = new() { ObjType = 2 };

        Client.World.Root.AddChild(v_o1);
        Client.World.Root.AddChild(v_o2);
        Client.World.Root.AddChild(v_o3);

        if (Client.World.Root.GetChildrenByObjType(1) != null)
            throw new Exception();
        if (Client.World.Root.GetChildrenByObjType(2) != null)
            throw new Exception();

        o2 v2_o1 = new() { isCrucialRoot = true };
        {
            Client.World.Root.AddChild(v2_o1);
            v2_o1.AddChild(v_o1);
            v2_o1.AddChild(v_o2);
            v2_o1.AddChild(v_o3);

            if (v2_o1.GetChildrenByObjType(1).Count != 1)
                throw new Exception();
            if (v2_o1.GetChildrenByObjType(1).FirstOrDefault() != v_o1)
                throw new Exception();
            if (v2_o1.GetChildrenByObjType(2).Count != 1)
                throw new Exception();
            if (v2_o1.GetChildrenByObjType(2).FirstOrDefault() != v_o3)
                throw new Exception();
        }
        o2 v2_o2 = new() { isCrucialRoot = true };
        v2_o1.AddChild(v2_o2);
        {
            v2_o2.AddChild(v_o1);
            v2_o2.AddChild(v_o2);
            v2_o2.AddChild(v_o3);

            if (v2_o1.GetChildrenByObjType(1).Count != 0)
                throw new Exception();
            if (v2_o1.GetChildrenByObjType(2).Count != 0)
                throw new Exception();

            if (v2_o2.GetChildrenByObjType(1).Count != 1)
                throw new Exception();
            if (v2_o2.GetChildrenByObjType(1).FirstOrDefault() != v_o1)
                throw new Exception();
            if (v2_o2.GetChildrenByObjType(2).Count != 1)
                throw new Exception();
            if (v2_o2.GetChildrenByObjType(2).FirstOrDefault() != v_o3)
                throw new Exception();
        }

        v_o1.Dispose();
        v_o2.Dispose();
        v_o3.Dispose();

        if (v2_o1.GetChildrenByObjType(1).Count != 0)
            throw new Exception();
        if (v2_o1.GetChildrenByObjType(2).Count != 0)
            throw new Exception();
        if (v2_o2.GetChildrenByObjType(1).Count != 0)
            throw new Exception();
        if (v2_o2.GetChildrenByObjType(2).Count != 0)
            throw new Exception();

        v2_o1.Dispose();
    }

    class o1 : SObject { }
    class o2 : STree { }
}
