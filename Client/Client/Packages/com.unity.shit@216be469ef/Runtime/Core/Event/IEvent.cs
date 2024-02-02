using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IEvent
{
    public bool EventEnable { get; set; }

    void RigisterEvent() => GameM.Event.RigisteEvent(this);
    void RigisterRPCEvent(long rpc)
    {
        if (rpc == 0)
        {
            Loger.Error($"rpc=0");
            return;
        }
        GameM.Event.RigisteRPCEvent(rpc, this);
    }
    void RemoveAllEvent(long rpc)
    {
        GameM.Event.RemoveEvent(this);
        if (rpc != 0)
            GameM.Event.RemoveRPCEvent(rpc, this);
    }
    void RemoveEvent() => GameM.Event.RemoveEvent(this);
    void RemoveRPCEvent(long rpc) => GameM.Event.RemoveRPCEvent(rpc, this);
}
