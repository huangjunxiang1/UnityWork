using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IEvent : IDispose
{
    public long rpc { get; }
    public bool EventEnable { get; set; }
    void AcceptedEvent();
}
