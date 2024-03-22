using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Core
{
    internal class SSystem
    {
        public SSystem(CoreWorld world) => this.world = world;

        CoreWorld world;
        Dictionary<Type, List<Action<object, CoreWorld>>> awakeHandle = new();
        Dictionary<Type, List<Action<object, CoreWorld>>> disposeHandle = new();
        Dictionary<Type, List<Action<SComponent, CoreWorld>>> enableHandle = new();
        Dictionary<Type, List<Action<SObject>>> changeHandle = new();

        Dictionary<Type, List<Func<SObject, __UpdateHandle>>> updateHandlerCreater = new();

        HashSet<__ChangeHandle> changeHandles = ObjectPool.Get<HashSet<__ChangeHandle>>();
        Queue<__UpdateHandle> updateHandles = ObjectPool.Get<Queue<__UpdateHandle>>();

        internal void TryAddChangeHandler(SComponent c)
        {
            if (changeHandle.TryGetValue(c.GetType(), out var list))
            {
                for (int i = 0; i < list.Count; i++)
                    list[i].Invoke(c.Entity);
            }
        }
        internal void TryAddUpdateHandler(SComponent c)
        {
            if (updateHandlerCreater.TryGetValue(c.GetType(), out var list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var v = list[i].Invoke(c.Entity);
                    if (v != null)
                        updateHandles.Enqueue(v);
                }
            }
        }

        internal void Awake(Type type, object o)
        {
            if (!awakeHandle.TryGetValue(type, out var list)) return;
            for (int i = 0; i < list.Count; i++)
            {
                try { list[i](o, world); }
                catch (Exception e) { Loger.Error("Awake 出错 " + e); }
            }
        }
        internal void Dispose(Type type, object o)
        {
            if (!disposeHandle.TryGetValue(type, out var list)) return;
            for (int i = 0; i < list.Count; i++)
            {
                try { list[i](o, world); }
                catch (Exception e) { Loger.Error("Dispose 出错 " + e); }
            }
        }
        internal void Enable(SComponent c)
        {
            if (!enableHandle.TryGetValue(c.GetType(), out var list)) return;
            for (int i = 0; i < list.Count; i++)
            {
                try { list[i](c, world); }
                catch (Exception e) { Loger.Error("Enable 出错 " + e); }
            }
        }
        internal void Change(SComponent c)
        {
            for (int i = 0; i < c._changeHandles.Count; i++)
                changeHandles.Add(c._changeHandles[i]);
        }

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        internal void Load(List<MethodParseData> methods)
        {
            awakeHandle.Clear();
            disposeHandle.Clear();
            enableHandle.Clear();
            changeHandle.Clear();

            updateHandlerCreater.Clear();

            changeHandles.Clear();
            updateHandles.Clear();

            HashSet<Type> awake = new();
            HashSet<Type> dispose = new();
            HashSet<Type> enable = new();
            HashSet<Type> change = new();
            HashSet<Type> update = new();
            for (int i = 0; i < methods.Count; i++)
            {
                MethodParseData ma = methods[i];
                if (ma.attribute is EventAttribute && typeof(__SystemHandle).IsAssignableFrom(ma.mainKey))
                {
                    if (typeof(__AwakeHandle).IsAssignableFrom(ma.mainKey))
                    {
                        awake.Add(ma.mainKey);
                        continue;
                    }
                    if (typeof(__DisposeHandle).IsAssignableFrom(ma.mainKey))
                    {
                        dispose.Add(ma.mainKey);
                        continue;
                    }
                    if (typeof(__EnableHandle).IsAssignableFrom(ma.mainKey))
                    {
                        enable.Add(ma.mainKey);
                        continue;
                    }
                    if (typeof(__ChangeHandle).IsAssignableFrom(ma.mainKey))
                    {
                        if (ma.method != null && ma.method.ReturnType == typeof(STask))
                        {
                            Loger.Error($"change处理事件不可使用task class:{ma.method.ReflectedType.FullName} method:{ma.method.Name}");
                            continue;
                        }
                        change.Add(ma.mainKey);
                        continue;
                    }
                    if (typeof(__UpdateHandle).IsAssignableFrom(ma.mainKey))
                    {
                        if (ma.method != null && ma.method.ReturnType == typeof(STask))
                        {
                            Loger.Error($"update处理事件不可使用task class:{ma.method.ReflectedType.FullName} method:{ma.method.Name}");
                            continue;
                        }
                        update.Add(ma.mainKey);
                        continue;
                    }
                }
            }
            foreach (var item in awake)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<object, CoreWorld>)item.GetMethod(nameof(Awake<SObject>.Invoke), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<object, CoreWorld>));
                if (!awakeHandle.TryGetValue(ts[0], out var lst))
                    awakeHandle[ts[0]] = lst = new();
                lst.Add(action);
            }
            foreach (var item in dispose)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<object, CoreWorld>)item.GetMethod(nameof(Dispose<SObject>.Invoke), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<object, CoreWorld>));
                if (!disposeHandle.TryGetValue(ts[0], out var lst))
                    disposeHandle[ts[0]] = lst = new();
                lst.Add(action);
            }
            foreach (var item in enable)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<SComponent, CoreWorld>)item.GetMethod(nameof(Enable<SComponent>.Invoke), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<SComponent, CoreWorld>));
                if (!enableHandle.TryGetValue(ts[0], out var lst))
                    enableHandle[ts[0]] = lst = new();
                lst.Add(action);
            }
            foreach (var item in change)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<SObject>)item.GetMethod(nameof(AnyChange<SComponent, SComponent>.TryCreateHandle), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<SObject>));
                for (int j = 0; j < ts.Length; j++)
                {
                    var key = ts[j];
                    if (!changeHandle.TryGetValue(key, out var lst))
                        changeHandle[key] = lst = new();
                    lst.Add(action);
                }
            }
            foreach (var item in update)
            {
                var ts = item.GetGenericArguments();
                var action = (Func<SObject, __UpdateHandle>)item.GetMethod(nameof(Update<SComponent>.TryCreateHandle), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Func<SObject, __UpdateHandle>));
                for (int j = 0; j < ts.Length; j++)
                {
                    var key = ts[j];
                    if (!updateHandlerCreater.TryGetValue(key, out var lst))
                        updateHandlerCreater[key] = lst = new();
                    lst.Add(action);
                }
            }
        }
        internal void Update()
        {
            if (changeHandles.Count > 0)
            {
                var tmp = changeHandles;
                changeHandles = ObjectPool.Get<HashSet<__ChangeHandle>>();
                foreach (var c in tmp)
                {
                    if (!c.Disposed)
                        c.Invoke(world);
                }
                tmp.Clear();
                ObjectPool.Return(tmp);
            }

            var update = updateHandles;
            updateHandles = ObjectPool.Get<Queue<__UpdateHandle>>();
            while (update.TryDequeue(out var u))
            {
                if (!u.IsValid())
                    continue;
                u.Invoke(world);
                updateHandles.Enqueue(u);
            }
            ObjectPool.Return(update);
        }
        internal void AfterUpdate()
        {

        }
    }
}
