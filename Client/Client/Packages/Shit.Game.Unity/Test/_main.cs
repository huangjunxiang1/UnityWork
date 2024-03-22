using Core;
using Game;
using NUnit.Framework;
using System;
using System.Collections.Generic;

static class _main
{
    [Test]
    public static void test()
    {
        _ = GameWorld.Init(new List<System.Reflection.Assembly> { typeof(CoreWorld).Assembly, typeof(_main).Assembly });

        _test.test();
        EventTest2.test();
        EventTest.test();
        STaskTest.test();
    }
}
