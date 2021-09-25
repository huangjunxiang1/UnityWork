using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 事件内容
/// </summary>
public class EventerContent
{
    public EventerContent(object sender, int value, object data)
    {
        this.Sender = sender;
        this.Value = value;
        this.Data = data;
    }
    public object Sender { get; }
    public int Value { get; }
    public object Data { get; }
}
