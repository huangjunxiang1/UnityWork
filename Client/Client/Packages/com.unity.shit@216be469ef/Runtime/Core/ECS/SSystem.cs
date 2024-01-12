using Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        static Dictionary<Type, Dictionary<Type, Delegate>> attributeMap = new();
        static Dictionary<Type, Component> componentMap = new();
        static Dictionary<Type, Delegate> runMis = new();
        static MethodInfo runMi = typeof(SSystem).GetMethod("run", BindingFlags.Static | BindingFlags.NonPublic);
        static object[] parameters1 = new object[1];
        static object[] parameters2 = new object[2];
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
            if (attributeMap.TryGetValue(typeof(T), out var map))
            {
                if (map.TryGetValue(c.GetType(), out var value))
                {
                    try
                    {
                        parameters1[0] = c;
                        value.DynamicInvoke(parameters1);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error($"{typeof(T)}事件执行出错 error:" + ex.ToString());
                    }
                }
            }
        }
        public static void RunAll<T, K>() where T : SSystemAttribute where K : SComponent
        {
            if (attributeMap.TryGetValue(typeof(T), out var map))
            {
                if (map.TryGetValue(typeof(K), out var d))
                {
                    if (componentMap.TryGetValue(typeof(K), out var cs) && cs.components.Count > 0)
                    {
                        run<K>(cs.components, d);
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

        static void run<T>(List<SComponent> lst, Delegate d) where T : SComponent
        {
            Action<T> call = (Action<T>)d;
            for (int i = 0; i < lst.Count; i++)
            {
                var c = lst[i];
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
        static bool _checkEventMethod(MethodInfo method, out Type key)
        {
            key = null;
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
            var ps = method.GetParameters();
            if (ps.Length != 1)
            {
                Loger.Error($"无法解析的参数类型 class:{method.ReflectedType.FullName} method:{method.Name}");
                return false;
            }
            key = ps[0].ParameterType;
            if (!typeof(SComponent).IsAssignableFrom(key))
            {
                Loger.Error($"{key}不是ECS组件");
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
                    if (!_checkEventMethod(ma.method, out var key))
                        continue;

                    if (!attributeMap.TryGetValue(ea.GetType(), out var map))
                        attributeMap[ea.GetType()] = map = new();
                    if (!map.ContainsKey(key))
                    {
                        types[0] = key;
                        map[key] = ma.method.CreateDelegate(typeof(Action<>).MakeGenericType(types));
                    }
                    else
                    {
                        Loger.Error($"{key}的{ea}处理重复 1={map[key].Method.ReflectedType.Name}.{map[key].Method.Name} 2={ma.method.ReflectedType.Name}.{ma.method.Name}");
                    }
                    if (!runMis.ContainsKey(key))
                        runMis[key] = runMi.MakeGenericMethod(types).CreateDelegate(typeof(Action<List<SComponent>, Delegate>));
                }
            }
        }
        internal static void Update()
        {
            if (changeHash.Count>0)
            {
                if (attributeMap.TryGetValue(typeof(ChangeAttribute), out var map1))
                {
                    var hs = changeHash;
                    changeHash = ObjectPool.Get<HashSet<SComponent>>();
                    foreach (var c in hs)
                    {
                        if (c.Disposed) continue;
                        if (map1.TryGetValue(c.GetType(), out var action))
                        {
                            parameters1[0] = c;
                            action.DynamicInvoke(parameters1);
                        }
                    }
                    hs.Clear();
                    ObjectPool.Return(hs);
                }
            }

            if (attributeMap.TryGetValue(typeof(UpdateAttribute), out var map))
            {
                foreach (var kv in map)
                {
                    if (componentMap.TryGetValue(kv.Key, out var cs) && cs.components.Count > 0)
                    {
                        parameters2[0] = cs.components;
                        parameters2[1] = kv.Value;
                        runMis[kv.Key].DynamicInvoke(parameters2);
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
    }
}
