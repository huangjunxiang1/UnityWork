using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class EventHandler
{
    public EventHandler() { }
    public EventHandler(object sender)
    {
        this.Sender = sender;
    }
    public EventHandler(object sender, int value, object data)
    {
        this.Sender = sender;
        this.Value = value;
        this.Data = data;
    }
    public EventHandler(object sender, int value)
    {
        this.Sender = sender;
        this.Value = value;
    }
    public EventHandler(object sender, object data)
    {
        this.Sender = sender;
        this.Data = data;
    }

    /// <summary>
    /// 事件发起者
    /// </summary>
    public object Sender { get; }
    public bool isBreak { get; private set; }
    public object Data { get; private set; }
    public long Value { get; private set; }

    public void BreakEvent()
    {
        isBreak = true;
    }
    public void SetData(object data)
    {
        Data = data;
    }
    public void SetValue(long v)
    {
        Value = v;
    }
    internal void Reset()
    {
        this.isBreak = false;
        this.Data = null;
        this.Value = 0;
    }
}
