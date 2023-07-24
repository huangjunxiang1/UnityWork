using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class EventAttribute : Attribute
{
	public int SortOrder { get; }//消息调用顺序权值
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class RPCEventAttribute : Attribute
{
	public int SortOrder { get; }//消息调用顺序权值
}