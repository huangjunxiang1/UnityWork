using System;
using System.Collections.Generic;

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

        public void BeforeUpdate(float time)
        {
            this.DeltaTime = time;
            Thread.Update();
            System.beforeUpdate();
        }
        public void Update()
        {
            System.update();
            Timer.Update(this.DeltaTime);
        }
        public void LateUpdate()
        {
            System.lateUpdate();
            Event.LateUpdate();
            ObjectManager.LateUpdate();
        }
        public void Dispose()
        {
            Worlds.Remove(this);
            Close?.Invoke();
            Thread.Post(s => Root.Dispose());
        }
    }
}
