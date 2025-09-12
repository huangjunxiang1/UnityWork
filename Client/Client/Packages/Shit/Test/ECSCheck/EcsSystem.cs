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
        Change.test();
        AnyChange.test();
        UpdateCheck.test();
        In.test();
        Out.test();
        EventWatcher.test();
        KVWatcherTest.test();
        //TimerCheck.test();
    }
}
