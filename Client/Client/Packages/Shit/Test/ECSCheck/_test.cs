using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

static class _test
{
    public static void test()
    {
        In.test();
        Out.test();
        Enable.test();
        Change.test();
        AnyChange.test();
        EventWatcher.test();
        KVWatcherTest.test();
        TimerCheck.test();
        UpdateCheck.test();
    }
}
