using Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Main
{
    public static class SSystem
    {
        static Dictionary<Type, List<MethodHandler>> awakeHandler = new();
        static Dictionary<Type, List<MethodHandler>> disposeHandler = new();
        static Dictionary<Type, List<MethodHandler>> moveHandler = new();
        static Dictionary<Type, List<ChangeHandler>> changeHandler = new();

        static Dictionary<Type, List<Type>> condition = new();
        static Dictionary<Type, AddComponentIfTemp> addIf = new();
        static Dictionary<SObject, HashSet<Type>> checkComponents = ObjectPool<Dictionary<SObject, HashSet<Type>>>.Get();

        static List<SComponent> changeFlag = ObjectPool<List<SComponent>>.Get();

        internal static void RigisteComponent(SComponent c)
        {
            if (condition.TryGetValue(c.GetType(), out var lst))
            {
                if (!checkComponents.TryGetValue(c.Entity, out var cs))
                    checkComponents[c.Entity] = cs = ObjectPool<HashSet<Type>>.Get();
                for (int i = 0; i < lst.Count; i++)
                    cs.Add(lst[i]); 
            }
        }
        internal static void UnRigisteComponent(SComponent c)
        {
            if (condition.TryGetValue(c.GetType(), out var lst))
            {
                if (!checkComponents.TryGetValue(c.Entity, out var cs))
                    checkComponents[c.Entity] = cs = ObjectPool<HashSet<Type>>.Get();
                for (int i = 0; i < lst.Count; i++)
                    cs.Add(lst[i]);
            }
        }
        internal static void SetChange(SComponent c)
        {
            changeFlag.Add(c);
        }
        internal static void CheckComponent(SObject entity, HashSet<Type> types)
        {
            foreach (var type in types)
            {
                if (addIf.TryGetValue(type, out var tmp))
                {
                    if (tmp.all != null)
                    {
                        for (int j = 0; j < tmp.all.Types.Length; j++)
                        {
                            if (!entity.HasComponent(tmp.all.Types[j]))
                                goto remove;
                        }
                    }
                    if (tmp.any != null)
                    {
                        for (int j = 0; j < tmp.any.Types.Length; j++)
                        {
                            if (entity.HasComponent(tmp.any.Types[j]))
                                goto check;
                        }
                        goto remove;
                    }
                check:;
                    if (tmp.none != null)
                    {
                        for (int j = 0; j < tmp.none.Types.Length; j++)
                        {
                            if (entity.HasComponent(tmp.none.Types[j]))
                                goto remove;
                        }
                    }
                    entity.AddComponentInternal(type);
                    break;
                remove: entity.RemoveComponentInternal(type);
                }
            }
        }
        internal static bool isAutoAddComponent(Type type) => addIf.ContainsKey(type);
        internal static void Awake(SComponent c)
        {
            if (awakeHandler.TryGetValue(c.GetType(), out var value))
            {
                var ps = ArrayCache<object>.Get(1);
                for (int i = 0; i < value.Count; i++)
                {
                    try
                    {
                        ps[0] = c;
                        value[i].method.Invoke(null, ps);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error($"Awake事件执行出错 error:" + ex.ToString());
                    }
                    if (c.Disposed) return;
                }
            }
        }
        internal static void Dispose(SComponent c)
        {
            if (disposeHandler.TryGetValue(c.GetType(), out var value))
            {
                var ps = ArrayCache<object>.Get(1);
                for (int i = 0; i < value.Count; i++)
                    try
                    {
                        ps[0] = c;
                        value[i].method.Invoke(null, ps);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error($"Dispose事件执行出错 error:" + ex.ToString());
                    }
            }
        }
        internal static void Move(SComponent c, SObject old)
        {
            if (moveHandler.TryGetValue(c.GetType(), out var value))
            {
                var ps = ArrayCache<object>.Get(2);
                for (int i = 0; i < value.Count; i++)
                {
                    try
                    {
                        ps[0] = c;
                        ps[1] = old;
                        value[i].method.Invoke(null, ps);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error($"Move事件执行出错 error:" + ex.ToString());
                    }
                    if (c.Disposed) return;
                }
            }
        }

        [Conditional("DebugEnable")]
        static void _checkMethod()
        {
            for (int i = 0; i < Types.AllTypes.Count; i++)
            {
                var type = Types.AllTypes[i];
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                for (int j = 0; j < methods.Length; j++)
                {
                    var method = methods[j];
                    var atts = method.GetCustomAttributes<SSystemAttribute>();
                    if (atts.Count() > 1)
                        Loger.Error("SSystemAttribute 同一个函数最多一个");
                    var att = atts.FirstOrDefault();
                    if (att != null)
                    {
                        if (!method.IsStatic)
                            Loger.Error("SSystemAttribute 只能标记在静态函数上");
                        var ps = method.GetParameters();
                        if (method.IsGenericMethod)
                            Loger.Error($"事件函数不能是泛型函数 class:{method.ReflectedType.FullName} method:{method.Name}");
                        if (method.ReturnType != typeof(void))
                            Loger.Error($"事件函数返回值无效 class:{method.ReflectedType.FullName} method:{method.Name}");

                        if (att is not MoveAttribute)
                        {
                            List<Type> check = ObjectPool<List<Type>>.Get();
                            for (int k = 0; k < ps.Length; k++)
                            {
                                var key = ps[k].ParameterType;
                                if (!typeof(SComponent).IsAssignableFrom(key))
                                    Loger.Error($"{key}不是ECS组件");
                                if (check.Contains(key))
                                    Loger.Error($"参数类型重复 class:{method.ReflectedType.FullName}  method: {method.Name}");
                                check.Add(key);
                            }
                            check.Clear();
                            ObjectPool<List<Type>>.Return(check);
                        }

                        //只有Change 可接受复合参数组件事件
                        if (att is ChangeAttribute)
                        {
                            if (ps.Length > ArrayCache<object>.Max)
                                Loger.Error($"Change事件 参数个数最多为{ArrayCache<object>.Max} class:{method.ReflectedType.FullName} method:{method.Name}");
                        }
                        else if (att is MoveAttribute)
                        {
                            if (ps.Length != 2 || !typeof(SComponent).IsAssignableFrom(ps[0].ParameterType) || typeof(SObject) != ps[1].ParameterType)
                                Loger.Error($"{att}事件 参数为0=SComponent组件 1=SObject强类型实体(不可为SObject的子类型) class:{method.ReflectedType.FullName} method:{method.Name}");
                        }
                        else
                        {
                            if (ps.Length != 1)
                                Loger.Error($"{att}事件 参数个数不正确 class:{method.ReflectedType.FullName} method:{method.Name}");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        internal static void Init(List<MethodParseData> methods)
        {
            _checkMethod();
            for (int i = 0; i < methods.Count; i++)
            {
                MethodParseData ma = methods[i];
                if (ma.attribute is SSystemAttribute)
                {
                    //只有Change 可接受复合参数组件事件
                    if (ma.attribute is ChangeAttribute)
                    {
                        ChangeHandler h = new(ma.parameters.Select(t => t.ParameterType).ToArray(), new MethodHandler(ma.method, ((SSystemAttribute)ma.attribute).SortOrder));
                        for (int j = 0; j < ma.parameters.Length; j++)
                        {
                            var key = ma.parameters[j].ParameterType;
                            if (!changeHandler.TryGetValue(key, out var hs))
                                changeHandler[key] = hs = new();
                            hs.Add(h);
                        }
                    }
                    else 
                    {
                        Dictionary<Type, List<MethodHandler>> handler = default;
                        if (ma.attribute is AwakeAttribute) handler = awakeHandler;
                        else if (ma.attribute is DisposeAttribute) handler = disposeHandler;
                        else if (ma.attribute is MoveAttribute) handler = moveHandler;
                        else Loger.Error("未知类型 =" + ma.attribute);
                        var key = ma.parameters[0].ParameterType;
                        if (!handler.TryGetValue(key, out var ds))
                            handler[key] = ds = new();
                        ds.Add(new MethodHandler(ma.method, ((SSystemAttribute)ma.attribute).SortOrder));
                    }
                }
            }
            foreach (var item in changeHandler.Values)
                item.Sort((x, y) => x.method.sortOrder - y.method.sortOrder);
            foreach (var item in awakeHandler.Values)
                item.Sort((x, y) => x.sortOrder - y.sortOrder);
            foreach (var item in disposeHandler.Values)
                item.Sort((x, y) => x.sortOrder - y.sortOrder);
            foreach (var item in moveHandler.Values)
                item.Sort((x, y) => x.sortOrder - y.sortOrder);

            for (int i = 0; i < Types.AllTypes.Count; i++)
            {
                Type t = Types.AllTypes[i];
                if (typeof(SComponent).IsAssignableFrom(t))
                {
                    var all = t.GetCustomAttribute<AddComponentIfAll>();
                    var any = t.GetCustomAttribute<AddComponentIfAny>();
                    var none = t.GetCustomAttribute<AddComponentIfNone>();
                    if (none != null && all == null && any == null)
                    {
                        Loger.Error($"{nameof(AddComponentIfNone)}不能单独存在");
                        continue;
                    }
                    if (all != null || any != null || none != null)
                    {
                        AddComponentIfTemp tmp = new();
                        tmp.all = all;
                        tmp.any = any;
                        tmp.none = none;
                        addIf[t] = tmp;

                        if (all != null)
                        {
                            for (int j = 0; j < all.Types.Length; j++)
                            {
                                if (!condition.TryGetValue(all.Types[j], out var list))
                                    condition[all.Types[j]] = list = new();
                                if (!list.Contains(t))
                                    list.Add(t);
                            }
                        }
                        if (any != null)
                        {
                            for (int j = 0; j < any.Types.Length; j++)
                            {
                                if (!condition.TryGetValue(any.Types[j], out var list))
                                    condition[any.Types[j]] = list = new();
                                if (!list.Contains(t))
                                    list.Add(t);
                            }
                        }
                        if (none != null)
                        {
                            for (int j = 0; j < none.Types.Length; j++)
                            {
                                if (!condition.TryGetValue(none.Types[j], out var list))
                                    condition[none.Types[j]] = list = new();
                                if (!list.Contains(t))
                                    list.Add(t);
                            }
                        }
                    }
                }
            }
        }
        internal static void Update()
        {
            if (checkComponents.Count > 0)
            {
                var tmp = checkComponents;
                checkComponents = ObjectPool<Dictionary<SObject, HashSet<Type>>>.Get();
                foreach (var c in tmp)
                {
                    if (c.Key.Disposed) continue;
                    CheckComponent(c.Key, c.Value);
                    c.Value.Clear();
                    ObjectPool<HashSet<Type>>.Return(c.Value);
                }
                tmp.Clear();
                ObjectPool<Dictionary<SObject, HashSet<Type>>>.Return(tmp);
            }

            if (changeFlag.Count > 0)
            {
                var tmp = changeFlag;
                changeFlag = ObjectPool<List<SComponent>>.Get();
                for (int i = 0; i < tmp.Count; i++)
                {
                    var c = tmp[i];
                    if (c.Disposed) continue;
                    c._isChanged = false;
                    if (changeHandler.TryGetValue(c.GetType(), out var handlers))
                    {
                        for (int j = 0; j < handlers.Count; j++)
                            handlers[j].TryInvoke(c.Entity);
                    }
                }
                tmp.Clear();
                ObjectPool<List<SComponent>>.Return(tmp);
            }
        }
        internal static void AfterUpdate()
        {
            
        }

        class ChangeHandler
        {
            public ChangeHandler(Type[] types, MethodHandler method)
            {
                this.types = types;
                this.method = method;
            }
            public MethodHandler method;
            public Type[] types;

            public void TryInvoke(SObject obj)
            {
                var ps = ArrayCache<object>.Get(types.Length);
                for (int i = 0; i < types.Length; i++)
                {
                    if (!obj.TryGetComponent(types[i], out var c))
                        return;
                    ps[i] = c;
                }
                try
                {
                    method.method.Invoke(null, ps);
                }
                catch (Exception ex)
                {
                    Loger.Error($"{method}事件执行出错 error:" + ex.ToString());
                }
            }
        }
        class MethodHandler
        {
            public MethodHandler(MethodInfo mi,int sortOrder)
            {
                this.method = mi;
                this.sortOrder = sortOrder;
            }
            public MethodInfo method;
            public int sortOrder;
        }
        class AddComponentIfTemp
        {
            public AddComponentIfAll all;
            public AddComponentIfAny any;
            public AddComponentIfNone none;
        }
    }
}
