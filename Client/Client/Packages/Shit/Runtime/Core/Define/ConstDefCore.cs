using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public static class ConstDefCore
{
    public const string DebugEnableString = "DebugEnable";
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
}
