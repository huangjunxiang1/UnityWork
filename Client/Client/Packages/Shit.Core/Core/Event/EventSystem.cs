using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Core
{
    public class EventSystem
    {
        readonly Dictionary<Type, EvtQueue> _evtMap = new(97);
        readonly Dictionary<long, Dictionary<Type, EvtQueue>> _rpcEvtMap = new(97);
        Queue<EvtQueue> removed = ObjectPool.Get<Queue<EvtQueue>>();
        public Action<object> getEvent;

        [Conditional(ConstDefCore.DebugEnableString)]
        internal static void Check(List<Type> types)
        {
            for (int i = 0; i < types.Count; i++)
            {
                var type = types[i];
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

                for (int j = 0; j < methods.Length; j++)
                {
                    var method = methods[j];
                    if (method.IsDefined(typeof(EventAttribute)))
                        Check(method);
                }
            }
        }
        [Conditional(ConstDefCore.DebugEnableString)]
        static void Check(MethodInfo method)
        {
            var ps = method.GetParameters();

            if (method.IsGenericMethod)
                Loger.Error($"事件函数不能是泛型函数  class:{method.ReflectedType.FullName} method:{method.Name}");
            if (ps.Length == 0 || ps.Length > 2)
                Loger.Error($"无法解析的参数类型 class:{method.ReflectedType.FullName} method:{method.Name}");
            if (ps.Length == 2)
            {
                if (ps[1].ParameterType != typeof(EventHandler))
                    Loger.Error($"无法解析的参数类型 class:{method.ReflectedType.FullName} method:{method.Name}");
            }
            if (ps[0].ParameterType.IsPrimitive)
                Loger.Error($"不要使用系统值类型作为事件参数类型  class:{method.ReflectedType.FullName} method:{method.Name}");
            if (method.ReturnType != typeof(void) && method.ReturnType != typeof(STask))
                Loger.Error($"事件函数的返回类型只能是void或者{nameof(STask)} class:{method.ReflectedType.FullName} method:{method.Name}");
        }

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        public void RigisteAllStaticEvent(List<MethodParseData> methods)
        {
            for (int i = 0; i < methods.Count; i++)
            {
                MethodParseData m = methods[i];
                if (m.attribute is EventAttribute ea)
                {
                    if (m.method != null && !m.method.IsStatic) continue;
                    if (ea.RPC)
                    {
                        Loger.Error("rpc事件不支持静态");
                        continue;
                    }

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

            var ms = CoreTypes.GetInstanceMethodsAttribute(t);
            for (int i = 0; i < ms.Length; i++)
            {
                var m = ms[i];
                if (m.attribute is EventAttribute ea)
                {
                    if (ea.RPC) continue;

                    if (!_evtMap.TryGetValue(m.mainKey, out var queue))
                        _evtMap[m.mainKey] = queue = new();

                    EvtData e = new(m, target);
                    queue.Add(e);
                }
            }
        }
        public void RigisteEvent<T>(Action<T> callBack, int sortOrder = 0)
        {
            Check(callBack.Method);
            if (!_evtMap.TryGetValue(typeof(T), out var queue))
                _evtMap[typeof(T)] = queue = new();

            EvtData e = new();
            e.action = callBack;
            e.sortOrder = sortOrder;
            queue.Add(e);
        }
        public void RigisteEvent(Delegate callBack, int sortOrder = 0)
        {
            Check(callBack.Method);
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
        public void RigisteRPCEvent(long rpc, IEvent target)
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

            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                _rpcEvtMap[rpc] = map = new();

            var ms = CoreTypes.GetInstanceMethodsAttribute(t);
            for (int i = 0; i < ms.Length; i++)
            {
                var m = ms[i];
                if (m.attribute is EventAttribute ea)
                {
                    if (!ea.RPC) continue;

                    if (!map.TryGetValue(m.mainKey, out var queue))
                        map[m.mainKey] = queue = new();

                    EvtData e = new(m, target);
                    queue.Add(e);
                }
            }
        }
        public void RigisteRPCEvent<T>(long rpc, Action<T> callBack, int sortOrder = 0)
        {
            Check(callBack.Method);
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
            Check(callBack.Method);
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
            var ms = CoreTypes.GetInstanceMethodsAttribute(t);
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
        }
        public void RemoveEvent<T>(Action<T> callBack)
        {
            Check(callBack.Method);
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
            Check(callBack.Method);
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
        public void RemoveRPCEvent(long rpc, IEvent target)
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
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return;
            var ms = CoreTypes.GetInstanceMethodsAttribute(t);
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
        public void RemoveRPCEvent<T>(long rpc, Action<T> callBack)
        {
            Check(callBack.Method);
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
            Check(callBack.Method);
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

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="data"></param>
        public void RunEvent<T>(T data)
        {
            getEvent?.Invoke(data);
            _runEvent(_evtMap, data);
        }
        public STask RunEventAsync<T>(T data)
        {
            getEvent?.Invoke(data);
            return _runEventAsync(_evtMap, data);
        }
        public void RunEvent(object data)
        {
            getEvent?.Invoke(data);
            _runEvent(_evtMap, data);
        }
        public STask RunEventAsync(object data)
        {
            getEvent?.Invoke(data);
            return _runEventAsync(_evtMap, data);
        }
        internal void RunEventNoGC<T>(T data)
        {
            _runEventNoGC(_evtMap, data);
        }

        public void RunRPCEvent<T>(long rpc, T data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return;
            _runEvent(map, data);
        }
        public STask RunRPCEventAsync<T>(long rpc, T data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return STask.Completed;
            return _runEventAsync(map, data);
        }
        public void RunRPCEvent(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return;
            _runEvent(map, data);
        }
        public STask RunRPCEventAsync(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return STask.Completed;
            return _runEventAsync(map, data);
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

        void _runEvent<T>(Dictionary<Type, EvtQueue> evtMap, T data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue)) return;
            queue.Run(data);
        }
        STask _runEventAsync<T>(Dictionary<Type, EvtQueue> evtMap, T data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue)) return STask.Completed;
            return queue.RunAsync(data);
        }
        void _runEvent(Dictionary<Type, EvtQueue> evtMap, object data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue)) return;
            queue.Run(data);
        }
        STask _runEventAsync(Dictionary<Type, EvtQueue> evtMap, object data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue)) return STask.Completed;
            return queue.RunAsync(data);
        }
        void _runEventNoGC<T>(Dictionary<Type, EvtQueue> evtMap, T data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue)) return;
            queue.RunNoGC(data);
        }

        class EvtData
        {
            public EvtData() { }
            public EvtData(MethodParseData m, IEvent target)
            {
                this.isTask = m.method?.ReturnType == typeof(STask);
                this.isProperty = m.property != null;
                this.isField = m.field != null;
                this.method = m.method;
                this.field = m.field;
                this.sortOrder = ((EventAttribute)m.attribute).SortOrder;
                this.target = target;
                this.isQueue = ((EventAttribute)m.attribute).Queue;
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
        class EvtQueue
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
            public void RunNoGC<T>(T data)
            {
                ++counter;
                int cnt = evts.Count;
                EventHandler eh = ObjectPool.Get<EventHandler>();
                for (int i = 0; i < cnt && !eh.isBreak; ++i)
                {
                    EvtData e = evts[i];
                    invokeNoGC(e, data, eh);
                }
                eh.Reset();
                ObjectPool.Return(eh);
                --counter;
            }

            STask invoke<T>(EvtData e, T data, EventHandler eh)
            {
                if (e.target != null && (e.target.Disposed || !e.target.EventEnable)) return null;
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
                e.target?.AcceptEventHandler(!e.isField && !e.isProperty);
                return null;
            }
            STask invoke(EvtData e, object data, EventHandler eh)
            {
                STask task = default;
                if (e.target != null && (e.target.Disposed || !e.target.EventEnable)) return null;
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
                e.target?.AcceptEventHandler(!e.isField && !e.isProperty);
                return task;
            }
            void invokeNoGC<T>(EvtData e, T data, EventHandler eh)
            {
                if (e.target != null && (e.target.Disposed || !e.target.EventEnable)) return;
                try
                {
                    if (!e.setHandler)
                    {
                        if (e.isTask)
                            ((Func<T, STask>)e.action).Invoke(data);
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
                            ((Func<T, EventHandler, STask>)e.action).Invoke(data, eh);
                        else
                            ((Action<T, EventHandler>)e.action).Invoke(data, eh);
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
                e.target?.AcceptEventHandler(!e.isField && !e.isProperty);
            }
        }
    }
}
