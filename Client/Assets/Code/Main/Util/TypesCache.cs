using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TypesCache
{
    public static Type[] Types { get; private set; }
    readonly static Dictionary<Type, ushort> _opCode = new Dictionary<Type, ushort>();
    readonly static Dictionary<ushort, Type> _opType = new Dictionary<ushort, Type>();
    readonly static Dictionary<Type, Type> _requestResponse = new Dictionary<Type, Type>();

    public static void InitTypes(Type[] types)
    {
        Types = types;
        int len = types.Length;
        for (int i = 0; i < len; i++)
        {
            Type type = types[i];
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
                {
                    Loger.Error($"not found responseType: {type}");
                    continue;
                }

                _requestResponse.Add(type, ((ResponseTypeAttribute)ras[0]).Type);
            }
        }
    }


    public static ushort GetOPCode(Type type)
    {
        _opCode.TryGetValue(type, out var code);
        return code;
    }
    public static Type GetOPType(ushort code)
    {
        _opType.TryGetValue(code, out var type);
        return type;
    }
    public static Type GetResponseType(Type request)
    {
        _requestResponse.TryGetValue(request, out var type);
        return type;
    }
}
