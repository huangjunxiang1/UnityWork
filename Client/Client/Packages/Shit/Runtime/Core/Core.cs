using System;
using System.Collections.Generic;
using System.Threading;

namespace Core
{
    public class SCore<T> where T : SCore<T>
    {
        public static SynchronizationContext ThreadSync { get; private set; }
        public static EventSystem Event { get; private set; }
        public static STimer Timer { get; private set; } = new();
        public static ObjectManager ObjectManager { get; private set; } = new();
        public static Types Types { get; private set; } = new();
        public static float DeltaTime { get; private set; }
        internal static SSystem System;
        internal static Action onClose;

        protected static void InitializeCore(List<Type> types)
        {
            Checker.Check(types);

            ThreadSync = SynchronizationContext.Current;
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
        protected static void UnInitialize()
        {
           
        }
    }
}
