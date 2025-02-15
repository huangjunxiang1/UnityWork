﻿using Game;
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
        [Sirenix.OdinInspector.ShowInInspector]
        public long rpc { get; set; }
        [Sirenix.OdinInspector.ShowInInspector]
        public long actorId { get; set; }
        [Sirenix.OdinInspector.ShowInInspector]
        public string error { get; set; }

        public abstract void Write(PBWriter writer);
        public abstract void Read(PBReader reader);
    }
}
