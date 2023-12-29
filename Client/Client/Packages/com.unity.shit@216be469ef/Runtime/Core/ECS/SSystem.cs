using Game;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Main
{
    static class SSystem
    {
        static Dictionary<Type, Dictionary<Type, Delegate>> attributeMap = new();
        static Dictionary<Type, List<SComponent>> componentMap = new();
        static Dictionary<Type, Delegate> runMis = new();
        static MethodInfo runMi = typeof(SSystem).GetMethod("run", BindingFlags.Static | BindingFlags.NonPublic);
        static object[] parameters1 = new object[1];
        static object[] parameters3 = new object[2];
        static HashSet<Type> removed = new();

        public static void RigisteComponent(SComponent c)
        {
            if (c.Disposed) return;
            if (!componentMap.TryGetValue(c.GetType(), out var value))
                componentMap[c.GetType()] = value = new();
            value.Add(c);
        }
        public static void UnRigisteComponent(SComponent c)
        {
            if (c == null) return;
            removed.Add(c.GetType());
        }
        public static void Run<T>(SComponent c) where T : SSystemAttribute
        {
            if (attributeMap.TryGetValue(typeof(T), out var map))
            {
                if (map.TryGetValue(c.GetType(), out var value))
                {
                    try
                    {
                        parameters1[0] = c;
                        value.DynamicInvoke(parameters1);
                        if (c.Disposed) return;
                    }
                    catch (Exception ex)
                    {
                        Loger.Error($"{typeof(T)}事件执行出错 error:" + ex.ToString());
                    }
                }
            }
        }
        public static void RunAll<T>() where T : SSystemAttribute
        {
            if (attributeMap.TryGetValue(typeof(T), out var map))
            {
                foreach (var kv in map)
                {
                    if (componentMap.TryGetValue(kv.Key, out var cs))
                    {
                        parameters3[0] = cs;
                        parameters3[1] = kv.Value;
                        runMis[kv.Key].DynamicInvoke(parameters3);
                    }
                }
            }
        }
        static void run<T>(List<SComponent> lst, Delegate d) where T : SComponent
        {
            Action<T> call = (Action<T>)d;
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].Disposed) continue;
                try
                {
                    call((T)lst[i]);
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
#if ILRuntime
            if (key is ILRuntime.Reflection.ILRuntimeWrapperType warp)
                key = warp.RealType;
#endif
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
        [Event]
        static void RigisteAllStaticEvent(EC_GameInit e)
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
            RunAll<UpdateAttribute>();
        }
        internal static void AfterUpdate()
        {
            if (removed.Count > 0)
            {
                foreach (var item in removed)
                {
                    if (componentMap.TryGetValue(item, out var value))
                        value.RemoveAll(t => t.Disposed);
                }
                removed.Clear();
            }
        }
    }
}
