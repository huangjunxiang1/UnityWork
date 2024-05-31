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
        public GameObjectComponent GameObject { get; private set; }
        public PlayingComponent Playing { get; private set; }

        [Event(-1)]
        static void Awake(Awake<SGameObject> t)
        {
            t.t.GameObject = t.t.AddComponent<GameObjectComponent>();
            t.t.Playing = t.t.AddComponent<PlayingComponent>();
        }
    }
}
