using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

public static class Types
{
    public static Type[] MainTypes { get; private set; }//主工程类型
    public static Type[] HotTypes { get; private set; }//热更工程类型
    public static Type[] AllTypes { get; private set; }//所有类型

    readonly static Dictionary<Type, uint> _cmdCode = new();
    readonly static Dictionary<uint, Type> _cmdType = new();
    readonly static Dictionary<Type, Type> _requestResponse = new();

    static Dictionary<Type, ulong> typeMaskV = new();
    static Dictionary<Type, int> attributeMask = new();
    static Dictionary<Type, object[]> typeAttributeMap = new();
    static Dictionary<Type, Type[]> assignableTypesMap = new();

    public static void InitTypes(Type[] mtypes, Type[] htypes)
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
        }
        len = AllTypes.Length;
        for (int i = 0; i < len; i++)
        {
            Type type = AllTypes[i];
            if (!typeof(PB.IPBMessage).IsAssignableFrom(type))
                continue;
            var mas = type.GetCustomAttributes(typeof(MessageAttribute), false);
            if (mas == null || mas.Length <= 0)
                continue;
            var att = (MessageAttribute)mas[0];
            uint cmd = att.cmd;
            _cmdCode.Add(type, cmd);
            _cmdType.Add(cmd, type);
            if (att.ResponseType != null)
                _requestResponse[type] = att.ResponseType;
        }
    }


    public static uint GetCMDCode(Type type)
    {
        if (!_cmdCode.TryGetValue(type, out var code))
            Loger.Error("消息没有cmdCode type:" + type.FullName);
        return code;
    }
    public static Type GetCMDType(uint cmd)
    {
        if (!_cmdType.TryGetValue(cmd, out var type))
            Loger.Error($"cmd没有类型 cmd: main={(ushort)cmd} sub={cmd >> 16}");
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

    public static object[] GetAttributes(Type t)
    {
        if (!typeAttributeMap.TryGetValue(t, out var lst))
            typeAttributeMap[t] = lst = t.GetCustomAttributes(true);
        return lst;
    }

    public static object GetAttribute(Type t, Type attribute)
    {
        var arr = GetAttributes(t);
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i].GetType() == attribute)
                return arr[i];
        }
        return null;
    }

    public static Type[] GetAllAssignableTypes(Type t)
    {
        if (!assignableTypesMap.TryGetValue(t, out var arr))
        {
            var lst = new List<Type>();
            int len = AllTypes.Length;
            for (int i = 0; i < len; i++)
            {
                Type type = AllTypes[i];
                if (!t.IsAssignableFrom(type) || t == type)
                    continue;
                lst.Add(type);
            }
            assignableTypesMap[t] = arr = lst.ToArray();
        }
        return arr;
    }
}
