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
        public SGameObject(SGameObjectType type) : this()
        {
            this.type = type;
        }

        SGameObjectType type = SGameObjectType.LogicRoot;

        public GameObjectComponent GameObject { get; private set; }
        public PlayingComponent Playing { get; private set; }

        [Event(-1)]
        static void Awake(Awake<SGameObject> t)
        {
            t.t.GameObject = t.t.AddComponent(new GameObjectComponent(t.t.type));
            t.t.Playing = t.t.AddComponent<PlayingComponent>();
        }
    }
}
