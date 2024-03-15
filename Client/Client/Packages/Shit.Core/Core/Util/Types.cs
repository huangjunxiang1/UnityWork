using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Core;

public static class CoreTypes
{
    public static List<Type> types;
   
    static Dictionary<Type, FieldInfo> StateMachineThisFieldMap = new();
    static Dictionary<Type, bool> _asyncNeedCancel = new();
    static Dictionary<Type, MethodParseData[]> methodAttributeCache = new();

    public static List<MethodParseData> Parse(List<Type> ts)
    {
        STimer.Check(ts);
        EventSystem.Check(ts);

        types = ts;
        int len = types.Count;
        List<MethodParseData> staticMethods = new(1000);
        for (int i = 0; i < len; i++)
        {
            Type type = types[i];
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
        }
        var lst = ObjectPool.Get<List<Type>>();
        for (int i = 0; i < len; i++)
        {
            Type type = types[i];

            var ms = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            for (int j = 0; j < ms.Length; j++)
            {
                var m = ms[j];
                var asm = m.GetCustomAttribute<AsyncStateMachineAttribute>();
                if (asm != null)
                    _asyncNeedCancel[asm.StateMachineType] = m.GetCustomAttribute<AsynCancelIfCallerDisposedAttribute>() != null;

                foreach (var item in m.GetCustomAttributes<SAttribute>())
                    staticMethods.Add(new MethodParseData(m, item));
            }
        }
        lst.Clear();
        ObjectPool.Return(lst);

        SSystem.Init(staticMethods, types);

        return staticMethods;
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

    public static MethodParseData[] GetInstanceMethodsAttribute(Type type)
    {
        if (!methodAttributeCache.TryGetValue(type, out var array))
        {
            var lst = ObjectPool.Get<List<MethodParseData>>();

            var fields = ObjectPool.Get<List<FieldInfo>>();
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
            ObjectPool.Return(fields);

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

            var methods = ObjectPool.Get<List<MethodInfo>>();
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
            ObjectPool.Return(methods);

            methodAttributeCache[type] = array = lst.Count == 0 ? Array.Empty<MethodParseData>() : lst.ToArray();
            lst.Clear();
            ObjectPool.Return(lst);
        }
        return array;
    }
}

public class MethodParseData
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
