using System;
using UnityEngine;
using System.Diagnostics;

public static class Loger
{
    [Conditional("DebugEnable")]
    [DebuggerHidden]
    public static void Log(object o)
    {
        UnityEngine.Debug.Log(o);
    }

    [Conditional("DebugEnable")]
    [DebuggerHidden]
    public static void Error(object o)
    {
        UnityEngine.Debug.LogError(o);
    }
}
