using Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Progress;

namespace Main
{
    static class SSystem
    {
        static Dictionary<Type, Dictionary<Type, List<Delegate>>> handlerMap = new();
        static Dictionary<Type, List<Delegate>> updateDelegates;
        static Dictionary<Type, List<ChangeHandler>> compoundHandlerMap = new();

        static Dictionary<Type, List<Type>> condition = new();
        static Dictionary<Type, AddComponentIfTemp> addIf = new();
        static Dictionary<SObject, HashSet<Type>> checkComponents = ObjectPool.Get<Dictionary<SObject, HashSet<Type>>>();

        static Dictionary<Type, List<SComponent>> updateComponents = new();
        static Dictionary<Type, Delegate> runMis = new();
        static MethodInfo runMi = typeof(SSystem).GetMethod(nameof(run), BindingFlags.Static | BindingFlags.NonPublic);
        static HashSet<Type> removed = new();
        static HashSet<SComponent> changeFlag = ObjectPool.Get<HashSet<SComponent>>();

        internal static void RigisteComponent(SComponent c)
        {
            if (updateDelegates != null && updateDelegates.ContainsKey(c.GetType()))
            {
                if (!updateComponents.TryGetValue(c.GetType(), out var value))
                    updateComponents[c.GetType()] = value = new();
                value.Add(c);
            }
            if (condition.TryGetValue(c.GetType(), out var lst))
            {
                if (!checkComponents.TryGetValue(c.Entity, out var cs))
                    checkComponents[c.Entity] = cs = ObjectPool.Get<HashSet<Type>>();
                for (int i = 0; i < lst.Count; i++)
                    cs.Add(lst[i]);
            }
        }
        internal static void UnRigisteComponent(SComponent c)
        {
            removed.Add(c.GetType());
            if (condition.TryGetValue(c.GetType(), out var lst))
            {
                if (!checkComponents.TryGetValue(c.Entity, out var cs))
                    checkComponents[c.Entity] = cs = ObjectPool.Get<HashSet<Type>>();
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
                    bool addit = false;
                    if (tmp.all != null)
                    {
                        for (int j = 0; j < tmp.all.Types.Length; j++)
                        {
                            if (!entity.HasComponent(tmp.all.Types[j]))
                                goto here;
                        }
                    }
                    if (tmp.any != null)
                    {
                        bool has = false;
                        for (int j = 0; j < tmp.any.Types.Length; j++)
                        {
                            if (entity.HasComponent(tmp.any.Types[j]))
                            {
                                has = true;
                                break;
                            }
                        }
                        if (!has)
                            goto here;
                    }
                    if (tmp.none != null)
                    {
                        for (int j = 0; j < tmp.none.Types.Length; j++)
                        {
                            if (entity.HasComponent(tmp.none.Types[j]))
                                goto here;
                        }
                    }
                    addit = true;
                here:;
                    if (addit)
                        entity.AddComponentInternal(type);
                    else
                        entity.RemoveComponentInternal(type);
                }
            }
        }
        internal static bool isAutoAddComponent(Type type) => addIf.ContainsKey(type);
        internal static void Run<T>(SComponent c) where T : SSystemAttribute
        {
            if (handlerMap.TryGetValue(typeof(T), out var map))
            {
                if (map.TryGetValue(c.GetType(), out var value))
                {
                    var ps = ParametersArrayCache.Get(1);
                    ps[0] = c;
                    for (int i = 0; i < value.Count; i++)
                        try
                        {
                            value[i].DynamicInvoke(ps);
                        }
                        catch (Exception ex)
                        {
                            Loger.Error($"{typeof(T)}事件执行出错 error:" + ex.ToString());
                        }
                }
            }
        }

