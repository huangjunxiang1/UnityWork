using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CodeRuntime
{
    Native,
    Assembly,
    ILRuntime,
}
public static partial class ConstDefM
{
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
