using System;
using System.Collections.Generic;

namespace Core
{
    public class SCore<T> where T : SCore<T>
    {
        public static ThreadSynchronizationContext ThreadSync { get; private set; }
        public static EventSystem Event { get; private set; }
        public static STimer Timer { get; private set; } = new();
        public static ObjectManager ObjectManager { get; private set; } = new();
        public static Types Types { get; private set; } = new();
        public static float DeltaTime { get; private set; }
        internal static SSystem System;
        internal static Action onClose;

        protected static void Initializes(List<Type> types)
        {
            Checker.Check(types);

            SValueTask.DelayHandle -= delayHandle;
            SValueTask.DelayHandle += delayHandle;

            ThreadSync = ThreadSynchronizationContext.GetOrCreate(Environment.CurrentManagedThreadId);
            Event = new();
            System = new();

            var methods = Types.Parse(types);
            Event.Load(methods);
            Timer.Load(methods);
            System.Load(methods);
        }
        public static void BeforeUpdate(float time)
        {
            DeltaTime = time;
            ThreadSync.Update();
            System.beforeUpdate();
        }
        public static void Update()
        {
            System.update();
            Timer.Update(DeltaTime);
        }
        public static void LateUpdate()
        {
            System.lateUpdate();
            Event.LateUpdate();
            ObjectManager.LateUpdate();
        }
        protected static void UnInitializes()
        {
            SValueTask.DelayHandle -= delayHandle;
        }
        static void delayHandle(int ms, SValueTask task)
        {
            Timer.Add(ms / 1000f, 1, task.TrySetResult);
        }
    }
}
