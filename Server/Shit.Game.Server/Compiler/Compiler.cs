using System.Diagnostics;

namespace Sirenix.OdinInspector
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    [Conditional("DEBUG")]
    public class ShowInInspector : Attribute
    {

    }
}