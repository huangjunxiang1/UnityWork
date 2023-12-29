using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SSystemAttribute : SAttribute { }
    public class AwakeAttribute : SSystemAttribute { }
    public class DisposeAttribute : SSystemAttribute { }
    public class UpdateAttribute : SSystemAttribute { }
}
