using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public partial class SGameObject : STree
    {
#if Server
        public SGameObject()
        {
            this.KV = this.AddComponent<KVComponent>();
            this.Transform = this.AddComponent<TransformComponent>();
        }
#endif

        public KVComponent KV { get; private set; }
        public TransformComponent Transform { get; private set; }
    }
}
