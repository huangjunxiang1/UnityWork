using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
public class EventAttribute : SAttribute
{
    public int SortOrder { get; }//消息调用顺序权值
    public bool RPC { get; set; }
    public bool Queue { get; set; }

    public int Type { get; set; }

    public EventAttribute(int sortOrder = 0) => this.SortOrder = sortOrder;

    public virtual bool Test(int type) => this.Type == type;

    public readonly static EventAttribute Default = new();
}
public class TimerAttribute : SAttribute
{
    public float delay { get; }
    public int count { get; }
    public TimerAttribute(float delay, int count)
    {
        this.delay = delay;
        this.count = count;
    }
}