using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

public class Loger
{
    [DebuggerNonUserCode]
    [DebuggerHidden]
    public static void Log(object o)
    {
        UnityEngine.Debug.Log(o);
    }

    [DebuggerNonUserCode]
    [DebuggerHidden]
    public static void Error(object o)
    {
        UnityEngine.Debug.LogError(o);
    }
}