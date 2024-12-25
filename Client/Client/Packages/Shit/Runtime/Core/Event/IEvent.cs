using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IEvent : IDispose
{
    public long ActorId { get; init; }
    public long gid { get; }
    public bool EventEnable { get; set; }
    void AcceptedEvent();
}
