using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TypesCache
{
    public static Type[] MainTypes { get; private set; }//主工程类型
    public static Type[] HotTypes { get; private set; }//热更工程类型
    public static Type[] AllTypes { get; private set; }//所有类型

    readonly static Dictionary<Type, ushort> _opCode = new();
    readonly static Dictionary<ushort, Type> _opType = new();
    readonly static Dictionary<Type, Type> _requestResponse = new();

    public static void InitTypes(Type[] mtypes,Type[] htypes)
    {
        AllTypes = new Type[mtypes.Length + htypes.Length];
        mtypes.CopyTo(AllTypes, 0);
        htypes.CopyTo(AllTypes, mtypes.Length);

        MainTypes = mtypes;
        HotTypes = htypes;
        int len = mtypes.Length;
        for (int i = 0; i < len; i++)
        {
            Type type = mtypes[i];
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
}
