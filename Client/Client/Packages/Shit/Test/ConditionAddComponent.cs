using Game;
using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

static class ConditionAddComponent
{
    public static void test()
    {
        {
            var o1 = new o1();
            GameM.World.AddChild(o1);

            o1.AddComponent<c1>();
            EditorApplication.update?.Invoke();
            if (o1.HasComponent<tc>())
                throw new Exception();

            o1.AddComponent<c2>();
            EditorApplication.update?.Invoke();
            if (!o1.HasComponent<tc>())
                throw new Exception();

            o1.AddComponent<AttributeComponent>();
            EditorApplication.update?.Invoke();
            if (o1.HasComponent<tc>())
                throw new Exception();

            o1.Dispose();
        }

        {
            var o1 = new o1();
            var o2 = new o2();
            o11 o11 = new o11();
            GameM.World.AddChild(o1);
            GameM.World.AddChild(o2);
            GameM.World.AddChild(o11);

            o1.AddComponent<c1>();
            EditorApplication.update?.Invoke();
            if (!o1.HasComponent<tc2>())
                throw new Exception();

            o11.AddComponent<c1>();
            EditorApplication.update?.Invoke();
            if (!o11.HasComponent<tc2>())
                throw new Exception();

            o2.AddComponent<c1>();
            EditorApplication.update?.Invoke();
            if (o2.HasComponent<tc2>())
                throw new Exception();

            o1.Dispose();
            o2.Dispose();
            o11.Dispose();
        }

    }
    class o1 : SObject { }
    class o2 : SObject { }
    class o11 : o1 { }

    class c1 : SComponent { }
    class c2 : SComponent { }


    [AddComponentIfAll(typeof(c1))]
    [AddComponentIfAny(typeof(c2))]
    [AddComponentIfNone(typeof(AttributeComponent))]
    class tc : SComponent { }

    [AddComponentIfIsSObject(typeof(o1))]
    [AddComponentIfAll(typeof(c1))]
    class tc2 : SComponent { }
}
