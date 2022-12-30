using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public static class Reflection
    {
        static Reflection()
        {
            int index = 0;
            for (int i = 0; i < Types.MainTypes.Length; i++)
            {
                if (typeof(Attribute).IsAssignableFrom(Types.MainTypes[i]))
                {
                    if (index >= 64)
                    {
                        Loger.Error("属性个数超过64限制");
                        break;
                    }
                    attributeMask[Types.MainTypes[i]] = ++index;
                }
            }
        }

        static Dictionary<Type, ulong> typeMaskV = new();
        static Dictionary<Type, int> attributeMask = new();
        static Dictionary<Type, object> typeAttributeMap = new();

        public static bool HasDefineAttribute(Type t,Type attribute)
        {
            if (!attributeMask.TryGetValue(attribute, out int v))
            {
                Loger.Error($"属性类型->{attribute} 未在定义里面");
                return false;
            }
            if (!typeMaskV.TryGetValue(t, out ulong vs))
            {
                ulong mv = 0;
                foreach (var item in attributeMask)
                {
#if ILRuntime
                    if (t is ILRuntime.Reflection.ILRuntimeType ilType)
                    {
                        Type temp = t;
                        while (temp != null)
                        {
                            if (temp.IsDefined(item.Key, true))
                            {
                                mv |= 1ul << item.Value;
                                break;
                            }
                            temp = temp.BaseType;
                        }
                    }
                    else
                    {
                        if (t.IsDefined(item.Key, true))
                            mv |= 1ul << item.Value;
                    }
#else
                    if (t.IsDefined(item.Key, true))
                        mv |= 1ul << item.Value;
#endif
                }
                typeMaskV[t] = vs = mv;
            }
            return (vs & (1ul << v)) != 0;
        }

        public static object GetAttribute(Type t, Type attribute)
        {
            if (!typeAttributeMap.TryGetValue(t, out object v))
            {
                var os = t.GetCustomAttributes(attribute, true);
                if (os.Length == 0)
                    typeAttributeMap[t] = null;
                else
                    typeAttributeMap[t] = v = os[0];
            }
            return v;
        }
    }
}
