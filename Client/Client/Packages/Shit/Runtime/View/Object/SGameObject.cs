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
#if !Server
        public SGameObject(SGameObjectType type = SGameObjectType.LogicRoot)
        {
            this.KV = this.AddComponent<KVComponent>();
            this.Transform = this.AddComponent<TransformComponent>();
            this.GameObject = this.AddComponent(new GameObjectComponent(type));
            this.Playing = this.AddComponent<PlayingComponent>();
        }
#endif

        public GameObjectComponent GameObject { get; private set; }
        public PlayingComponent Playing { get; private set; }
    }
}
