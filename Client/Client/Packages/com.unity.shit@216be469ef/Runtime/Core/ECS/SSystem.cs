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
    public static class SSystem
    {
        static Dictionary<Type, Dictionary<Type, List<Delegate>>> handlerMap = new();
        static Dictionary<Type, List<Delegate>> updateDelegates;
        static Dictionary<Type, List<ChangeHandler>> compoundHandlerMap = new();
        static Dictionary<Type, Component> componentMap = new();
        static Dictionary<Type, Delegate> runMis = new();
        static MethodInfo runMi = typeof(SSystem).GetMethod(nameof(run), BindingFlags.Static | BindingFlags.NonPublic);
        static HashSet<Type> removed = new();
        static HashSet<SComponent> changeHash = ObjectPool.Get<HashSet<SComponent>>();

        internal static void RigisteComponent(SComponent c)
        {
            if (!componentMap.TryGetValue(c.GetType(), out var value))
                componentMap[c.GetType()] = value = new();
            value.Add(c);
        }
        internal static void UnRigisteComponent(SComponent c)
        {
            if (componentMap.TryGetValue(c.GetType(), out var value))
                value.Remove(c);
            removed.Add(c.GetType());
        }
        internal static void SetChange(SComponent c)
        {
            changeHash.Add(c);
        }

        internal static void Run<T>(SComponent c) where T : SSystemAttribute
        {
            if (handlerMap.TryGetValue(typeof(T), out var map))
            {
                if (map.TryGetValue(c.GetType(), out var value))
                {
                    try
                    {
                        var ps = ParametersArrayCache.Get(1);
                        ps[0] = c;
                        for (int i = 0; i < value.Count; i++)
                            value[i].DynamicInvoke(ps);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error($"{typeof(T)}事件执行出错 error:" + ex.ToString());
                    }
                }
            }
        }

        public static bool QueryWithRpc<T>(long rpc, out T c) where T : SComponent
        {
            if (componentMap.TryGetValue(typeof(T), out var value))
            {
                if (value.rpcMap.TryGetValue(rpc, out var cc))
                {
                    c = (T)cc;
                    return true;
                }
            }
            c = default;
            return false;
        }
        public static bool QueryWithGid<T>(long gid, out T c) where T : SComponent
        {
            if (componentMap.TryGetValue(typeof(T), out var value))
            {
                if (value.gidMap.TryGetValue(gid, out var cc))
                {
                    c = (T)cc;
                    return true;
                }
            }
            c = default;
            return false;
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
                    for (int j = 0; j < ps.Length; j++)
                    {
                        if (!typeof(SComponent).IsAssignableFrom(ps[j].ParameterType))
                        {
                            Loger.Error($"{ps[j].ParameterType}不是ECS组件");
                            goto go;
                        }
                    }

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
            updateDelegates = handlerMap.TryGetValue(typeof(UpdateAttribute), out var dc) ? dc : new(0);
        }
        internal static void Update()
        {
            if (changeHash.Count > 0)
            {
                var hs = changeHash;
                changeHash = ObjectPool.Get<HashSet<SComponent>>();
                foreach (var c in hs)
                {
                    if (c.Disposed) continue;
                    if (compoundHandlerMap.TryGetValue(c.GetType(), out var handlers))
                    {
                        for (int i = 0; i < handlers.Count; i++)
                            handlers[i].TryInvoke(c.Entity);
                    }
                }
                hs.Clear();
                ObjectPool.Return(hs);
            }

            foreach (var kv in updateDelegates)
            {
                if (componentMap.TryGetValue(kv.Key, out var cs) && cs.components.Count > 0)
                {
                    var ps = ParametersArrayCache.Get(2);
                    ps[0] = cs.components;
                    ps[1] = kv.Value;
                    runMis[kv.Key].DynamicInvoke(ps);
                }
            }
        }
        internal static void AfterUpdate()
        {
            if (removed.Count > 0)
            {
                foreach (var item in removed)
                {
                    if (componentMap.TryGetValue(item, out var value))
                        value.RemoveAll();
                }
                removed.Clear();
            }
        }

        class Component
        {
            public List<SComponent> components = new();
            public Dictionary<long, SComponent> rpcMap = new();
            public Dictionary<long, SComponent> gidMap = new();

            public void Add(SComponent c)
            {
                components.Add(c);
                if (c.Entity.rpc > 0)
                {
                    if (!rpcMap.TryGetValue(c.Entity.rpc, out var cc))
                        rpcMap[c.Entity.rpc] = c;
                    else
                        Loger.Error($"已经包含rpc组件 c={c} rpc={cc.Entity.rpc}");
                }
                if (!gidMap.TryGetValue(c.Entity.gid, out var ccc))
                    gidMap[c.Entity.gid] = c;
                else
                    Loger.Error($"已经包含gid组件 c={c} gid={ccc.Entity.gid}");
            }
            public void Remove(SComponent c)
            {
                rpcMap.Remove(c.Entity.rpc);
                gidMap.Remove(c.Entity.gid);
            }
            public void RemoveAll()
            {
                components.RemoveAll(t => t.Disposed);
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
                for (int i = 0; i < types.Length; i++)
                {
                    if (!obj.HasComponent(types[i]))
                        return;
                }
                var ps = ParametersArrayCache.Get(types.Length);
                for (int i = 0; i < ps.Length; i++)
                {
                    var c = obj.GetComponent(types[i]);
                    ps[typeIndex[c.GetType()]] = c;
                }
                method.Invoke(null, ps);
            }
        }
    }
}
