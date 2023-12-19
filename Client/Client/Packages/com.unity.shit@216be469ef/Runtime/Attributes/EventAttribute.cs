using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class EventAttribute : Attribute
{
	public int SortOrder { get; }//消息调用顺序权值

	public EventAttribute() { }
	public EventAttribute(int sortOrder)
    {
        SortOrder = sortOrder;
    }
}

public class QueueEventAttribute : EventAttribute { }

public class RPCEventAttribute : EventAttribute
{
	public RPCEventAttribute() : base() { }
	public RPCEventAttribute(int sortOrder) : base(sortOrder) { }
}

public class QueueRPCEventAttribute : EventAttribute { }
