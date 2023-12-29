using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class SComponent
    {
        public SObject Entity { get; internal set; }
        public bool Disposed { get; private set; }
        public void Dispose()
        {
            this.Disposed = true;
            long cid = Entity.cid;
            Entity.UnPack(this);
            SSystem.UnRigisteComponent(this);
            SGameM.Event.RemoveEvent(this);
            if (cid > 0)
                SGameM.Event.RemoveRPCEvent(Entity.cid, this);
            SSystem.Run<DisposeAttribute>(this);
        }
    }
}
