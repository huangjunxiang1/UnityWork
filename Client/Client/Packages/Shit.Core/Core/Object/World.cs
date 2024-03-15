using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class World : STree
    {
        public World(int id) : base(null, id)
        {
            worlds[id] = this;
            this.System = new(this);
            this.Timer = new();
        }

        static ConcurrentDictionary<int, World> worlds = new();

        public int id { get; private set; }
        public EventSystem Event { get; private set; } = new();
        public ObjectManager ObjectManager { get; private set; } = new();
        public STimer Timer { get; private set; }

        internal SSystem System;

        public static World GetWorld(int id)
        {
            worlds.TryGetValue(id, out var v);
            return v;
        }
        public static bool TryGetWorld(int id, out World world)
        {
            return worlds.TryGetValue(id, out world);
        }

        public void Update(float time)
        {
            ObjectManager.Update();
            System.Update();
            Timer.Update(time);
        }
        public void AfterUpdate()
        {
            System.AfterUpdate();
            Event.AfterUpdate();
            Timer.AfterUpdate();
        }
    }
}
