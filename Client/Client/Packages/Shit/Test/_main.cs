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
        var tsc = ThreadSynchronizationContext.GetOrCreate(Thread.CurrentThread.ManagedThreadId);
        ThreadSynchronizationContext.SetMainThread(tsc);
        Loger.__get__log += o => ThreadSynchronizationContext.MainThread.Post(() => UnityEngine.Debug.Log(o + Loger.GetStackTrace()));
        Loger.__get__warning += o => ThreadSynchronizationContext.MainThread.Post(() => UnityEngine.Debug.LogWarning(o + Loger.GetStackTrace()));
        Loger.__get__error += o => ThreadSynchronizationContext.MainThread.Post(() => UnityEngine.Debug.LogError(o + Loger.GetStackTrace()));

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
