using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class ConstDefL
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
