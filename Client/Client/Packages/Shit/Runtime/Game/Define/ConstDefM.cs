using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConstDefM
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
}
