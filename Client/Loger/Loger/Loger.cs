using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

public static class Loger
{
    public static Action<object> __get__log;
    public static Action<object> __get__warning;
    public static Action<object> __get__error;

    [Conditional("DebugEnable")]
    [Conditional("DEBUG")]
    [DebuggerHidden]
    public static void Log(object o)
    {
        __get__log?.Invoke(o);
    }

    [Conditional("DebugEnable")]
    [Conditional("DEBUG")]
    [DebuggerHidden]
    public static void Warning(object o)
    {
        __get__warning?.Invoke(o);
    }

    [Conditional("DebugEnable")]
    [Conditional("DEBUG")]
    [DebuggerHidden]
    public static void Error(object o)
    {
        __get__error?.Invoke(o);
    }

    [Conditional("DebugEnable")]
    [Conditional("DEBUG")]
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
        Log(str);
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
