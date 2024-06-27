using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Core
{
    public class EventSystem
    {
        struct EventKey : IEquatable<EventKey>
        {
            public EventKey(Type k, long rpc = 0, long gid = 0, int type = 0)
            {
                this.keyType = k;
                this.rpc = rpc;
                this.gid = gid;
                this.type = type;
            }

            public Type keyType;
            public long rpc;
            public long gid;
            public int type;

            public bool Equals(EventKey other) => other.keyType == keyType && other.rpc == rpc && other.gid == gid && other.type == type;
            public override int GetHashCode() => keyType.GetHashCode() ^ rpc.GetHashCode() ^ gid.GetHashCode() ^ type;
            public override string ToString() => $"key={keyType} rpc={rpc} gid={gid} type={type}";
        }
        internal EventSystem(World world) => this.world = world;

        World world;
        readonly Dictionary<EventKey, EvtQueue> _evtMap = new(97);
        Queue<EvtQueue> removed = ObjectPool.Get<Queue<EvtQueue>>();
        internal Action<object> getEvent;
        GenericEventHelper GenericEvent;

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        internal void Load(List<MethodParseData> methods)
        {
            _evtMap.Clear();
            removed.Clear();
            getEvent = null;
            for (int i = 0; i < methods.Count; i++)
            {
                MethodParseData m = methods[i];
                if (m.attribute is EventAttribute ea)
                {
                    if (m.method != null && !m.method.IsStatic) continue;
                    EventKey k = new(m.mainKey, 0, 0, ea.Type);
                    if (!_evtMap.TryGetValue(k, out var queue))
                        _evtMap[k] = queue = new();

                    EvtData e = new(m, null);
                    queue.Add(e);
                }
            }
            GenericEvent = new(methods, this);
        }

        /// <summary>
        /// 注册消息和事件监听
        /// </summary>
        /// <param name="target"></param>
        public void RigisteEvent(IEvent target)
        {
            if (target == null)
            {
                Loger.Error("添加事件对象为空");
                return;
            }
            Type t = target.GetType();
            if (!t.IsClass)
            {
                Loger.Error("Only rigister event in class");
                return;
            }

            var ms = Types.GetInstanceMethodsAttribute(t);
            for (int i = 0; i < ms.Length; i++)
            {
                var m = ms[i];
                if (m.attribute is EventAttribute ea)
                {
                    EvtData e = new(m, target);
                    {
                        EventKey k = new(m.mainKey, 0, 0, ea.Type);
                        if (!_evtMap.TryGetValue(k, out var queue))
                            _evtMap[k] = queue = new();
                        queue.Add(e);
                    }
                    if (target.rpc != 0)
                    {
                        EventKey k = new(m.mainKey, target.rpc, 0, ea.Type);
                        if (!_evtMap.TryGetValue(k, out var queue))
                            _evtMap[k] = queue = new();
                        queue.Add(e);
                    }
                    {
                        EventKey k = new(m.mainKey, 0, target.gid, ea.Type);
                        if (!_evtMap.TryGetValue(k, out var queue))
                            _evtMap[k] = queue = new();
                        queue.Add(e);
                    }
                }
            }
        }
        public void RigisteEvent<T>(Action<T> callBack, long rpc = 0, int sortOrder = 0)
        {
            Checker.Check(callBack.Method);
            EventKey k = new(typeof(T), rpc);
            if (!_evtMap.TryGetValue(k, out var queue))
                _evtMap[k] = queue = new();

            EvtData e = new();
            e.action = callBack;
            e.sortOrder = sortOrder;
            queue.Add(e);
        }
        public void RigisteEvent(Delegate callBack, long rpc = 0, int sortOrder = 0)
        {
            Checker.Check(callBack.Method);
            Type[] gs = callBack.GetType().GetGenericArguments();
            EventKey k = new(gs[0], rpc);
            if (!_evtMap.TryGetValue(k, out var queue))
                _evtMap[k] = queue = new();

            EvtData e = new();
            e.action = callBack;
            e.sortOrder = sortOrder;
            e.isTask = gs[^1] == typeof(STask);
            e.setHandler = gs.Length >= 2 && gs[1] == typeof(EventHandler);
            queue.Add(e);
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="target"></param>
        public void RemoveEvent(IEvent target)
        {
            if (target == null)
            {
                Loger.Error("移除事件对象为空");
                return;
            }
            Type t = target.GetType();
            if (!t.IsClass)
            {
                Loger.Error("Only rigister event in class");
                return;
            }
            var ms = Types.GetInstanceMethodsAttribute(t);
            for (int i = 0; i < ms.Length; i++)
            {
                var m = ms[i];
                if (m.attribute is EventAttribute ea)
                {
                    {
                        if (_evtMap.TryGetValue(new EventKey(m.mainKey, 0, 0, ea.Type), out var queue))
                        {
                            if (!queue.addToQueue)
                            {
                                queue.addToQueue = true;
                                removed.Enqueue(queue);
                            }
                        }
                    }
                    if (target.rpc != 0)
                    {
                        if (_evtMap.TryGetValue(new EventKey(m.mainKey, target.rpc, 0, ea.Type), out var queue))
                        {
                            if (!queue.addToQueue)
                            {
                                queue.addToQueue = true;
                                removed.Enqueue(queue);
                            }
                        }
                    }
                    {
                        if (_evtMap.TryGetValue(new EventKey(m.mainKey, 0, target.gid, ea.Type), out var queue))
                        {
                            if (!queue.addToQueue)
                            {
                                queue.addToQueue = true;
                                removed.Enqueue(queue);
                            }
                        }
                    }
                }
            }
        }
        public void RemoveEvent<T>(Action<T> callBack, long rpc = 0)
        {
            Checker.Check(callBack.Method);
            if (!_evtMap.TryGetValue(new EventKey(typeof(T), rpc), out var queue))
                return;

            if (queue.Remove(callBack))
            {
                if (!queue.addToQueue)
                {
                    queue.addToQueue = true;
                    removed.Enqueue(queue);
                }
            }
        }
        public void RemoveEvent(Delegate callBack, long rpc = 0)
        {
            Checker.Check(callBack.Method);
            if (!_evtMap.TryGetValue(new EventKey(callBack.GetType().GetGenericArguments()[0], rpc), out var queue))
                return;

            if (queue.Remove(callBack))
            {
                if (!queue.addToQueue)
                {
                    queue.addToQueue = true;
                    removed.Enqueue(queue);
                }
            }
        }

        public bool HasEvent<T>() => _evtMap.TryGetValue(new EventKey(typeof(T)), out var queue) && queue.evts.Count > 0;
        public bool HasEvent(Type type) => _evtMap.TryGetValue(new EventKey(type), out var queue) && queue.evts.Count > 0;

        internal bool GetEventQueue(Type type, out List<EvtData> es)
        {
            if (_evtMap.TryGetValue(new EventKey(type), out var queue))
            {
                es = queue.evts.FindAll(t => !t.disposed && (t.target == null || !t.target.Disposed));
                return true;
            }
            else
            {
                es = new List<EvtData>();   
                return false;
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="data"></param>
        public void RunEvent<T>(T data, long rpc = 0, long gid = 0, int type = 0)
        {
#if DebugEnable
            if (rpc != 0 && gid != 0)
                Loger.Error($"rpc 和 gid  不可同时为有效值");
#endif
            if (rpc != 0)
            {
                world.System.EventWatcherRpc(rpc, data);
                if (_evtMap.TryGetValue(new EventKey(typeof(T), rpc, 0, type), out var queue))
                    queue.Run(data);
            }
            else if (gid != 0)
            {
                world.System.EventWatcherGid(gid, data);
                if (_evtMap.TryGetValue(new EventKey(typeof(T), 0, gid, type), out var queue))
                    queue.Run(data);
            }
            else
            {
                getEvent?.Invoke(data);
                world.System.EventWatcher(data);
                if (_evtMap.TryGetValue(new EventKey(typeof(T), 0, 0, type), out var queue))
                    queue.Run(data);
            }
        }
        public STask RunEventAsync<T>(T data, long rpc = 0, long gid = 0, int type = 0)
        {
#if DebugEnable
            if (rpc != 0 && gid != 0)
                Loger.Error($"rpc 和 gid  不可同时为有效值");
#endif
            if (rpc != 0)
            {
                world.System.EventWatcherRpc(rpc, data);
                if (_evtMap.TryGetValue(new EventKey(typeof(T), rpc, 0, type), out var queue))
                    return queue.RunAsync(data);
                else return STask.Completed;
            }
            else if (gid != 0)
            {
                world.System.EventWatcherGid(gid, data);
                if (_evtMap.TryGetValue(new EventKey(typeof(T), 0, gid, type), out var queue))
                    return queue.RunAsync(data);
                else return STask.Completed;
            }
            else
            {
                getEvent?.Invoke(data);
                world.System.EventWatcher(data);
                if (_evtMap.TryGetValue(new EventKey(typeof(T), 0, 0, type), out var queue))
                    return queue.RunAsync(data);
                else return STask.Completed;
            }
        }
        public void RunEvent(object data, long rpc = 0, long gid = 0, int type = 0)
        {
#if DebugEnable
            if (rpc != 0 && gid != 0)
                Loger.Error($"rpc 和 gid  不可同时为有效值");
#endif
            if (rpc != 0)
            {
                world.System.EventWatcherRpc(rpc, data);
                if (_evtMap.TryGetValue(new EventKey(data.GetType(), rpc, 0, type), out var queue))
                    queue.Run(data);
            }
            else if (gid != 0)
            {
                world.System.EventWatcherGid(gid, data);
                if (_evtMap.TryGetValue(new EventKey(data.GetType(), 0, gid, type), out var queue))
                    queue.Run(data);
            }
            else
            {
                getEvent?.Invoke(data);
                world.System.EventWatcher(data);
                if (_evtMap.TryGetValue(new EventKey(data.GetType(), 0, 0, type), out var queue))
                    queue.Run(data);
            }
        }
        public STask RunEventAsync(object data, long rpc = 0, long gid = 0, int type = 0)
        {
#if DebugEnable
            if (rpc != 0 && gid != 0)
                Loger.Error($"rpc 和 gid  不可同时为有效值");
#endif
            if (rpc != 0)
            {
                world.System.EventWatcherRpc(rpc, data);
                if (_evtMap.TryGetValue(new EventKey(data.GetType(), rpc, 0, type), out var queue))
                    return queue.RunAsync(data);
                else return STask.Completed;
            }
            else if (gid != 0)
            {
                world.System.EventWatcherGid(gid, data);
                if (_evtMap.TryGetValue(new EventKey(data.GetType(), 0, gid, type), out var queue))
                    return queue.RunAsync(data);
                else return STask.Completed;
            }
            else
            {
                getEvent?.Invoke(data);
                world.System.EventWatcher(data);
                if (_evtMap.TryGetValue(new EventKey(data.GetType(), 0, 0, type), out var queue))
                    return queue.RunAsync(data);
                else return STask.Completed;
            }
        }

        public void RunGenericEvent(Type baseType, object o, Type elementType = null) => GenericEvent.Invoke(baseType, o, elementType);
        public void RunGenericEventAndBaseType(Type baseType, object o) => GenericEvent.InvokeAndBaseType(baseType, o);
        internal void RunEventNoGCAndFaster<T>(T data, int type = 0)
        {
            if (!_evtMap.TryGetValue(new EventKey(data.GetType(), 0, 0, type), out var queue)) return;
            queue.RunNoGCAndFaster(data);
        }

        public void Clear()
        {
            _evtMap.Clear();
        }

        internal void AfterUpdate()
        {
            if (removed.Count > 0)
            {
                var tmp = removed;
                removed = ObjectPool.Get<Queue<EvtQueue>>();
                while (tmp.TryDequeue(out var queue))
                {
                    if (queue.counter != 0)
                    {
                        removed.Enqueue(queue);
                        continue;
                    }
                    queue.RemoveAll();
                    queue.addToQueue = false;
                }
                ObjectPool.Return(tmp);
            }
        }

        internal class EvtData
        {
            public EvtData()
            {
                this.Attribute = EventAttribute.Default;
            }
            public EvtData(MethodParseData m, IEvent target)
            {
                this.Attribute = (EventAttribute)m.attribute;
                this.isTask = m.method?.ReturnType == typeof(STask);
                this.isProperty = m.property != null;
                this.isField = m.field != null;
                this.method = m.method;
                this.field = m.field;
                this.sortOrder = this.Attribute.SortOrder;
                this.target = target;
                this.isQueue = this.Attribute.Queue;
                this.setHandler = m.parameters != null && m.parameters.Length == 2;

                if (!this.isTask)
                {
                    if (!this.isField)
                    {
                        if (this.setHandler)
                        {
                            var ts = ArrayCache.Get<Type>(2);
                            ts[0] = m.mainKey;
                            ts[1] = typeof(EventHandler);
                            this.action = this.method.CreateDelegate(typeof(Action<,>).MakeGenericType(ts), target);
                        }
                        else
                        {
                            var ts = ArrayCache.Get<Type>(1);
                            ts[0] = m.mainKey;
                            this.action = this.method.CreateDelegate(typeof(Action<>).MakeGenericType(ts), target);
                        }
                    }
                }
                else
                {
                    if (this.setHandler)
                    {
                        var ts = ArrayCache.Get<Type>(3);
                        ts[0] = m.mainKey;
                        ts[1] = typeof(EventHandler);
                        ts[2] = typeof(STask);
                        this.action = this.method.CreateDelegate(typeof(Func<,,>).MakeGenericType(ts), target);
                    }
                    else
                    {
                        var ts = ArrayCache.Get<Type>(2);
                        ts[0] = m.mainKey;
                        ts[1] = typeof(STask);
                        this.action = this.method.CreateDelegate(typeof(Func<,>).MakeGenericType(ts), target);
                    }
                }
            }

            public EventAttribute Attribute;
            public bool disposed;

            public bool isTask;
            public bool isProperty;
            public bool isField;
            public MethodInfo method;
            public FieldInfo field;
            public int sortOrder;
            public IEvent target;
            public bool isQueue;
            public bool setHandler;

            public Delegate action;
        }
        internal class EvtQueue
        {
            public List<EvtData> evts = new();
            public int counter = 0;
            public bool addToQueue = false;

            public void Add(EvtData evt)
            {
                int inserIdx = evts.FindIndex(t => t.sortOrder > evt.sortOrder);
                if (inserIdx != -1)
                    evts.Insert(inserIdx, evt);
                else
                    evts.Add(evt);
            }
            public bool Remove(Delegate callBack)
            {
                for (int i = 0; i < evts.Count; i++)
                {
                    if (evts[i].action == callBack)
                    {
                        evts[i].disposed = true;
                        return true;
                    }
                }
                return false;
            }
            public void RemoveAll() => evts.RemoveAll(t => t.disposed || (t.target != null && t.target.Disposed));
            public async void Run<T>(T data)
            {
                ++counter;
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt && !eh.isBreak; ++i)
                {
                    EvtData e = evts[i];
                    STask task = invoke(e, data, eh);
                    if (task != null && e.isQueue) await task;
                }
                --counter;
            }
            public async STask RunAsync<T>(T data)
            {
                ++counter;
                List<STask> ts = ObjectPool.Get<List<STask>>();
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt && !eh.isBreak; ++i)
                {
                    EvtData e = evts[i];
                    STask task = invoke(e, data, eh);
                    if (task != null)
                    {
                        if (e.isQueue) await task;
                        else ts.Add(task);
                    }
                }
                --counter;
                if (ts.Count > 0)
                {
                    await STask.All(ts, false);
                    ts.Clear();
                    ObjectPool.Return(ts);
                }
            }
            public async void Run(object data)
            {
                ++counter;
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt; ++i)
                {
                    EvtData e = evts[i];
                    STask task = invoke(e, data, eh);
                    if (task != null && e.isQueue) await task;
                }
                --counter;
            }
            public async STask RunAsync(object data)
            {
                ++counter;
                List<STask> ts = ObjectPool.Get<List<STask>>();
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt; ++i)
                {
                    EvtData e = evts[i];
                    STask task = invoke(e, data, eh);
                    if (task != null)
                    {
                        if (e.isQueue) await task;
                        else ts.Add(task);
                    }
                }
                --counter;
                if (ts.Count > 0)
                {
                    await STask.All(ts, false);
                    ts.Clear();
                    ObjectPool.Return(ts);
                }
            }
            public void RunNoGCAndFaster<T>(T data)
            {
                ++counter;
                int cnt = evts.Count;
                EventHandler eh = ObjectPool.Get<EventHandler>();
                for (int i = 0; i < cnt && !eh.isBreak; ++i)
                    invokeNoGCAndFaster(evts[i], data, eh);
                eh.Reset();
                ObjectPool.Return(eh);
                --counter;
            }

            STask invoke<T>(EvtData e, T data, EventHandler eh)
            {
                if (e.target != null && (e.target.Disposed || !e.target.EventEnable)) return default;
                try
                {
                    if (!e.setHandler)
                    {
                        if (e.isTask)
                            return ((Func<T, STask>)e.action).Invoke(data);
                        else
                        {
                            if (e.isField)
                                e.field.SetValue(e.target, data);
                            else
                                ((Action<T>)e.action).Invoke(data);
                        }
                    }
                    else
                    {
                        if (e.isTask)
                            return ((Func<T, EventHandler, STask>)e.action).Invoke(data, eh);
                        else
                            ((Action<T, EventHandler>)e.action).Invoke(data, eh);
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error("Event Execute Error :" + ex.ToString());
                }
                e.target?.AcceptedEvent();
                return default;
            }
            STask invoke(EvtData e, object data, EventHandler eh)
            {
                STask task = default;
                if (e.target != null && (e.target.Disposed || !e.target.EventEnable)) return task;
                try
                {
                    if (!e.setHandler)
                    {
                        if (e.method != null)
                        {
                            var ps = ArrayCache.Get<object>(1);
                            ps[0] = data;
                            task = e.method.Invoke(e.target, ps) as STask;
                        }
                        else if (e.isField)
                            e.field.SetValue(e.target, data);
                        else
                        {
                            var ps = ArrayCache.Get<object>(1);
                            ps[0] = data;
                            task = e.action.DynamicInvoke(ps) as STask;
                        }
                    }
                    else
                    {
                        var ps = ArrayCache.Get<object>(2);
                        ps[0] = data;
                        ps[1] = eh;
                        task = e.method.Invoke(e.target, ps) as STask;
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error("Event Execute Error :" + ex.ToString());
                }
                e.target?.AcceptedEvent();
                return task;
            }
            void invokeNoGCAndFaster<T>(EvtData e, T data, EventHandler eh)
            {
                if (e.target != null && (e.target.Disposed || !e.target.EventEnable)) return;
                try
                {
                    if (!e.setHandler)
                        ((Action<T>)e.action).Invoke(data);
                    else
                        ((Action<T, EventHandler>)e.action).Invoke(data, eh);
                }
                catch (Exception ex)
                {
                    Loger.Error("Event Execute Error :" + ex.ToString());
                }
            }
        }
    }
}
