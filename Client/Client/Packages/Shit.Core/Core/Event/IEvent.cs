using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IEvent : IDispose
{
    public bool EventEnable { get; set; }
    void AcceptEventHandler(bool isInvokeMethod) { }
}
