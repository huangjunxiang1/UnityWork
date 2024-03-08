using Main;
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
        Types.EditorClear();
        SSystem.EditorClear();
        STimer.EditorClear();

        _ = GameM.Init(typeof(_main).Assembly.GetTypes());

        ConditionAddComponent.test();
        ECSEvent.test();
        EventTest.test();
    }
}
