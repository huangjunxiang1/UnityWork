using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    [AttributeUsage(AttributeTargets.Class)]
    public abstract class ConditionAttribute : SAttribute
    {
        public Type[] Types { get; protected set; }
    }
    public class AddComponentIfAll : ConditionAttribute
    {
        public AddComponentIfAll(params Type[] types) => this.Types = types;
    }
    public class AddComponentIfAny : ConditionAttribute
    {
        public AddComponentIfAny(params Type[] types) => this.Types = types;
    }
    public class AddComponentIfNone : ConditionAttribute
    {
        public AddComponentIfNone(params Type[] types) => this.Types = types;
    }
}
