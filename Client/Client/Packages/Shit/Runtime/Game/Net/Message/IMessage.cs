using PB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public interface IMessage : IData
    {
        public long rpc { get; set; }
        public long actorId { get; set; }
        public string error { get; set; }
        public abstract void Write(PBWriter writer);
        public abstract void Read(PBReader reader);
    }
}