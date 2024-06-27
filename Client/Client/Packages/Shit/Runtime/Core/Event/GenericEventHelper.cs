using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public abstract class GenericEvent { }
    internal class GenericEventHelper
    {
        internal GenericEventHelper(List<MethodParseData> methods, EventSystem Event)
        {
            this.Event = Event;
            var gt = typeof(GenericEvent);
            for (int i = 0; i < methods.Count; i++)
            {
                if (gt.IsAssignableFrom(methods[i].mainKey))
                {
                    var m = methods[i];
#if DebugEnable
                    if (!m.mainKey.IsGenericType || m.mainKey.GetGenericArguments().Length != 1)
                        Loger.Error($"{m.mainKey} 泛型错误");
#endif
                    var element = m.mainKey.GetGenericArguments()[0];
                    var ctor = m.mainKey.GetConstructors().FirstOrDefault();
#if DebugEnable
                    if (ctor == null || ctor.GetParameters().Length != 1 || ctor.GetParameters()[0].ParameterType != element)
                        Loger.Error($"{m.mainKey} 没有匹配的构造函数");
#endif
                    Keys k = new() { a = m.mainKey.GetGenericTypeDefinition(), b = element };
                    if (map.ContainsKey(k)) continue;
                    map.Add(k, ctor);
                }
            }
        }

        EventSystem Event;
        Dictionary<Keys, ConstructorInfo> map = new();

        public void Invoke(Type genericType, object o, Type elementType = null)
        {
            elementType ??= o.GetType();
            Keys k = new() { a = genericType, b = elementType };
            if (map.TryGetValue(k, out var v))
            {
                var os = ArrayCache.Get<object>(1);
                os[0] = o;
                Event.RunEvent(v.Invoke(os));
            }
        }
        public void InvokeAndBaseType(Type genericType, object o)
        {
            var elementType = o.GetType();
            List<Type> ts = ObjectPool.Get<List<Type>>();

            var t = elementType;
            while (t != typeof(object))
            {
                ts.Add(t);
                t = t.BaseType;
            }
            var os = ArrayCache.Get<object>(1);
            for (int i = ts.Count - 1; i >= 0; i--)
            {
                Keys k = new() { a = genericType, b = ts[i] };
                if (map.TryGetValue(k, out var v))
                {
                    os[0] = o;
                    Event.RunEvent(v.Invoke(os));
                }
            }

            ts.Clear();
            ObjectPool.Return(ts);
        }


        struct Keys : IEquatable<Keys>
        {
            public Type a;
            public Type b;

            public bool Equals(Keys other) => this.a == other.a && this.b == other.b;
            public override int GetHashCode() => a.GetHashCode() ^ b.GetHashCode();
        }
    }
}
