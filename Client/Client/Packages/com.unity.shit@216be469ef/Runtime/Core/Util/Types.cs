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
    readonly static Dictionary<Type, Dictionary<Type, Type>> _genericMap = new();

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

            var fs = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            for (int j = 0; j < fs.Length; j++)
            {
                var f = fs[j];
                foreach (var item in f.GetCustomAttributes<SAttribute>())
                    staticMethods.Add(new MethodParseData(f, item));
            }

            var ps = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            for (int j = 0; j < ps.Length; j++)
            {
                var p = ps[j];
                if (p.SetMethod == null) continue;
                foreach (var item in p.GetCustomAttributes<SAttribute>())
                    staticMethods.Add(new MethodParseData(p, item));
            }

            var ms = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            for (int j = 0; j < ms.Length; j++)
            {
                var m = ms[j];
                var asm = m.GetCustomAttribute<AsyncStateMachineAttribute>();
                if (asm != null)
                    _asyncNeedCancel[asm.StateMachineType] = m.GetCustomAttribute<AsynCancelIfCallerDisposedAttribute>() != null;

                if (m.IsStatic)
                {
                    foreach (var item in m.GetCustomAttributes<SAttribute>())
                        staticMethods.Add(new MethodParseData(m, item));
                }
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
            var lst = ObjectPool<List<Type>>.Get();
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
            ObjectPool<List<Type>>.Return(lst);
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
            var lst = ObjectPool<List<MethodParseData>>.Get();

            var fields = ObjectPool<List<FieldInfo>>.Get();
            var tmp = type;
            fields.AddRange(tmp.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            while (tmp.BaseType != null)
            {
                tmp = tmp.BaseType;
                var fs = tmp.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                for (int i = 0; i < fs.Length; i++)
                {
                    if (fs[i].IsPrivate)
                        fields.Add(fs[i]);
                }
            }
            for (int i = 0; i < fields.Count; i++)
            {
                foreach (var item in fields[i].GetCustomAttributes<SAttribute>())
                    lst.Add(new MethodParseData(fields[i], item));
            }
            fields.Clear();
            ObjectPool<List<FieldInfo>>.Return(fields);

            var propertys = ObjectPool<List<PropertyInfo>>.Get();
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
            ObjectPool<List<PropertyInfo>>.Return(propertys);

            var methods = ObjectPool<List<MethodInfo>>.Get();
            tmp = type;
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
            ObjectPool<List<MethodInfo>>.Return(methods);

            methodAttributeCache[type] = array = lst.Count == 0 ? Array.Empty<MethodParseData>() : lst.ToArray();
            lst.Clear();
            ObjectPool<List<MethodParseData>>.Return(lst);
        }
        return array;
    }
    internal static Type GetGenericType(Type generic, Type element)
    {
        if (!_genericMap.TryGetValue(generic, out var value))
            value = _genericMap[generic] = new();
        if (!value.TryGetValue(element, out var value2))
        {
            var ps = ArrayCache<Type>.Get(1);
            ps[0] = element;
            value2 = value[element] = generic.MakeGenericType(ps);
        }
        return value2;
    }
}

class MethodParseData
{
    public MethodParseData(MethodInfo method, Attribute attribute)
    {
        this.parameters = method.GetParameters();
        this.method = method;
        this.mainKey = this.parameters.FirstOrDefault()?.ParameterType;
        this.attribute = attribute;
    }
    public MethodParseData(PropertyInfo property, Attribute attribute)
    {
        this.method = property.SetMethod;
        this.mainKey = property.PropertyType;
        this.attribute = attribute;
        this.property = property;
    }
    public MethodParseData(FieldInfo field, Attribute attribute)
    {
        this.mainKey = field.FieldType;
        this.attribute = attribute;
        this.field = field;
    }

    public Type mainKey;
    public Attribute attribute;

    public MethodInfo method;

    public ParameterInfo[] parameters;
    public PropertyInfo property;
    public FieldInfo field;
}
