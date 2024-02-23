using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class _main
{
    [Test]
    public static void test()
    {
        Types.RigisterTypes(typeof(GameM).Assembly.GetTypes(), typeof(_main).Assembly.GetTypes());

        GameM.Init();

        ConditionAddComponent.test();
        ECSEvent.test();
        Event.test();
    }
}
