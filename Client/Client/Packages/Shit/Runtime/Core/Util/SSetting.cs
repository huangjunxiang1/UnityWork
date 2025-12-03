using System;
using System.Collections.Generic;
using System.Text;

public static partial class SSetting
{
    public static partial class CoreSetting
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
}