        static void run<T>(List<SComponent> lst, List<Delegate> ds) where T : SComponent
        {
            for (int i = 0; i < ds.Count; i++)
            {
                Action<T> call = (Action<T>)ds[i];
                for (int j = 0; j < lst.Count; j++)
                {
                    var c = lst[j];
                    if (c.Disposed || !c.Enable) continue;
                    try
                    {
                        call((T)c);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error($"{typeof(T)}事件执行出错 error:" + ex.ToString());
                    }
                }
            }
        }
        static bool _checkEventMethod(MethodInfo method)
        {
            if (method.IsGenericMethod)
            {
                Loger.Error($"事件函数不能是泛型函数 class:{method.ReflectedType.FullName} method:{method.Name}");
                return false;
            }
            if (method.ReturnType != typeof(void))
            {
                Loger.Error($"事件函数返回值无效 class:{method.ReflectedType.FullName} method:{method.Name}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        internal static void Init()
        {
            Type[] types = new Type[1];
            var lst = Types.GetStaticMethods();
            for (int i = 0; i < lst.Count; i++)
            {
                MethodAndAttribute ma = lst[i];
                if (ma.attribute is SSystemAttribute ea)
                {
                    if (!_checkEventMethod(ma.method))
                        continue;

                    var ps = ma.method.GetParameters();
#if DebugEnable
                    List<Type> check = new();
                    for (int j = 0; j < ps.Length; j++)
                    {
                        var key = ps[j].ParameterType;
                        if (!typeof(SComponent).IsAssignableFrom(key))
                            Loger.Error($"{key}不是ECS组件");
                        if (check.Contains(key))
                            Loger.Error($"参数类型重复 class:{ma.method.ReflectedType.FullName} method:{ma.method.Name}");
                        check.Add(key);
                    }
#endif

                    //只有Change 可接受复合参数组件事件
                    if (ma.attribute is not ChangeAttribute)
                    {
                        if (ps.Length != 1)
                        {
                            Loger.Error($"无法解析的参数类型 class:{ma.method.ReflectedType.FullName} method:{ma.method.Name}");
                            goto go;
                        }

                        var key = ps[0].ParameterType;
                        if (!handlerMap.TryGetValue(ea.GetType(), out var map))
                            handlerMap[ea.GetType()] = map = new();
                        if (!map.TryGetValue(key, out var ds))
                            map[key] = ds = new();
                        types[0] = key;
                        ds.Add(ma.method.CreateDelegate(typeof(Action<>).MakeGenericType(types)));
                        if (!runMis.ContainsKey(key))
                            runMis[key] = runMi.MakeGenericMethod(types).CreateDelegate(typeof(Action<List<SComponent>, List<Delegate>>));
                    }
                    else
                    {
                        if (ps.Length > 5)
                        {
                            Loger.Error($"参数类型最多为5 class:{ma.method.ReflectedType.FullName} method:{ma.method.Name}");
                            goto go;
                        }
                      
                        ChangeHandler h = new(ps.Select(t => t.ParameterType).ToArray());
                        h.method = ma.method;
                        for (int j = 0; j < ps.Length; j++)
                        {
                            var key = ps[j].ParameterType;

                            if (!compoundHandlerMap.TryGetValue(key, out var hs))
                                compoundHandlerMap[key] = hs = new();
                            hs.Add(h);
                        }
                    }
                go:;
                }
            }
            handlerMap.TryGetValue(typeof(UpdateAttribute), out updateDelegates);

            for (int i = 0; i < Types.AllTypes.Length; i++)
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
                checkComponents = ObjectPool.Get<Dictionary<SObject, HashSet<Type>>>();
                foreach (var c in tmp)
                {
                    if (c.Key.Disposed) continue;
                    CheckComponent(c.Key, c.Value);
                    c.Value.Clear();
                    ObjectPool.Return(c.Value);
                }
                tmp.Clear();
                ObjectPool.Return(tmp);
            }

            if (changeFlag.Count > 0)
            {
                var tmp = changeFlag;
                changeFlag = ObjectPool.Get<HashSet<SComponent>>();
                foreach (var c in tmp)
                {
                    if (c.Disposed) continue;
                    if (compoundHandlerMap.TryGetValue(c.GetType(), out var handlers))
                    {
                        for (int i = 0; i < handlers.Count; i++)
                            handlers[i].TryInvoke(c.Entity);
                    }
                }
                tmp.Clear();
                ObjectPool.Return(tmp);
            }

            if (updateDelegates != null)
            {
                foreach (var kv in updateDelegates)
                {
                    if (updateComponents.TryGetValue(kv.Key, out var cs) && cs.Count > 0)
                    {
                        var ps = ParametersArrayCache.Get(2);
                        ps[0] = cs;
                        ps[1] = kv.Value;
                        runMis[kv.Key].DynamicInvoke(ps);
                    }
                }
            }
        }
        internal static void AfterUpdate()
        {
            if (removed.Count > 0)
            {
                foreach (var item in removed)
                {
                    if (updateComponents.TryGetValue(item, out var value))
                        value.RemoveAll(t => t.Disposed);
                }
                removed.Clear();
            }
        }

        class ChangeHandler
        {
            public ChangeHandler(Type[] types)
            {
                this.types = types;
                for (int i = 0; i < types.Length; i++)
                    typeIndex[types[i]] = i;
            }
            public MethodInfo method;
            public Type[] types;

            Dictionary<Type, int> typeIndex = new(); 

            public void TryInvoke(SObject obj)
            {
                var ps = ParametersArrayCache.Get(types.Length);
                for (int i = 0; i < types.Length; i++)
                {
                    if (!obj.TryGetComponent(types[i], out var c))
                        return;
                    ps[typeIndex[c.GetType()]] = c;
                }
                try
                {
                    method.Invoke(null, ps);
                }
                catch (Exception ex)
                {
                    Loger.Error($"{method}事件执行出错 error:" + ex.ToString());
                }
            }
        }
        class AddComponentIfTemp
        {
            public AddComponentIfAll all;
            public AddComponentIfAny any;
            public AddComponentIfNone none;
        }
    }
}
