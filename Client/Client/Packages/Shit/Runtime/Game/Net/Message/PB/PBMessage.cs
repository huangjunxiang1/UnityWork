using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PB
{
    public abstract class PBMessage : IMessage
    {
        [Newtonsoft.Json.JsonIgnore]
        public long rpc { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public long actorId { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public string error { get; set; }

        public abstract void Write(PBWriter writer);
        public abstract void Read(PBReader reader);
        public abstract void Merge(PBMessage message);
    }
}
