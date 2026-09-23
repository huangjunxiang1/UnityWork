using Core;
using Event;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;

static class _main
{
    [Test]
    public static void test()
    {
        ThreadSynchronizationContext.GetOrCreate(System.Environment.CurrentManagedThreadId);

        List<Type> types = new();
        types.AddRange(typeof(World).Assembly.GetTypes());
        types.AddRange(typeof(_main).Assembly.GetTypes());
        Game.Initialize(types);

        TypeLstCheck.test();
        EcsSystem.test();
        EventTest.Test();
        STaskTest.test();
    }
}
