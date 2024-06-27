using Core;
using Game;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

static class _main
{
    [Test]
    public static void test()
    {
        var tsc = ThreadSynchronizationContext.GetOrCreate(System.Environment.CurrentManagedThreadId);
        ThreadSynchronizationContext.SetMainThread(tsc);

        List<Type> types = new();
        types.AddRange(typeof(World).Assembly.GetTypes());
        types.AddRange(typeof(_main).Assembly.GetTypes());
        Client.Load(types);

        _test.test();
        GenericEventTest.test();
        EventTest2.test();
        EventTest.test();
        STaskTest.test();
    }
}
