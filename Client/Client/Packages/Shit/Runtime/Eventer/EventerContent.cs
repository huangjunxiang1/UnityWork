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
    public EventerContent() { }
    public EventerContent(object sender) 
    { 
        this.Sender = sender;
    }
    public EventerContent(object sender, int value, object data)
    {
        this.Sender = sender;
        this.Value = value;
        this.Data = data;
    }
    public EventerContent(object sender, int value)
    {
        this.Sender = sender;
        this.Value = value;
    }
    public EventerContent(object sender, object data)
    {
        this.Sender = sender;
        this.Data = data; 
    }

    /// <summary>
    /// 事件发起者
    /// </summary>
    public object Sender { get; }

    //事件数据
    public int Value { get; }
    public object Data { get; }
}
