using Main;
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

    public EventAttribute() { }
    public EventAttribute(int sortOrder)
    {
        SortOrder = sortOrder;
    }
}