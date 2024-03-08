using Main;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using static UnityEngine.GraphicsBuffer;

namespace Game
{
    public class EventSystem
    {
        readonly Dictionary<Type, EvtQueue> _evtMap = new(97);
        readonly Dictionary<long, Dictionary<Type, EvtQueue>> _rpcEvtMap = new(97);
        Queue<EvtQueue> removed = ObjectPool<Queue<EvtQueue>>.Get();

        [Conditional("DebugEnable")]
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
                    {
                        var ps = method.GetParameters();

                        if (method.IsGenericMethod)
                            Loger.Error("事件函数不能是泛型函数  class:" + method.ReflectedType.FullName + " method:" + method.Name);
                        if (ps.Length == 0 || ps.Length > 2)
                        {
                            Loger.Error("无法解析的参数类型 class:" + method.ReflectedType.FullName + " method:" + method.Name);
                            continue;
                        }
                        if (ps.Length == 2)
                        {
                            if (ps[1].ParameterType != typeof(EventHandler))
                                Loger.Error("无法解析的参数类型 class:" + method.ReflectedType.FullName + " method:" + method.Name);
                        }
                        if (ps[0].ParameterType.IsPrimitive)
                            Loger.Error("不要使用系统值类型作为事件参数类型");
                    }
                }
            }
        }

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        internal void RigisteAllStaticEvent(List<MethodParseData> methods)
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

            var ms = Types.GetInstanceMethodsAttribute(t);
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
            if (!_evtMap.TryGetValue(typeof(T), out var queue))
                _evtMap[typeof(T)] = queue = new();

            EvtData e = new();
            e.action = callBack;
            e.sortOrder = sortOrder;
            e.returnType = typeof(void);
            queue.Add(e);
        }
        public void RigisteEvent(Delegate callBack, int sortOrder = 0)
        {
            if (!callBack.GetType().IsGenericType || callBack.GetType().GetGenericTypeDefinition() == typeof(Func<>))
            {
                Loger.Error("参数错误");
                return;
            }
            Type[] gs = callBack.GetType().GetGenericArguments();
            if (gs.Length != 1)
            {
                Loger.Error("参数错误");
                return;
            }
            Type type = gs[0];
            if (!_evtMap.TryGetValue(type, out var queue))
                _evtMap[type] = queue = new();

            EvtData e = new();
            e.action = callBack;
            e.sortOrder = sortOrder;
            e.returnType = typeof(void);
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

            var ms = Types.GetInstanceMethodsAttribute(t);
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
            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                _rpcEvtMap[rpc] = map = new();
            if (!map.TryGetValue(typeof(T), out var queue))
                map[typeof(T)] = queue = new();

            EvtData e = new();
            e.action = callBack;
            e.sortOrder = sortOrder;
            e.returnType = typeof(void);
            queue.Add(e);
        }
        public void RigisteRPCEvent(long rpc, Delegate callBack, int sortOrder = 0)
        {
            if (!callBack.GetType().IsGenericType || callBack.GetType().GetGenericTypeDefinition() == typeof(Func<>))
            {
                Loger.Error("参数错误");
                return;
            }
            Type[] gs = callBack.GetType().GetGenericArguments();
            if (gs.Length != 1)
            {
                Loger.Error("参数错误");
                return;
            }
            Type type = gs[0];
            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                _rpcEvtMap[rpc] = map = new();
            if (!map.TryGetValue(type, out var queue))
                map[type] = queue = new();

            EvtData e = new();
            e.action = callBack;
            e.sortOrder = sortOrder;
            e.returnType = typeof(void);
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
        }
        public void RemoveEvent<T>(Action<T> callBack)
        {
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
            if (!callBack.GetType().IsGenericType || callBack.GetType().GetGenericTypeDefinition() == typeof(Func<>))
            {
                Loger.Error("参数错误");
                return;
            }
            Type[] gs = callBack.GetType().GetGenericArguments();
            if (gs.Length != 1)
            {
                Loger.Error("参数错误");
                return;
            }
            Type type = gs[0];
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
            var ms = Types.GetInstanceMethodsAttribute(t);
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
            if (!callBack.GetType().IsGenericType || callBack.GetType().GetGenericTypeDefinition() == typeof(Func<>))
            {
                Loger.Error("参数错误");
                return;
            }
            Type[] gs = callBack.GetType().GetGenericArguments();
            if (gs.Length != 1)
            {
                Loger.Error("参数错误");
                return;
            }
            Type type = gs[0];
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
            GameM.Data.Add(data);
            _runEvent(_evtMap, data);
        }
        public STask RunEventAsync<T>(T data)
        {
            GameM.Data.Add(data);
            return _runEventAsync(_evtMap, data);
        }
        public void RunEvent(object data)
        {
            GameM.Data.Add(data);
            _runEvent(_evtMap, data);
        }
        public STask RunEventAsync(object data)
        {
            GameM.Data.Add(data);
            return _runEventAsync(_evtMap, data);
        }
        public STask<K> RunEventAsync<K>(object data)
        {
            GameM.Data.Add(data);
            return _runEventAsync<K>(_evtMap, 0, data);
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
        public async STask<K> RunRPCEventAsync<K>(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return default;
            return await _runEventAsync<K>(map, rpc, data);
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
                removed = ObjectPool<Queue<EvtQueue>>.Get();
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
                ObjectPool<Queue<EvtQueue>>.Return(tmp);
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
        async STask<K> _runEventAsync<K>(Dictionary<Type, EvtQueue> evtMap, long rpc, object data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue))
            {
                if (rpc == 0) Loger.Error($"没有此监听 key={key}");
                else Loger.Error($"没有此监听 key={key} rpc={rpc}");
                return default;
            }
            var task = queue.RunAsync<K>(data);
            if (task == null)
            {
                if (rpc == 0) Loger.Error($"没有匹配的监听 key={key}");
                else Loger.Error($"没有匹配的监听 key={key} rpc={rpc}");
                return default;
            }
            else
                return await task;
        }

        class EvtData
        {
            public EvtData() { }
            public EvtData(MethodParseData m, IEvent target)
            {
                this.returnType = m.method?.ReturnType;
                this.isPropertyOrField = m.property != null || m.field != null;
                this.method = m.method;
                this.field = m.field;
                this.sortOrder = ((EventAttribute)m.attribute).SortOrder;
                this.target = target;
                this.isQueue = ((EventAttribute)m.attribute).Queue;
                this.setHandler = m.parameters != null && m.parameters.Length == 2;

                if (this.returnType == typeof(void))
                {
                    if (this.setHandler)
                    {
                        var ts = ArrayCache<Type>.Get(2);
                        ts[0] = m.mainKey;
                        ts[1] = typeof(EventHandler);
                        this.action = this.method.CreateDelegate(typeof(Action<,>).MakeGenericType(ts), target);
                    }
                    else
                    {
                        var ts = ArrayCache<Type>.Get(1);
                        ts[0] = m.mainKey;
                        this.action = this.method.CreateDelegate(typeof(Action<>).MakeGenericType(ts), target);
                    }
                }
            }

            public bool disposed;

            public Type returnType;
            public bool isPropertyOrField;
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
            public List<EvtData> evts = new List<EvtData>();
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
                    object o = invoke(e, data, eh);
                    if (o is STask t && e.isQueue) await t;
                }
                --counter;
            }
            public async STask RunAsync<T>(T data)
            {
                ++counter;
                List<STask> ts = ObjectPool<List<STask>>.Get();
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt && !eh.isBreak; ++i)
                {
                    EvtData e = evts[i];
                    object o = invoke(e, data, eh);
                    if (o is STask task)
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
                    ObjectPool<List<STask>>.Return(ts);
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
                    object o = invoke(e, data, eh);
                    if (o is STask t && e.isQueue) await t;
                }
                --counter;
            }
            public async STask RunAsync(object data)
            {
                ++counter;
                List<STask> ts = ObjectPool<List<STask>>.Get();
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt; ++i)
                {
                    EvtData e = evts[i];
                    object o = invoke(e, data, eh);
                    if (o is STask task)
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
                    ObjectPool<List<STask>>.Return(ts);
                }
            }
            public async STask<K> RunAsync<K>(object data)
            {
                ++counter;
                bool get = false;
                K ret = default;
                STask<K> task = default;
                int cnt = evts.Count;
                EventHandler eh = new();
                for (int i = 0; i < cnt; ++i)
                {
                    EvtData e = evts[i];
                    object o = invoke(e, data, eh);
                    if (!get && o is K)
                    {
                        get = true;
                        ret = (K)o;
                    }
                    if (!get && o is STask<K>)
                    {
                        get = true;
                        task = (STask<K>)o;
                    }
                    if (o is STask t && e.isQueue) await t;
                }
                --counter;
                if (task != null) ret = await task;
                return ret;
            }

            object invoke<T>(EvtData e, T data, EventHandler eh)
            {
                object o = default;
                if (e.target != null && (e.target.Disposed || !e.target.EventEnable)) return o;
                try
                {
                    if (!e.setHandler)
                    {
                        if (e.returnType == typeof(void))
                            ((Action<T>)e.action).Invoke(data);
                        else
                        {
                            if (e.method != null)
                            {
                                var ps = ArrayCache<object>.Get(1);
                                ps[0] = data;
                                o = e.method.Invoke(e.target, ps);
                            }
                            else
                                e.field.SetValue(e.target, data);
                        }
                    }
                    else
                    {
                        if (e.returnType == typeof(void))
                            ((Action<T, EventHandler>)e.action).Invoke(data, eh);
                        else
                        {
                            var ps = ArrayCache<object>.Get(2);
                            ps[0] = data;
                            ps[1] = eh;
                            o = e.method.Invoke(e.target, ps);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
                if (e.isPropertyOrField && e.target is SComponent c) c.SetChange();
                return o;
            }
            object invoke(EvtData e, object data, EventHandler eh)
            {
                object o = default;
                if (e.target != null && (e.target.Disposed || !e.target.EventEnable)) return o;
                try
                {
                    if (!e.setHandler)
                    {
                        if (e.method != null)
                        {
                            var ps = ArrayCache<object>.Get(1);
                            ps[0] = data;
                            o = e.method.Invoke(e.target, ps);
                        }
                        else if (e.field != null)
                            e.field.SetValue(e.target, data);
                        else
                        {
                            var ps = ArrayCache<object>.Get(1);
                            ps[0] = data;
                            o = e.action.DynamicInvoke(ps);
                        }
                    }
                    else
                    {
                        var ps = ArrayCache<object>.Get(2);
                        ps[0] = data;
                        ps[1] = eh;
                        o = e.method.Invoke(e.target, ps);
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
                if (e.isPropertyOrField && e.target is SComponent c) c.SetChange();
                return o;
            }
        }
    }
}
