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
        List<Type> types = new();
        types.AddRange(typeof(World).Assembly.GetTypes());
        types.AddRange(typeof(_main).Assembly.GetTypes());
        Client.Load(types);

        _test.test();
        EventTest2.test();
        EventTest.test();
        STaskTest.test();
    }
}
