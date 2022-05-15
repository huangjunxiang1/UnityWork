using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class EventAttribute : Attribute
{
	public int EventID { get; }//事件ID
	public int SortOrder { get; }//消息调用顺序权值

	public EventAttribute(int eventID)
	{
		this.EventID = eventID;
	}
	public EventAttribute(int eventID, int sortOrder)
	{
		this.EventID = eventID;
		this.SortOrder = sortOrder;
	}
}