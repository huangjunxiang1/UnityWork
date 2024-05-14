using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;

namespace Core
{
    public sealed class World
    {
        public World(List<Type> types,string name)
        {
            this.Name = string.IsNullOrEmpty(name) ? "Unknown" : name;
            Thread = ThreadSynchronizationContext.GetOrCreate(Environment.CurrentManagedThreadId);

            Checker.Check(types);

            Event = new(this);
            System = new(this);

            var methods = Types.Parse(types);
            Event.Load(methods);
            Timer.Load(methods);
            System.Load(methods);

            Worlds.Add(this);
            this.Root.World = this;
        }
        public string Name { get; }
        public ThreadSynchronizationContext Thread { get; private set; }
        public EventSystem Event { get; private set; } 
        public STimer Timer { get; private set; } = new();
        public ObjectManager ObjectManager { get; private set; } = new();
        public Types Types { get; private set; } = new();
        public float DeltaTime { get; private set; }
        public STree Root { get; private set; } = new();

        internal SSystem System;
        internal static List<World> Worlds = new();
        internal static Action Close;

        public void Update(float time)
        {
            this.DeltaTime = time;
            Thread.Update();

            System.update();
            Timer.Update(time);

            System.AfterUpdate();
            Timer.AfterUpdate();
            Event.AfterUpdate();
            ObjectManager.AfterUpdate();
        }
        public void Dispose()
        {
            Worlds.Remove(this);
            Close?.Invoke();
            Thread.Post(Root.Dispose);
        }
    }
}
