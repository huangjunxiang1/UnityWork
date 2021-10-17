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
    public EventerContent(object sender, int value, object data)
    {
        this.Sender = sender;
        this.ValueInt = value;
        this.Data = data;
    }
    public EventerContent(int value, object data)
    {
        this.ValueInt = value;
        this.Data = data;
    }
    public EventerContent(int value) { this.ValueInt = value; }
    public EventerContent(object data) { this.Data = data; }

    /// <summary>
    /// 事件发起者
    /// </summary>
    public object Sender { get; }

    //事件数据
    public int ValueInt { get; }
    public object Data { get; }
}
