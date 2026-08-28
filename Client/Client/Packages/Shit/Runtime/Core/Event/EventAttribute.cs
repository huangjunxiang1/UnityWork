using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class EventAttribute : SAttribute
{
    public int SortOrder { get; set; }//消息调用顺序权值
    public bool Queue { get; set; }

    public int Type { get; set; }

    public Type EventType { get; set; }

    /// <summary>
    /// excute in other thread
    /// </summary>
    public bool Parallel { get; set; }

    public EventAttribute(int sortOrder = 0) => this.SortOrder = sortOrder;
    public EventAttribute(Type eventType, int sortOrder = 0)
    {
        this.EventType = eventType;
        this.SortOrder = sortOrder;
    }

    public readonly static EventAttribute Default = new();
}