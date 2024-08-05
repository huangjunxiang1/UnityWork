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
        internal Dictionary<Type, List<Action<SObject>>> inHandle = new();
        internal Dictionary<Type, List<(Action<Type, SComponent>, Action<SObject, Dictionary<Type, __OutHandle>>)>> outHandle = new();
        internal Dictionary<Type, List<Action<SObject>>> changeHandle = new();
        internal Dictionary<Type, List<(Type, Action<object, SObject, int>)>> eventWatcherHandle = new();
        internal Dictionary<Type, List<Action<SObject>>> kvWatcherHandle = new();
        internal Dictionary<Type, List<Action<SObject>>> timerHandle = new();
        internal Dictionary<Type, List<Func<SObject, __UpdateHandle>>> updateHandlerCreater = new();

        Queue<__ChangeHandle> changeWaitInvoke = ObjectPool.Get<Queue<__ChangeHandle>>();
        HashSet<SComponent> changeWaitRemove = new();
        HashSet<SComponent> kvWaitRemove = new();
        Queue<__UpdateHandle> updateHandles = ObjectPool.Get<Queue<__UpdateHandle>>();

        internal void RigisterHandler(Type type, SComponent c)
        {
            {
                if (changeHandle.TryGetValue(type, out var list))
                {
                    for (int i = 0; i < list.Count; i++)
                        list[i].Invoke(c.Entity);
                }
            }
            {
                if (c.Entity._components.ContainsKey(typeof(KVComponent)))
                {
                    if (kvWatcherHandle.TryGetValue(type, out var list))
                    {
                        for (int i = 0; i < list.Count; i++)
                            list[i].Invoke(c.Entity);
                    }
                }
            }
            {
                if (timerHandle.TryGetValue(type, out var list))
                {
                    for (int i = 0; i < list.Count; i++)
                        list[i].Invoke(c.Entity);
                }
            }
            {
                if (updateHandlerCreater.TryGetValue(type, out var list))
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        var v = list[i].Invoke(c.Entity);
                        if (v != null)
                        {
                            updateHandles.Enqueue(v);
#if UNITY_EDITOR
                            c.Entity._updates.Add(v);
#endif
                        }
                    }
                }
            }
        }
        internal void In(Type type, SObject o)
        {
            if (!inHandle.TryGetValue(type, out var list)) return;
            for (int i = 0; i < list.Count; i++)
            {
                try { list[i](o); }
                catch (Exception e) { Loger.Error("In Error " + e); }
            }
        }
        internal void Out(Type type, SComponent c)
        {
            if (!outHandle.TryGetValue(type, out var list)) return;
            for (int i = 0; i < list.Count; i++)
            {
                try { list[i].Item1(type, c); }
                catch (Exception e) { Loger.Error("Out Error " + e); }
            }
        }
        internal void Out(Type type, SObject o, Dictionary<Type, __OutHandle> record)
        {
            if (!outHandle.TryGetValue(type, out var list)) return;
            for (int i = 0; i < list.Count; i++)
            {
                try { list[i].Item2.Invoke(o, record); }
                catch (Exception e) { Loger.Error("Out Error " + e); }
            }
        }
        internal void AddToChangeWaitInvoke(__ChangeHandle h) => changeWaitInvoke.Enqueue(h);
        internal void AddToChangeWaitRemove(SComponent c) => changeWaitRemove.Add(c);
        internal void AddToKVWaitRemove(SComponent c) => kvWaitRemove.Add(c);
        internal void EventWatcherRpc(long rpc, object e, int type)
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
                    acts[i].Item2.Invoke(e, lst[j], type);
                }
            }
        }
        internal void EventWatcherGid(long gid, object e, int type)
        {
            if (!world.ObjectManager.TryGetByGid(gid, out var o))
                return;
            if (!eventWatcherHandle.TryGetValue(e.GetType(), out var acts))
                return;
            for (int i = 0; i < acts.Count; i++)
                acts[i].Item2.Invoke(e, o, type);
        }
        internal void EventWatcher(object e, int type)
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
                        acts[i].Item2.Invoke(e, lst[j], type);
                    }
                }
            }
        }

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        internal void Load(List<MethodParseData> methods)
        {
            HashSet<Type> aIn = new();
            HashSet<Type> aOut = new();
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
                    if (typeof(__InHandle).IsAssignableFrom(ma.mainKey))
                    {
                        aIn.Add(ma.mainKey);
                        continue;
                    }
                    if (typeof(__OutHandle).IsAssignableFrom(ma.mainKey))
                    {
                        aOut.Add(ma.mainKey);
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
            foreach (var item in aIn)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<SObject>)item.GetMethod(nameof(In<SObject>.Invoke), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<SObject>));
                for (int i = 0; i < ts.Length; i++)
                {
                    if (!inHandle.TryGetValue(ts[i], out var lst))
                        inHandle[ts[i]] = lst = new();
                    lst.Add(action);
                }
            }
            foreach (var item in aOut)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<Type, SComponent>)item.GetMethod(nameof(Out<SObject>.Invoke), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<Type, SComponent>));
                var action2 = (Action<SObject, Dictionary<Type, __OutHandle>>)item.GetMethod(nameof(Out<SObject>.Invoke2), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<SObject, Dictionary<Type, __OutHandle>>));
                for (int i = 0; i < ts.Length; i++)
                {
                    if (!outHandle.TryGetValue(ts[i], out var lst))
                        outHandle[ts[i]] = lst = new();
                    lst.Add((action, action2));
                }
            }
            foreach (var item in change)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<SObject>)item.GetMethod(nameof(AnyChange<SComponent, SComponent>.TryCreateHandle), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<SObject>));
                for (int i = 0; i < ts.Length; i++)
                {
                    if (!changeHandle.TryGetValue(ts[i], out var lst))
                        changeHandle[ts[i]] = lst = new();
                    lst.Add(action);
                }
            }
            foreach (var item in eventWatcher)
            {
                var ts = item.GetGenericArguments();
                var action = (Action<object, SObject, int>)item.GetMethod(nameof(EventWatcher<object, SComponent>.Invoke), BindingFlags.Static | BindingFlags.NonPublic).CreateDelegate(typeof(Action<object, SObject, int>));
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

            if (changeWaitInvoke.Count > 0)
            {
                var change = changeWaitInvoke;
                changeWaitInvoke = ObjectPool.Get<Queue<__ChangeHandle>>();
                while (change.TryDequeue(out var c))
                {
                    if (!c.setInvokeWaiting || c.Disposed) continue;
                    c.setInvokeWaiting = false;
                    c.Invoke();
                }
                ObjectPool.Return(change);
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
