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

        public AttributeComponent Attribute { get; private set; }
        public TransformComponent Transform { get; private set; }

        [Event]
        static void awake(Awake<SGameObject> t)
        {
            t.t.Attribute = t.t.AddComponent<AttributeComponent>();
            t.t.Transform = t.t.AddComponent<TransformComponent>();
        }
    }
}
