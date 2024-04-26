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
        public SSystem(World world) => this.world = world;

        World world;
        Dictionary<Type, List<Action<SObject>>> awakeHandle = new();
        Dictionary<Type, List<Action<object>>> disposeHandle = new();
        Dictionary<Type, List<Action<SComponent>>> enableHandle = new();
        Dictionary<Type, List<Action<SObject>>> changeHandle = new();
        Dictionary<Type, List<(Type, Action<object, SObject>)>> eventWatcherHandle = new();
        Dictionary<Type, List<Action<SObject>>> kvWatcherHandle = new();
        Dictionary<Type, List<Action<SObject>>> timerHandle = new();

        Dictionary<Type, List<Func<SObject, __UpdateHandle>>> updateHandlerCreater = new();

        HashSet<__ChangeHandle> changeWaitInvoke = ObjectPool.Get<HashSet<__ChangeHandle>>();
        HashSet<SComponent> changeWaitRemove = new();
        HashSet<SComponent> kvWaitRemove = new();
        Queue<__UpdateHandle> updateHandles = ObjectPool.Get<Queue<__UpdateHandle>>();
        internal Queue<__Timer> timerHandles = ObjectPool.Get<Queue<__Timer>>();

        internal void RigisterHandler(Type type, SObject o)
        {
            {
                if (changeHandle.TryGetValue(type, out var list))
                {
                    for (int i = 0; i < list.Count; i++)
                        list[i].Invoke(o);
                }
            }
            {
                if (o._components.ContainsKey(typeof(KVComponent)))
                {
                    if (kvWatcherHandle.TryGetValue(type, out var list))
                    {
                        for (int i = 0; i < list.Count; i++)
                            list[i].Invoke(o);
                    }
                }
            }
            {
                if (timerHandle.TryGetValue(type, out var list))
                {
                    for (int i = 0; i < list.Count; i++)
                        list[i].Invoke(o);
                }
            }   
            {
                if (updateHandlerCreater.TryGetValue(type, out var list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var v = list[i].Invoke(o);
                        if (v != null)
                            updateHandles.Enqueue(v);
                    }
                }
            }
        }
        internal void Awake(Type type, SObject o)
        {
            if (!awakeHandle.TryGetValue(type, out var list)) return;
            for (int i = 0; i < list.Count; i++)
            {
                try { list[i](o); }
                catch (Exception e) { Loger.Error("Awake 出错 " + e); }
            }
        }
        internal void Dispose(Type type, object o)
        {
            if (!disposeHandle.TryGetValue(type, out var list)) return;
            for (int i = 0; i < list.Count; i++)
            {
                try { list[i](o); }
                catch (Exception e) { Loger.Error("Dispose 出错 " + e); }
            }
        }
        internal void Enable(SComponent c, Type type)
        {
            if (!enableHandle.TryGetValue(type, out var list)) return;
            for (int i = 0; i < list.Count; i++)
            {
                try { list[i](c); }
                catch (Exception e) { Loger.Error("Enable 出错 " + e); }
            }
        }
        internal void AddToChangeWaitInvoke(__ChangeHandle h) => changeWaitInvoke.Add(h);
        internal void AddToChangeWaitRemove(SComponent c) => changeWaitRemove.Add(c);
        internal void AddToKVWaitRemove(SComponent c) => kvWaitRemove.Add(c);
        internal void EventWatcher(long rpc, object e)
        {
            if (!world.ObjectManager.TryGetByRpc(rpc, out var lst))
                return;
            if (!eventWatcherHandle.TryGetValue(e.GetType(), out var acts))
                return;
            for (int i = 0; i < acts.Count; i++)
            {
                int len = lst.Count;
                for (int j = 0; j < len; j++)
                {
                    if (lst[j].Disposed) continue;
                    acts[i].Item2.Invoke(e, lst[j]);
                }
            }
        }
        internal void EventWatcher(object e)
        {
            if (!eventWatcherHandle.TryGetValue(e.GetType(), out var acts))
                return;
            for (int i = 0; i < acts.Count; i++)
            {
                var lst = world.ObjectManager.GetObjectsByComponentType(acts[i].Item1);
                if (lst != null)
                {
                    int len = lst.Count;
                    for (int j = 0; j < len; j++)
                    {
                        if (lst[j].Disposed) continue;
                        acts[i].Item2.Invoke(e, lst[j]);
                    }
                }
            }
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
            eventWatcherHandle.Clear();
            kvWatcherHandle.Clear();
            timerHandle.Clear();

            updateHandlerCreater.Clear();

            changeWaitInvoke.Clear();
            changeWaitRemove.Clear();
            kvWaitRemove.Clear();
            updateHandles.Clear();
            timerHandles.Clear();

            HashSet<Type> awake = new();
            HashSet<Type> dispose = new();
            HashSet<Type> enable = new();
            HashSet<Type> change = new();
            HashSet<Type> eventWatcher = new();
            HashSet<Type> kvWatcher = new();
            HashSet<Type> timer = new();
            Dictionary<Type, List<TimerItem>> timerItems = new();
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
                        change.Add(ma.mainKey);
                        continue;
                    }
                    if (typeof(__EventWatcher).IsAssignableFrom(ma.mainKey))
                    {
                        eventWatcher.Add(ma.mainKey);
                        continue;
                    }
                    if (typeof(__KVWatcher).IsAssignableFrom(ma.mainKey))
                    {
                        kvWatcher.Add(ma.mainKey);
                        continue;
                    }
                    if (typeof(__UpdateHandle).IsAssignableFrom(ma.mainKey))
                    {
                        update.Add(ma.mainKey);
                        continue;
                    }
                }
                if (ma.attribute is TimerAttribute && typeof(__Timer).IsAssignableFrom(ma.mainKey))
                {
                    TimerItem ti = new();
                    ti.attribute = (TimerAttribute)ma.attribute;
                    var ts = ArrayCache.Get<Type>(1);
                    ts[0] = ma.mainKey;
                    ti.action = ma.method.CreateDelegate(typeof(Action<>).MakeGenericType(ts));
                    if (!timerItems.TryGetValue(ma.mainKey, out var lst))
                        timerItems[ma.mainKey] = lst = new();
                    lst.Add(ti);
                    timer.Add(ma.mainKey);
                    continue;
                }
            }
            foreach (var item in awake)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<SObject>)item.GetMethod(nameof(Awake<SObject>.Invoke), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<SObject>));
                if (!awakeHandle.TryGetValue(ts[0], out var lst))
                    awakeHandle[ts[0]] = lst = new();
                lst.Add(action);
            }
            foreach (var item in dispose)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<object>)item.GetMethod(nameof(Dispose<SObject>.Invoke), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<object>));
                if (!disposeHandle.TryGetValue(ts[0], out var lst))
                    disposeHandle[ts[0]] = lst = new();
                lst.Add(action);
            }
            foreach (var item in enable)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<SComponent>)item.GetMethod(nameof(Enable<SComponent>.Invoke), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<SComponent>));
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
            foreach (var item in eventWatcher)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<object, SObject>)item.GetMethod(nameof(EventWatcher<object, SComponent>.Invoke), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<object, SObject>));
                if (!eventWatcherHandle.TryGetValue(ts[0], out var lst))
                    eventWatcherHandle[ts[0]] = lst = new();
                lst.Add((ts[1], action));
                world.ObjectManager.AddEventWatcherFirstType(ts[1]);
            }
            var kvs = kvWatcherHandle[typeof(KVComponent)] = new();
            var kvAction = (Action<SObject>)KVWatcher.TryCreateHandle;
            kvs.Add(kvAction);
            foreach (var item in kvWatcher)
            {
                if (item != typeof(KVWatcher))
                {
                    var action = (Action<SObject>)item.GetMethod(nameof(KVWatcher.TryCreateHandle), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<SObject>));
                    kvs.Add(action);
                    var ts = item.GetGenericArguments();
                    for (int j = 0; j < ts.Length; j++)
                    {
                        var key = ts[j];
                        if (!kvWatcherHandle.TryGetValue(key, out var lst))
                            kvWatcherHandle[key] = lst = new();
                        lst.Add(action);
                    }
                }
            }
            foreach (var item in timer)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<SObject>)item.GetMethod(nameof(Timer<SComponent>.TryCreateHandle), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<SObject>));
                for (int j = 0; j < ts.Length; j++)
                {
                    var key = ts[j];
                    if (!timerHandle.TryGetValue(key, out var lst))
                        timerHandle[key] = lst = new();
                    lst.Add(action);
                }
            }
            var ps = ArrayCache.Get<object>(1);
            foreach (var item in timerItems)
            {
                ps[0] = item.Value;
                item.Key.GetMethod(nameof(Timer<SComponent>.SetTimerList), BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, ps);
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
        internal void update()
        {
            Update.Invoke(world);
            var update = updateHandles;
            updateHandles = ObjectPool.Get<Queue<__UpdateHandle>>();
            while (update.TryDequeue(out var u))
            {
                if (u.Disposed)
                    continue;
                u.Invoke();
                updateHandles.Enqueue(u);
            }
            ObjectPool.Return(update);

            var timer = timerHandles;
            timerHandles = ObjectPool.Get<Queue<__Timer>>();
            while (timer.TryDequeue(out var t))
            {
                if (t.Disposed)
                    continue;
                t.Update();
                timerHandles.Enqueue(t);
            }
            ObjectPool.Return(timer);

            if (changeWaitInvoke.Count > 0)
            {
                var tmp = changeWaitInvoke;
                changeWaitInvoke = ObjectPool.Get<HashSet<__ChangeHandle>>();
                foreach (var c in tmp)
                    c.Invoke();
                tmp.Clear();
                ObjectPool.Return(tmp);
            }
        }
        internal void AfterUpdate()
        {
            if (changeWaitRemove.Count > 0)
            {
                foreach (var c in changeWaitRemove)
                {
                    if (c.Disposed)
                    {
                        c._changeHandles.Clear();
                        ObjectPool.Return(c._changeHandles);
                        c._changeHandles = null;
                    }
                    else
                        c._changeHandles.RemoveAll(t => t.Disposed);
                }
                changeWaitRemove.Clear();
            }
            if (kvWaitRemove.Count > 0)
            {
                foreach (var c in kvWaitRemove)
                {
                    if (c.Disposed)
                    {
                        c._kvWatcherHandles.Clear();
                        ObjectPool.Return(c._kvWatcherHandles);
                        c._kvWatcherHandles = null;
                    }
                    else
                        c._kvWatcherHandles.RemoveAll(t => t.Disposed);
                }
                kvWaitRemove.Clear();
            }
        }
    }
    internal class TimerItem
    {
        public TimerAttribute attribute;
        public Delegate action;
    }
}
