using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CodeRuntime
{
    Native,
    Assembly,
    ILRuntime,
}
public static class ConstDefM
{
    public const string LoginAddressInner = "127.0.0.1:10002";
    public const string LoginAddressOuter = "139.155.0.67:10002";

    public static bool Debug
    {
        get
        {
#if DebugEnable
            return true;
#else 
            return false;
#endif
        }
    }
    public static bool isILRuntime
    {
        get
        {
#if ILRuntime
            return true;
#else 
            return false;
#endif
        }
    }
}
