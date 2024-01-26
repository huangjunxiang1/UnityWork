using Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

public static class Types
{
    public static List<Type> AllTypes { get; } = new(1000);

    readonly static Dictionary<Type, uint> _typeCmd = new();
    readonly static Dictionary<uint, Type> _cmdType = new();
    readonly static Dictionary<Type, Type> _requestResponse = new();

    static Dictionary<Type, Type[]> assignableTypesMap = new();
    static Dictionary<Type, FieldInfo> StateMachineThisFieldMap = new();
    static Dictionary<Type, bool> _asyncNeedCancel = new();
    static Dictionary<Type, MethodParseData[]> methodAttributeCache = new();

    public static void RigisterTypes(IEnumerable<Type> types)
    {
        AllTypes.AddRange(types);
    }
    internal static List<MethodParseData> Parse()
    {
        int len = AllTypes.Count;
        List<MethodParseData> staticMethods = new(1000);
        for (int i = 0; i < len; i++)
        {
            Type type = AllTypes[i];

            if (typeof(PB.PBMessage).IsAssignableFrom(type))
            {
                var m = type.GetCustomAttribute<MessageAttribute>();
                if (m != null)
                {
                    uint cmd = m.cmd;
                    _typeCmd.Add(type, cmd);
                    _cmdType.Add(cmd, type);
                    if (m.ResponseType != null)
                        _requestResponse[type] = m.ResponseType;
                }
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            for (int j = 0; j < methods.Length; j++)
            {
                var method = methods[j];
                var asm = method.GetCustomAttribute<AsyncStateMachineAttribute>();
                if (asm != null)
                    _asyncNeedCancel[asm.StateMachineType] = method.GetCustomAttribute<AsynAutoCancelIfCallerDisposedAttribute>() != null;

                if (method.IsStatic)
                {
                    foreach (var item in method.GetCustomAttributes<SAttribute>())
                        staticMethods.Add(new MethodParseData(method, item));
                }
            }

            var propertys = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            for (int j = 0; j < propertys.Length; j++)
            {
                var property = propertys[j];
                if (property.SetMethod == null) continue;
                foreach (var item in property.GetCustomAttributes<SAttribute>())
                    staticMethods.Add(new MethodParseData(property, item));
            }
        }
        return staticMethods;
    }

    public static uint GetCMDCode(Type type)
    {
        if (!_typeCmd.TryGetValue(type, out var code))
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
    public static Type[] GetAllAssignableTypes(Type t)
    {
        if (!assignableTypesMap.TryGetValue(t, out var arr))
        {
            var lst = ObjectPool.Get<List<Type>>();
            int len = AllTypes.Count;
            for (int i = 0; i < len; i++)
            {
                Type type = AllTypes[i];
                if (!t.IsAssignableFrom(type) || t == type)
                    continue;
                lst.Add(type);
            }
            assignableTypesMap[t] = arr = lst.Count == 0 ? Array.Empty<Type>() : lst.ToArray();
            lst.Clear();
            ObjectPool.Return(lst);
        }
        return arr;
    }
    public static FieldInfo GetStateMachineThisField(Type t)
    {
        if (!StateMachineThisFieldMap.TryGetValue(t, out var value))
            StateMachineThisFieldMap[t] = value = t.GetField("<>4__this", BindingFlags.Public | BindingFlags.Instance);

        return value;
    }
    public static bool AsyncInvokeIsNeedAutoCancel(Type stateMachineType)
    {
        return _asyncNeedCancel.TryGetValue(stateMachineType, out bool v) && v;
    }

    internal static MethodParseData[] GetInstanceMethodsAttribute(Type type)
    {
        if (!methodAttributeCache.TryGetValue(type, out var array))
        {
            var lst = ObjectPool.Get<List<MethodParseData>>();

            var methods = ObjectPool.Get<List<MethodInfo>>();
            var tmp = type;
            methods.AddRange(tmp.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            while (tmp.BaseType != null)
            {
                tmp = tmp.BaseType;
                var ms = tmp.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                for (int i = 0; i < ms.Length; i++)
                {
                    if (ms[i].IsPrivate)
                        methods.Add(ms[i]);
                }
            }
            for (int i = 0; i < methods.Count; i++)
            {
                foreach (var item in methods[i].GetCustomAttributes<SAttribute>())
                    lst.Add(new MethodParseData(methods[i], item));
            }
            methods.Clear();
            ObjectPool.Return(methods);

            var propertys = ObjectPool.Get<List<PropertyInfo>>();
            tmp = type;
            propertys.AddRange(tmp.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            while (tmp.BaseType != null)
            {
                tmp = tmp.BaseType;
                var ps = tmp.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                for (int i = 0; i < ps.Length; i++)
                {
                    var p = ps[i];
                    if (p.SetMethod != null && p.SetMethod.IsPrivate)
                        propertys.Add(p);
                }
            }
            for (int i = 0; i < propertys.Count; i++)
            {
                if (propertys[i].SetMethod == null) continue;
                foreach (var item in propertys[i].GetCustomAttributes<SAttribute>())
                    lst.Add(new MethodParseData(propertys[i], item));
            }
            propertys.Clear();
            ObjectPool.Return(propertys);

            methodAttributeCache[type] = array = lst.Count == 0 ? Array.Empty<MethodParseData>() : lst.ToArray();
            lst.Clear();
            ObjectPool.Return(lst);
        }
        return array;
    }
}

class MethodParseData
{
    public MethodParseData(MethodInfo method, Attribute attribute)
    {
        this.method = method;
        this.attribute = attribute;
        this.parameters = method.GetParameters();
    }
    public MethodParseData(PropertyInfo property, Attribute attribute)
    {
        this.method = property.SetMethod;
        this.attribute = attribute;
        this.property = property;
    }

    public MethodInfo method;
    public Attribute attribute;

    public ParameterInfo[] parameters;
    public PropertyInfo property;
}
