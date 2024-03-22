using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Core;

public class Types
{
    public List<Type> types { get; private set; }

    //static Dictionary<Type, FieldInfo> StateMachineThisFieldMap = new();
    //static Dictionary<Type, bool> _asyncNeedCancel = new();
    static Dictionary<Type, MethodParseData[]> methodAttributeCache = new();

    internal List<MethodParseData> Parse(List<Type> ts)
    {
        this.types = ts;
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
            var ms = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            for (int j = 0; j < ms.Length; j++)
            {
                var m = ms[j];
                foreach (var item in m.GetCustomAttributes<SAttribute>())
                    staticMethods.Add(new MethodParseData(m, item));
            }
        }

        var lst = ObjectPool.Get<List<MethodParseData>>();
        for (int i = 0; i < len; i++)
        {
            var type = types[i];
            lst.Clear();

            var fields = ObjectPool.Get<List<FieldInfo>>();
            var tmp = type;
            fields.AddRange(tmp.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            while (tmp.BaseType != null)
            {
                tmp = tmp.BaseType;
                var fs = tmp.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                for (int j = 0; j < fs.Length; j++)
                {
                    if (fs[j].IsPrivate)
                        fields.Add(fs[j]);
                }
            }
            for (int j = 0; j < fields.Count; j++)
            {
                foreach (var item in fields[j].GetCustomAttributes<SAttribute>())
                    lst.Add(new MethodParseData(fields[j], item));
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
                for (int j = 0; j < ps.Length; j++)
                {
                    var p = ps[j];
                    if (p.SetMethod != null && p.SetMethod.IsPrivate)
                        propertys.Add(p);
                }
            }
            for (int j = 0; j < propertys.Count; j++)
            {
                if (propertys[j].SetMethod == null) continue;
                foreach (var item in propertys[j].GetCustomAttributes<SAttribute>())
                    lst.Add(new MethodParseData(propertys[j], item));
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
                for (int j = 0; j < ms.Length; j++)
                {
                    if (ms[j].IsPrivate)
                        methods.Add(ms[j]);
                }
            }
            for (int j = 0; j < methods.Count; j++)
            {
                foreach (var item in methods[j].GetCustomAttributes<SAttribute>())
                    lst.Add(new MethodParseData(methods[j], item));
            }
            methods.Clear();
            ObjectPool.Return(methods);

            if (lst.Count > 0)
                methodAttributeCache[type] = lst.ToArray();
        }
        ObjectPool.Return(lst);

        return staticMethods;
    }

    /*public static FieldInfo GetStateMachineThisField(Type t)
    {
        if (!StateMachineThisFieldMap.TryGetValue(t, out var value))
            StateMachineThisFieldMap[t] = value = t.GetField("<>4__this", BindingFlags.Public | BindingFlags.Instance);

        return value;
    }
    public static bool AsyncInvokeIsNeedAutoCancel(Type stateMachineType)
    {
        return _asyncNeedCancel.TryGetValue(stateMachineType, out bool v) && v;
    }*/

    internal static MethodParseData[] GetInstanceMethodsAttribute(Type type) => methodAttributeCache.TryGetValue(type, out var arr) ? arr : Array.Empty<MethodParseData>();
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
