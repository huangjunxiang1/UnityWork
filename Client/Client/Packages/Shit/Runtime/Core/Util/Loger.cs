using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

public static class Loger
{
    [DebuggerHidden]
    public static void Info(object o)
    {
        UnityEngine.Debug.Log(o);
    }

    [Conditional(SSetting.CoreSetting.DebugEnableString)]
    [DebuggerHidden]
    public static void Error(object o)
    {
        UnityEngine.Debug.LogError(o);
    }
    [Conditional(SSetting.CoreSetting.DebugEnableString)]
    [DebuggerHidden]
    public static void Log(object o)
    {
        UnityEngine.Debug.Log(o);
    }
    [Conditional(SSetting.CoreSetting.DebugEnableString)]
    [DebuggerHidden]
    public static void LogStackTrace()
    {
        Log(new StackTrace());
    }
    [Conditional(SSetting.CoreSetting.DebugEnableString)]
    [DebuggerHidden]
    public static void Warning(object o)
    {
        UnityEngine.Debug.LogWarning(o);
    }
}