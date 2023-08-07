using System;
using UnityEngine;
using System.Diagnostics;
using System.Text;
using System.Threading;

public static class Loger
{
    public static int MainThreadID;
    public static Action<Action> PostToMainThread;

    [Conditional("DebugEnable")]
    [DebuggerHidden]
    public static void Log(object o)
    {
        if (MainThreadID == 0 || MainThreadID == Thread.CurrentThread.ManagedThreadId)
            UnityEngine.Debug.Log(o);
        else
            PostToMainThread?.Invoke(() => UnityEngine.Debug.Log(o));
    }

    [Conditional("DebugEnable")]
    [DebuggerHidden]
    public static void Warning(object o)
    {
        if (MainThreadID == 0 || MainThreadID == Thread.CurrentThread.ManagedThreadId)
            UnityEngine.Debug.LogWarning(o);
        else
            PostToMainThread?.Invoke(() => UnityEngine.Debug.LogWarning(o));
    }

    [Conditional("DebugEnable")]
    [DebuggerHidden]
    public static void Error(object o)
    {
        if (MainThreadID == 0 || MainThreadID == Thread.CurrentThread.ManagedThreadId)
            UnityEngine.Debug.LogError(o);
        else
            PostToMainThread?.Invoke(() => UnityEngine.Debug.LogError(o));
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

        if (MainThreadID == 0 || MainThreadID == Thread.CurrentThread.ManagedThreadId)
            UnityEngine.Debug.Log(str);
        else
            PostToMainThread?.Invoke(() => UnityEngine.Debug.Log(str));
    }

    public static string GetStackTrace()
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
        return str.ToString();
    }
}
