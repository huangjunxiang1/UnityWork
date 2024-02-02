using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Game
{
    public class EventSystem
    {
        readonly Dictionary<Type, EvtQueue> _evtMap = new(97);
        readonly Dictionary<long, Dictionary<Type, EvtQueue>> _rpcEvtMap = new(97);

        [Conditional("DebugEnable")]
        static void _checkAllMethod()
        {
            for (int i = 0; i < Types.AllTypes.Count; i++)
            {
                var type = Types.AllTypes[i];
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
            _checkAllMethod();
            for (int i = 0; i < methods.Count; i++)
            {
                MethodParseData m = methods[i];
                if (m.attribute is EventAttribute ea)
                {
                    if (ea.RPC)
                    {
                        Loger.Error("rpc事件不支持静态");
                        continue;
                    }

                    EvtData e = new();
                    e.key = m.mainKey;
                    e.isPropertyOrField = m.property != null || m.field != null;
                    if (!_evtMap.TryGetValue(e.key, out var queue))
                        _evtMap[e.key] = queue = new();

                    e.sortOrder = ea.SortOrder;
                    e.isQueue = ea.Queue;
                    e.method = m.method;
                    e.field = m.field;
                    e.setHandler = m.parameters != null && m.parameters.Length == 2;
                    e.returnType = m.method?.ReturnType;
                    createDelegate(e, null);

                    queue.Add(e);
                }
            }
        }

        /// <summary>
        /// 注册消息和事件监听
        /// </summary>
        /// <param name="target"></param>
        public void RigisteEvent(object target)
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

                    Type key = m.mainKey;
                    if (!_evtMap.TryGetValue(key, out var queue))
                        _evtMap[key] = queue = new();

                    EvtData e = new();
                    e.key = key;
                    e.method = m.method;
                    e.field = m.field;
                    e.isPropertyOrField = m.property != null || m.field != null;
                    e.sortOrder = ea.SortOrder;
                    e.target = target;
                    e.isQueue = ea.Queue;
                    e.setHandler = m.parameters != null && m.parameters.Length == 2;
                    e.returnType = m.method?.ReturnType;
                    createDelegate(e, target);

                    queue.Add(e);
                }
            }
        }
        public void RigisteRPCEvent(long rpc, object target)
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

                    Type key = m.mainKey;
                    if (!map.TryGetValue(key, out var queue))
                        map[key] = queue = new();

                    EvtData e = new();
                    e.key = key;
                    e.method = m.method;
                    e.field = m.field;
                    e.isPropertyOrField = m.property != null || m.field != null;
                    e.sortOrder = ea.SortOrder;
                    e.target = target;
                    e.isQueue = ea.Queue;
                    e.setHandler = m.parameters != null && m.parameters.Length == 2;
                    e.returnType = m.method?.ReturnType;
                    createDelegate(e, target);

                    queue.Add(e);
                }
            }
        }
        public STask<T> WaitEvent<T>(int sortOrder = 0)
        {
            STask<T> task = new();
            var k = typeof(T);

            if (!_evtMap.TryGetValue(k, out var queue))
                _evtMap[k] = queue = new();

            EvtData e = new();
            e.key = k;
            e.sortOrder = sortOrder;
            e.isOnece = true;
            e.target = task;

            queue.Add(e);
            return task;
        }
        public STask<T> WaitRPCEvent<T>(long rpc, int sortOrder = 0)
        {
            STask<T> task = new();
            var k = typeof(T);

            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                _rpcEvtMap[rpc] = map = new();

            if (!map.TryGetValue(k, out var queue))
                map[k] = queue = new();

            EvtData e = new();
            e.key = k;
            e.sortOrder = sortOrder;
            e.isOnece = true;
            e.target = task;

            queue.Add(e);
            return task;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="target"></param>
        public void RemoveEvent(object target)
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
                        queue.Remove(target);
                }
            }
        }
        public void RemoveRPCEvent(long rpc, object target)
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
                        queue.Remove(target);
                }
            }
        }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="data"></param>
        public void RunEvent<T>(T data)
        {
            _runEvent(_evtMap, 0, data);
        }
        public STask RunEventAsync<T>(T data)
        {
            return _runEventAsync(_evtMap, 0, data);
        }
        public void RunEvent(object data)
        {
            _runEvent(_evtMap, 0, data);
        }
        public STask RunEventAsync(object data)
        {
            return _runEventAsync(_evtMap, 0, data);
        }
        public STask<K> RunEventAsync<K>(object data)
        {
            return _runEventAsync<K>(_evtMap, 0, data);
        }

        public void RunRPCEvent<T>(long rpc, T data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return;
            _runEvent(map, rpc, data);
        }
        public STask RunRPCEventAsync<T>(long rpc, T data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return STask.Completed;
            return _runEventAsync(map, rpc, data);
        }
        public void RunRPCEvent(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return;
            _runEvent(map, rpc, data);
        }
        public STask RunRPCEventAsync(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return STask.Completed;
            return _runEventAsync(map, rpc, data);
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

        void _runEvent<T>(Dictionary<Type, EvtQueue> evtMap, long rpc, T data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue)) return;
            if (queue.index > -1)
            {
                logErrorIfEventLoop(rpc, key, queue);
                return;
            }
            queue.Run(data);
        }
        STask _runEventAsync<T>(Dictionary<Type, EvtQueue> evtMap, long rpc, T data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue)) return STask.Completed;
            if (queue.index > -1)
            {
                logErrorIfEventLoop(rpc, key, queue);
                return STask.Completed;
            }
            return queue.RunAsync(data);
        }
        void _runEvent(Dictionary<Type, EvtQueue> evtMap, long rpc, object data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue)) return;
            if (queue.index > -1)
            {
                logErrorIfEventLoop(rpc, key, queue);
                return;
            }
            queue.Run(data);
        }
        STask _runEventAsync(Dictionary<Type, EvtQueue> evtMap, long rpc, object data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue)) return STask.Completed;
            if (queue.index > -1)
            {
                logErrorIfEventLoop(rpc, key, queue);
                return STask.Completed;
            }
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
            if (queue.index > -1)
            {
                logErrorIfEventLoop(rpc, key, queue);
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
        void createDelegate(EvtData ed, object o)
        {
            if (ed.returnType == typeof(void))
            {
                if (ed.setHandler)
                {
                    var ts = ArrayCache<Type>.Get(2);
                    ts[0] = ed.key;
                    ts[1] = typeof(EventHandler);
                    ed.action = ed.method.CreateDelegate(typeof(Action<,>).MakeGenericType(ts), o);
                }
                else
                    ed.action = ed.method.CreateDelegate(Types.GetGenericType(typeof(Action<>), ed.key), o);
            }
        }
        [Conditional("DebugEnable")]
        void logErrorIfEventLoop(long rpc, Type key, EvtQueue queue)
        {
            var evt = queue.evts[queue.index];
            if (evt.method != null)
            {
                if (rpc == 0) Loger.Error($"事件执行队列循环 key={key} 当前执行中 class={evt.method.ReflectedType.FullName} method={evt.method.Name}");
                else Loger.Error($"事件执行队列循环 key={key} rpc={rpc} 当前执行中 class={evt.method.ReflectedType.FullName} method={evt.method.Name}");
            }
            else
            {

                if (rpc == 0) Loger.Error($"事件执行队列循环 key={key} 当前执行中 target={evt.target}");
                else Loger.Error($"事件执行队列循环 key={key} rpc={rpc} 当前执行中 target={evt.target}");
            }
            Loger.LogStackTrace();
        }

        class EvtData
        {
            public Type key;
            public Type returnType;
            public bool isPropertyOrField;

            public Delegate action;
            public MethodInfo method;
            public FieldInfo field;

            public int sortOrder;
            public bool isOnece;

            public object target;
            public bool isQueue;
            public bool setHandler;
        }
        class EvtQueue
        {
            public List<EvtData> evts = new List<EvtData>();
            public int index = -1;//当前执行位置
            int cnt;

            public void Add(EvtData evt)
            {
                int inserIdx = evts.FindIndex(t => t.sortOrder > evt.sortOrder);
                if (inserIdx != -1)
                {
                    evts.Insert(inserIdx, evt);
                    if (inserIdx < index) ++index;
                    if (inserIdx < cnt) ++cnt;
                }
                else
                    evts.Add(evt);
            }
            public void Remove(object target)
            {
                if (index == -1) evts.RemoveAll(t => t.target == target);
                else
                {
                    for (int j = evts.Count - 1; j >= 0; j--)
                    {
                        if (evts[j].target == target)
                        {
                            evts.RemoveAt(j);
                            if (j < index) --index;
                            if (j < cnt) --cnt;
                        }
                    }
                }
            }
            public async void Run<T>(T data)
            {
                cnt = evts.Count;
                EventHandler eh = new();
                for (index = 0; index < cnt;)
                {
                    EvtData e = evts[index];
                    if (e.isOnece)
                    {
                        evts.RemoveAt(index);
                        --cnt;
                    }
                    else ++index;
                    object o = invoke(e, data, eh);
                    if (o is STask t && e.isQueue) await t;
                }
                index = -1;
            }
            public async STask RunAsync<T>(T data)
            {
                List<STask> ts = ObjectPool<List<STask>>.Get();
                cnt = evts.Count;
                EventHandler eh = new();
                for (index = 0; index < cnt;)
                {
                    EvtData e = evts[index];
                    if (e.isOnece)
                    {
                        evts.RemoveAt(index);
                        --cnt;
                    }
                    else ++index;
                    object o = invoke(e, data, eh);
                    if (o is STask task)
                    {
                        if (e.isQueue) await task;
                        else ts.Add(task);
                    }
                }
                index = -1;
                if (ts.Count > 0)
                {
                    await STask.All(ts, false);
                    ts.Clear();
                    ObjectPool<List<STask>>.Return(ts);
                }
            }
            public async void Run(object data)
            {
                cnt = evts.Count;
                EventHandler eh = new();
                for (index = 0; index < cnt;)
                {
                    EvtData e = evts[index];
                    if (e.isOnece)
                    {
                        evts.RemoveAt(index);
                        --cnt;
                    }
                    else ++index;
                    object o = invoke(e, data, eh);
                    if (o is STask t && e.isQueue) await t;
                }
                index = -1;
            }
            public async STask RunAsync(object data)
            {
                List<STask> ts = ObjectPool<List<STask>>.Get();
                cnt = evts.Count;
                EventHandler eh = new();
                for (index = 0; index < cnt;)
                {
                    EvtData e = evts[index];
                    if (e.isOnece)
                    {
                        evts.RemoveAt(index);
                        --cnt;
                    }
                    else ++index;
                    object o = invoke(e, data, eh);
                    if (o is STask task)
                    {
                        if (e.isQueue) await task;
                        else ts.Add(task);
                    }
                }
                index = -1;
                if (ts.Count > 0)
                {
                    await STask.All(ts, false);
                    ts.Clear();
                    ObjectPool<List<STask>>.Return(ts);
                }
            }
            public async STask<K> RunAsync<K>(object data)
            {
                bool get = false;
                K ret = default;
                STask<K> task = default;
                cnt = evts.Count;
                EventHandler eh = new();
                for (index = 0; index < cnt;)
                {
                    EvtData e = evts[index];
                    if (e.isOnece)
                    {
                        evts.RemoveAt(index);
                        --cnt;
                    }
                    else ++index;
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
                index = -1;
                if (task != null) ret = await task;
                return ret;
            }

            object invoke<T>(EvtData e, T data, EventHandler eh)
            {
                object o = default;
                if (e.target is IEvent evt && !evt.EventEnable) return o;
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
                            else if (e.field != null)
                                e.field.SetValue(e.target, data);
                            else
                                ((STask)e.target).TrySetResult(data);
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
                        if (eh.isBreak) cnt = index;
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
                if (e.isPropertyOrField && e.target is SComponent c) c.Change();
                return o;
            }
            object invoke(EvtData e, object data, EventHandler eh)
            {
                object o = default;
                if (e.target is IEvent evt && !evt.EventEnable) return o;
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
                            ((STask)e.target).TrySetResult(data);
                    }
                    else
                    {
                        var ps = ArrayCache<object>.Get(2);
                        ps[0] = data;
                        ps[1] = eh;
                        o = e.method.Invoke(e.target, ps);
                        if (eh.isBreak) cnt = index;
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
                if (e.isPropertyOrField && e.target is SComponent c) c.Change();
                return o;
            }
        }
    }
}
