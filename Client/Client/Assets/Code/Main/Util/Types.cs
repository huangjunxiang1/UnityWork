using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Types
{
    public static Type[] MainTypes { get; private set; }//主工程类型
    public static Type[] HotTypes { get; private set; }//热更工程类型
    public static Type[] AllTypes { get; private set; }//所有类型

    readonly static Dictionary<Type, ushort> _opCode = new();
    readonly static Dictionary<ushort, Type> _opType = new();
    readonly static Dictionary<Type, Type> _requestResponse = new();

    static Dictionary<Type, ulong> typeMaskV = new();
    static Dictionary<Type, int> attributeMask = new();
    static Dictionary<Type, object> typeAttributeMap = new();

    public static void InitTypes(Type[] mtypes,Type[] htypes)
    {
        AllTypes = new Type[mtypes.Length + htypes.Length];
        mtypes.CopyTo(AllTypes, 0);
        htypes.CopyTo(AllTypes, mtypes.Length);

        MainTypes = mtypes;
        HotTypes = htypes;
        int len = mtypes.Length;
        int index = 0;
        for (int i = 0; i < len; i++)
        {
            Type type = mtypes[i];

            if (typeof(Attribute).IsAssignableFrom(type))
            {
                if (index >= 64)
                    Loger.Error("属性个数超过64限制");
                else
                    attributeMask[type] = ++index;
            }

            if (typeof(IMessage).IsAssignableFrom(type))
            {
                var mas = type.GetCustomAttributes(typeof(MessageAttribute), false);
                if (mas == null || mas.Length <= 0)
                    continue;
                ushort opCode = ((MessageAttribute)mas[0]).Opcode;
                _opCode[type] = opCode;
                _opType[opCode] = type;

                // 检查request response
                if (typeof(IRequest).IsAssignableFrom(type))
                {
                    if (typeof(IActorLocationMessage).IsAssignableFrom(type))
                    {
                        _requestResponse.Add(type, typeof(ActorResponse));
                        continue;
                    }

                    var ras = type.GetCustomAttributes(typeof(ResponseTypeAttribute), false);
                    if (ras.Length == 0)
                        continue;

                    _requestResponse.Add(type, ((ResponseTypeAttribute)ras[0]).Type);
                }
            }
        }
        len = htypes.Length;
        for (int i = 0; i < len; i++)
        {
            Type type = htypes[i];
            if (!typeof(IMessage).IsAssignableFrom(type))
                continue;
            var mas = type.GetCustomAttributes(typeof(MessageAttribute), false);
            if (mas == null || mas.Length <= 0)
                continue;
            ushort opCode = ((MessageAttribute)mas[0]).Opcode;
            _opCode[type] = opCode;
            _opType[opCode] = type;

            // 检查request response
            if (typeof(IRequest).IsAssignableFrom(type))
            {
                if (typeof(IActorLocationMessage).IsAssignableFrom(type))
                {
                    _requestResponse.Add(type, typeof(ActorResponse));
                    continue;
                }

                var ras = type.GetCustomAttributes(typeof(ResponseTypeAttribute), false);
                if (ras.Length == 0)
                    continue;

                _requestResponse.Add(type, ((ResponseTypeAttribute)ras[0]).Type);
            }
        }
    }


    public static ushort GetOPCode(Type type)
    {
        if (!_opCode.TryGetValue(type, out var code))
            Loger.Error("消息没有opCode type:" + type.FullName);
        return code;
    }
    public static Type GetOPType(ushort code)
    {
        _opType.TryGetValue(code, out var type);
        return type;
    }
    public static Type GetResponseType(Type request)
    {
        if (!_requestResponse.TryGetValue(request, out var type))
            Loger.Error("request没有Response  requestType:" + type.FullName);
        return type;
    }

    public static bool HasDefineAttribute(Type t, Type attribute)
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
#endif
                {
                    if (t.IsDefined(item.Key, true))
                        mv |= 1ul << item.Value;
                }
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
