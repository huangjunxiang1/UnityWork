using System;
using UnityEngine;
using System.Diagnostics;
using System.Text;

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
    public static void Warning(object o)
    {
        UnityEngine.Debug.LogWarning(o);
    }

    [Conditional("DebugEnable")]
    [DebuggerHidden]
    public static void Error(object o)
    {
        UnityEngine.Debug.LogError(o);
    }

    [Conditional("DebugEnable")]
    [DebuggerHidden]
    public static void LogStackTrace()
    {
        var stacktrace = new StackTrace();
        StringBuilder str = new StringBuilder(stacktrace.FrameCount * 50);
        for (var i = 0; i < stacktrace.FrameCount; i++)
        {
            var method = stacktrace.GetFrame(i).GetMethod();
            str.Append(method.ReflectedType.Name);
            str.Append("::");
            str.AppendLine(method.Name);
        }
        UnityEngine.Debug.Log(str);
    }
}
