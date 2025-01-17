using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

static class EcsSystem
{
    public static void test()
    {
        Awake.test();
        Dispose.test();
        In.test();
        Out.test();
        Change.test();
        AnyChange.test();
        EventWatcher.test();
        KVWatcherTest.test();
        TimerCheck.test();
        UpdateCheck.test();
    }
}
