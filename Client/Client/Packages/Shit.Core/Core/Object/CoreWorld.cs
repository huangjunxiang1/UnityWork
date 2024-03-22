using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Core
{
    public class CoreWorld<T> : CoreWorld where T : CoreWorld<T>, new()
    {
        public static T World { get; protected set; }
        public static STree Root { get; protected set; }

        static bool close = false;

        public override void Dispose()
        {
            base.Dispose();
            Root.Dispose();
        }
        public static void Close()
        {
            if (World == null|| close) return;
            World.Thread.Post(World.Dispose);
            close = true;
        }
    }
    public abstract class CoreWorld
    {
        public ThreadSynchronizationContext Thread { get; private set; }
        public EventSystem Event { get; private set; } = new();
        public STimer Timer { get; private set; } = new();
        public Types Types { get; private set; } = new();
        public float DeltaTime { get; private set; }

        internal SSystem System;

        public List<MethodParseData> Load(List<Assembly> assemblies)
        {
            List<Type> types = new(assemblies.SelectMany(t => t.GetTypes()));
            return this.Load(types);
        }
        public List<MethodParseData> Load(List<Type> types)
        {
            Thread = ThreadSynchronizationContext.GetOrCreate(global::System.Threading.Thread.CurrentThread.ManagedThreadId);

            STimer.Check(types);
            EventSystem.Check(types);

            System = new(this);

            var methods = Types.Parse(types);
            Event.Load(methods);
            Timer.Load(methods);
            System.Load(methods);
            return methods;
        }
        public void Update(float time)
        {
            this.DeltaTime = time;
            Thread.Update();

            System.Update();
            Timer.Update(time);

            System.AfterUpdate();
            Timer.AfterUpdate();
            Event.AfterUpdate();
        }
        public virtual void Dispose()
        {
            
        }
    }
}
