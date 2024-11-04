using System.Diagnostics;

namespace Sirenix.OdinInspector
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    [Conditional("DEBUG")]
    public class ShowInInspector : Attribute
    {

    }
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    [Conditional("DEBUG")]
    public class PropertyOrderAttribute : Attribute
    {
        public float Order;

        public PropertyOrderAttribute(float order = 0) { Order = order; }
    }
}