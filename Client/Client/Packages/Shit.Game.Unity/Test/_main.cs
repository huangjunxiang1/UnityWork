using NUnit.Framework;
using System.Collections.Generic;

static class _main
{
    [Test]
    public static void test()
    {
        _ = GameM.Init(new List<System.Reflection.Assembly> { typeof(GameM).Assembly, typeof(_main).Assembly });

        _test.test();
        EventTest2.test();
        EventTest.test();
        STaskTest.test();
    }
}
