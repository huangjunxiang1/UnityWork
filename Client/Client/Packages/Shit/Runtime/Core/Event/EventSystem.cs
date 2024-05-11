using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Core
{
    public class EventSystem
    {
        internal EventSystem(World world) => this.world = world;

        World world;
        readonly Dictionary<Type, EvtQueue> _evtMap = new(97);
        readonly Dictionary<long, Dictionary<Type, EvtQueue>> _rpcEvtMap = new(97);
        Queue<EvtQueue> removed = ObjectPool.Get<Queue<EvtQueue>>();
        internal Action<object> getEvent;

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        internal void Load(List<MethodParseData> methods)
        {
            _evtMap.Clear();
            _rpcEvtMap.Clear();
            removed.Clear();
            getEvent = null;
            for (int i = 0; i < methods.Count; i++)
            {
                MethodParseData m = methods[i];
                if (m.attribute is EventAttribute ea)
                {
                    if (m.method != null && !m.method.IsStatic) continue;
                    if (!_evtMap.TryGetValue(m.mainKey, out var queue))
                        _evtMap[m.mainKey] = queue = new();

                    EvtData e = new(m, null);
                    queue.Add(e);
                }
            }
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
                Loger.Error("只能在class注册事件");
                return;
            }

            var ms = Types.GetInstanceMethodsAttribute(t);
            for (int i = 0; i < ms.Length; i++)
            {
                var m = ms[i];
                if (m.attribute is EventAttribute ea)
                {
                    if (!_evtMap.TryGetValue(m.mainKey, out var queue))
                        _evtMap[m.mainKey] = queue = new();

                    EvtData e = new(m, target);
                    queue.Add(e);
                }
            }

            if (target.rpc != 0)
            {
                if (!_rpcEvtMap.TryGetValue(target.rpc, out var map))
                    _rpcEvtMap[target.rpc] = map = new();
                for (int i = 0; i < ms.Length; i++)
                {
                    var m = ms[i];
                    if (m.attribute is EventAttribute ea)
                    {
                        if (!map.TryGetValue(m.mainKey, out var queue))
                            map[m.mainKey] = queue = new();

                        EvtData e = new(m, target);
                        queue.Add(e);
                    }
                }
            }
        }
        public void RigisteEvent<T>(Action<T> callBack, int sortOrder = 0)
        {
            Checker.Check(callBack.Method);
            if (!_evtMap.TryGetValue(typeof(T), out var queue))
                _evtMap[typeof(T)] = queue = new();

            EvtData e = new();
            e.action = callBack;
            e.sortOrder = sortOrder;
            queue.Add(e);
        }
        public void RigisteEvent(Delegate callBack, int sortOrder = 0)
        {
            Checker.Check(callBack.Method);
            Type[] gs = callBack.GetType().GetGenericArguments();
            Type type = gs[0];
            if (!_evtMap.TryGetValue(type, out var queue))
                _evtMap[type] = queue = new();

            EvtData e = new();
            e.action = callBack;
            e.sortOrder = sortOrder;
            e.isTask = gs[^1] == typeof(STask);
            e.setHandler = gs.Length >= 2 && gs[1] == typeof(EventHandler);
            queue.Add(e);
        }
        public void RigisteRPCEvent<T>(long rpc, Action<T> callBack, int sortOrder = 0)
        {
            Checker.Check(callBack.Method);
            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                _rpcEvtMap[rpc] = map = new();
            if (!map.TryGetValue(typeof(T), out var queue))
                map[typeof(T)] = queue = new();

            EvtData e = new();
            e.action = callBack;
            e.sortOrder = sortOrder;
            queue.Add(e);
        }
        public void RigisteRPCEvent(long rpc, Delegate callBack, int sortOrder = 0)
        {
            Checker.Check(callBack.Method);
            Type[] gs = callBack.GetType().GetGenericArguments();
            Type type = gs[0];
            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                _rpcEvtMap[rpc] = map = new();
            if (!map.TryGetValue(type, out var queue))
                map[type] = queue = new();

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
                Loger.Error("只能在class注册事件");
                return;
            }
            var ms = Types.GetInstanceMethodsAttribute(t);
            for (int i = 0; i < ms.Length; i++)
            {
                var m = ms[i];
                if (m.attribute is EventAttribute)
                {
                    if (_evtMap.TryGetValue(m.mainKey, out var queue))
                    {
                        if (!queue.addToQueue)
                        {
                            queue.addToQueue = true;
                            removed.Enqueue(queue);
                        }
                    }
                }
            }

            if (target.rpc != 0)
            {
                if (!_rpcEvtMap.TryGetValue(target.rpc, out var map)) return;
                for (int i = 0; i < ms.Length; i++)
                {
                    var m = ms[i];
                    if (m.attribute is EventAttribute)
                    {
                        if (map.TryGetValue(m.mainKey, out var queue))
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
        public void RemoveEvent<T>(Action<T> callBack)
        {
            Checker.Check(callBack.Method);
            if (!_evtMap.TryGetValue(typeof(T), out var queue))
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
        public void RemoveEvent(Delegate callBack)
        {
            Checker.Check(callBack.Method);
            Type type = callBack.GetType().GetGenericArguments()[0];
            if (!_evtMap.TryGetValue(type, out var queue))
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
        public void RemoveRPCEvent<T>(long rpc, Action<T> callBack)
        {
            Checker.Check(callBack.Method);
            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                return;
            if (!map.TryGetValue(typeof(T), out var queue))
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
        public void RemoveRPCEvent(long rpc, Delegate callBack)
        {
            Checker.Check(callBack.Method);
            Type type = callBack.GetType().GetGenericArguments()[0];
            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                return;
            if (!map.TryGetValue(type, out var queue))
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

        public bool HasEvent<T>() => _evtMap.TryGetValue(typeof(T), out var queue) && queue.evts.Count > 0;
        public bool HasEvent(Type type) => _evtMap.TryGetValue(type, out var queue) && queue.evts.Count > 0;

        internal bool GetEventQueue(Type type, out EvtQueue queue)
        {
            return _evtMap.TryGetValue(type, out queue);
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="data"></param>
        public void RunEvent<T>(T data, int testParam = 0)
        {
            getEvent?.Invoke(data);
            world.System.EventWatcher(data);
            _runEvent(_evtMap, data, testParam);
        }
        public STask RunEventAsync<T>(T data, int testParam = 0)
        {
            getEvent?.Invoke(data);
            world.System.EventWatcher(data);
            return _runEventAsync(_evtMap, data, testParam);
        }
        public void RunEvent(object data, int testParam = 0)
        {
            getEvent?.Invoke(data);
            world.System.EventWatcher(data);
            _runEvent(_evtMap, data, testParam);
        }
        public STask RunEventAsync(object data, int testParam = 0)
        {
            getEvent?.Invoke(data);
            world.System.EventWatcher(data);
            return _runEventAsync(_evtMap, data, testParam);
        }
        internal void RunEventNoGCAndFaster<T>(T data, int testParam = 0)
        {
            if (!_evtMap.TryGetValue(data.GetType(), out var queue)) return;
            queue.RunNoGCAndFaster(data, testParam);
        }

        public void RunRPCEvent<T>(long rpc, T data, int testParam = 0)
        {
            world.System.EventWatcher(rpc, data);
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return;
            _runEvent(map, data, testParam);
        }
        public STask RunRPCEventAsync<T>(long rpc, T data, int testParam = 0)
        {
            world.System.EventWatcher(rpc, data);
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return STask.Completed;
            return _runEventAsync(map, data, testParam);
        }
        public void RunRPCEvent(long rpc, object data, int testParam = 0)
        {
            world.System.EventWatcher(rpc, data);
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return;
            _runEvent(map, data, testParam);
        }
        public STask RunRPCEventAsync(long rpc, object data, int testParam = 0)
        {
            world.System.EventWatcher(rpc, data);
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return STask.Completed;
            return _runEventAsync(map, data, testParam);
        }

        public void Clear()
        {
            _evtMap.Clear();
            _rpcEvtMap.Clear();
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

        void _runEvent<T>(Dictionary<Type, EvtQueue> evtMap, T data, int testParam = 0)
        {
            if (!evtMap.TryGetValue(typeof(T), out var queue)) return;
            queue.Run(data, testParam);
        }
        STask _runEventAsync<T>(Dictionary<Type, EvtQueue> evtMap, T data, int testParam = 0)
        {
            if (!evtMap.TryGetValue(typeof(T), out var queue)) return STask.Completed;
            return queue.RunAsync(data, testParam);
        }
        void _runEvent(Dictionary<Type, EvtQueue> evtMap, object data, int testParam = 0)
        {
            if (!evtMap.TryGetValue(data.GetType(), out var queue)) return;
            queue.Run(data, testParam);
        }
        STask _runEventAsync(Dictionary<Type, EvtQueue> evtMap, object data, int testParam = 0)
        {
            if (!evtMap.TryGetValue(data.GetType(), out var queue)) return STask.Completed;
            return queue.RunAsync(data, testParam);
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
            public async void Run<T>(T data, int testParam = 0)
            {
                ++counter;
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt && !eh.isBreak; ++i)
                {
                    EvtData e = evts[i];
                    if (!e.Attribute.Test(testParam)) continue;
                    STask task = invoke(e, data, eh);
                    if (task != null && e.isQueue) await task;
                }
                --counter;
            }
            public async STask RunAsync<T>(T data, int testParam = 0)
            {
                ++counter;
                List<STask> ts = ObjectPool.Get<List<STask>>();
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt && !eh.isBreak; ++i)
                {
                    EvtData e = evts[i];
                    if (!e.Attribute.Test(testParam)) continue;
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
            public async void Run(object data, int testParam = 0)
            {
                ++counter;
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt; ++i)
                {
                    EvtData e = evts[i];
                    if (!e.Attribute.Test(testParam)) continue;
                    STask task = invoke(e, data, eh);
                    if (task != null && e.isQueue) await task;
                }
                --counter;
            }
            public async STask RunAsync(object data, int testParam = 0)
            {
                ++counter;
                List<STask> ts = ObjectPool.Get<List<STask>>();
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt; ++i)
                {
                    EvtData e = evts[i];
                    if (!e.Attribute.Test(testParam)) continue;
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
            public void RunNoGCAndFaster<T>(T data, int testParam = 0)
            {
                ++counter;
                int cnt = evts.Count;
                EventHandler eh = ObjectPool.Get<EventHandler>();
                for (int i = 0; i < cnt && !eh.isBreak; ++i)
                {
                    EvtData e = evts[i];
                    if (!e.Attribute.Test(testParam)) continue;
                    invokeNoGCAndFaster(evts[i], data, eh);
                }
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
                    Loger.Error("事件执行出错 error:" + ex.ToString());
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
                    Loger.Error("事件执行出错 error:" + ex.ToString());
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
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
            }
        }
    }
}
