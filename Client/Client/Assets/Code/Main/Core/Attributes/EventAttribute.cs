using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class EventAttribute : Attribute
{
	public int EventID { get; }//事件ID
	public int SortOrder { get; }//消息调用顺序权值

	public EventAttribute(int eventID, int sortOrder = 0)
	{
		this.EventID = eventID;
		this.SortOrder = sortOrder;
	}
}