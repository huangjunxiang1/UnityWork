using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class SGameObject : SUnityObject
    {
        public SGameObject(long rpc = 0, SGameObjectType style = SGameObjectType.LogicRoot) : base(rpc)
        {
            this.Attribute = this.AddComponent<AttributeComponent>();
            this.GameObject = this.AddComponent(new GameObjectComponent(style));
            this.Transform = this.AddComponent<TransformComponent>();
            this.Playing = this.AddComponent<PlayingComponent>();
            GameM.World.AddChild(this);
        }

        public AttributeComponent Attribute { get; private set; }
        public GameObjectComponent GameObject { get; private set; }
        public TransformComponent Transform { get; private set; }
        public PlayingComponent Playing { get; private set; }
    }
}
