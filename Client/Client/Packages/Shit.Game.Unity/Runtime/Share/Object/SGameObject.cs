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
        public SGameObject(long rpc = 0) : base(rpc)
        {
        }

        public KVComponent KV { get; private set; }
        public TransformComponent Transform { get; private set; }
        public MoveComponent Move { get; private set; }
        public MoveToComponent MoveTo { get; private set; }

        [Event]
        static void awake(Awake<SGameObject> t)
        {
            t.t.KV = t.t.AddComponent<KVComponent>();
            t.t.Transform = t.t.AddComponent<TransformComponent>();
            t.t.Move = t.t.AddComponent<MoveComponent>();
            t.t.MoveTo = t.t.AddComponent<MoveToComponent>();
        }
    }
}
