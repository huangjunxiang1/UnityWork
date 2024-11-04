using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

public static class Loger
{
#if Unity
    [Conditional("DebugEnable")]
#endif
    [DebuggerHidden]
    public static void Log(object o)
    {
#if Unity
        UnityEngine.Debug.Log(o);
#else
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"{DateTime.Now:G}:{o}");
#endif
    }

#if Unity
    [Conditional("DebugEnable")]
#endif
    [DebuggerHidden]
    public static void Warning(object o)
    {
#if Unity
        UnityEngine.Debug.LogWarning(o);
#else
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{DateTime.Now:G}:warning:{o}");
#endif
    }

#if Unity
    [Conditional("DebugEnable")]
#endif
    [DebuggerHidden]
    public static void Error(object o)
    {
#if Unity
        UnityEngine.Debug.LogError(o);
#else
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"{DateTime.Now:G}:error:{o}");
#endif
    }

#if Unity
    [Conditional("DebugEnable")]
#endif
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
