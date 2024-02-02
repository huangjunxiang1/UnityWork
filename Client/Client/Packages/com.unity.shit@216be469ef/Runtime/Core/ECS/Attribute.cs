using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SSystemAttribute : SAttribute
    {
        public int SortOrder { get; }
        public SSystemAttribute(int sortOrder) => SortOrder = sortOrder;
    }
    public class AwakeAttribute : SSystemAttribute
    {
        public AwakeAttribute(int sortOrder = 0) : base(sortOrder) { }
    }
    public class DisposeAttribute : SSystemAttribute
    {
        public DisposeAttribute(int sortOrder = 0) : base(sortOrder) { }
    }
    public class ChangeAttribute : SSystemAttribute
    {
        public ChangeAttribute(int sortOrder = 0) : base(sortOrder) { }
    }
    public class MoveAttribute : SSystemAttribute
    {
        public MoveAttribute(int sortOrder = 0) : base(sortOrder) { }
    }

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
