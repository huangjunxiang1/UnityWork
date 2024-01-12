using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game
{
    public class EventSystem
    {
        public EventSystem()
        {
            this.RigisteAllStaticEvent();
        }

        readonly Dictionary<Type, EvtQueue> _evtMap = new(97);
        readonly Dictionary<long, Dictionary<Type, EvtQueue>> _rpcEvtMap = new(97);

        readonly static Dictionary<Type, MethodData[]> _listenerMethodCache = new(97);
        readonly static List<MethodInfo> _methodInfos = new List<MethodInfo>();
        static Type[] _parameterType1 = new Type[1];
        static Type[] _parameterType2 = new Type[2];
        static Type[] _parameterType3 = new Type[3];

        static bool _checkEventMethod(MethodInfo method, out Type key, out bool setHandler, out Type returnType)
        {
            key = null;
            setHandler = false;
            returnType = method.ReturnType;
            if (method.IsGenericMethod)
            {
                Loger.Error("事件函数不能是泛型函数");
                return false;
            }
            var ps = method.GetParameters();
            if (ps.Length == 0 || ps.Length > 2)
            {
                Loger.Error("无法解析的参数类型 class:" + method.ReflectedType.FullName + " method:" + method.Name);
                return false;
            }
            if (ps.Length == 2)
            {
                setHandler = ps[1].ParameterType == typeof(EventHandler);
                if (!setHandler)
                {
                    Loger.Error("无法解析的参数类型 class:" + method.ReflectedType.FullName + " method:" + method.Name);
                    return false;
                }
            }
            key = ps[0].ParameterType;
#if DebugEnable
            if (key.IsPrimitive)
                Loger.Error("不要使用系统值类型作为事件参数类型");
#endif
            return true;
        }
        /// <summary>
        /// 获取对应事件函数
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        static MethodData[] _getListenerMethods(Type t)
        {
            if (!_listenerMethodCache.TryGetValue(t, out MethodData[] result))
            {
                Type tt = t;
                _methodInfos.AddRange(tt.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
                while ((tt = tt.BaseType) != null)
                {
                    var ms = tt.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
                    for (int i = 0; i < ms.Length; i++)
                    {
                        if (ms[i].IsPrivate)
                            _methodInfos.Add(ms[i]);
                    }
                }

                List<MethodData> evts = new();
                for (int i = 0; i < _methodInfos.Count; i++)
                {
                    var method = _methodInfos[i];

                    var ea = method.GetCustomAttributes(typeof(EventAttribute), true).FirstOrDefault() as EventAttribute;
                    if (ea != null)
                    {
                        MethodData e = new();
                        if (!_checkEventMethod(method, out e.key, out e.setHandler, out e.returnType))
                            continue;
                        e.method = method;
                        e.attribute = ea;
                        evts.Add(e);
                    }
                }
                _methodInfos.Clear();
                result = _listenerMethodCache[t] = evts.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        void RigisteAllStaticEvent()
        {
            var lst = Types.GetStaticMethods();
            for (int i = 0; i < lst.Count; i++)
            {
                MethodAndAttribute ma = lst[i];
                if (ma.attribute is EventAttribute ea)
                {
                    if (ea.RPC)
                    {
                        Loger.Error("rpc事件不支持静态");
                        continue;
                    }

                    EvtData e = new();
                    if (!_checkEventMethod(ma.method, out e.Key, out e.setHandler, out e.returnType))
                        continue;

                    if (!_evtMap.TryGetValue(e.Key, out var queue))
                        _evtMap[e.Key] = queue = new();

                    e.sortOrder = ea.SortOrder;
                    e.isQueue = ea.Queue;
                    e.method = ma.method;
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
            MethodData[] ms = _getListenerMethods(t);

            for (int i = 0; i < ms.Length; i++)
            {
                MethodData m = ms[i];
                if (m.attribute.RPC) continue;

                if (!_evtMap.TryGetValue(m.key, out var queue))
                    _evtMap[m.key] = queue = new();

                EvtData e = new();
                e.Key = m.key;
                e.method = m.method;
                e.sortOrder = m.attribute.SortOrder;
                e.target = target;
                e.isQueue = m.attribute.Queue;
                e.setHandler = m.setHandler;
                e.returnType = m.returnType;
                createDelegate(e, target);

                queue.Add(e);
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
            MethodData[] ms = _getListenerMethods(t);

            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                _rpcEvtMap[rpc] = map = new();

            for (int i = 0; i < ms.Length; i++)
            {
                MethodData m = ms[i];
                if (!m.attribute.RPC) continue;

                if (!map.TryGetValue(m.key, out var queue))
                    map[m.key] = queue = new();

                EvtData e = new();
                e.Key = m.key;
                e.method = m.method;
                e.sortOrder = m.attribute.SortOrder;
                e.target = target;
                e.isQueue = m.attribute.Queue;
                e.setHandler = m.setHandler;
                e.returnType = m.returnType;
                createDelegate(e, target);

                queue.Add(e);
            }
        }
        public STask<T> WaitEvent<T>(int sortOrder = 0)
        {
            STask<T> task = new();
            var k = typeof(T);

            if (!_evtMap.TryGetValue(k, out var queue))
                _evtMap[k] = queue = new();

            EvtData e = new();
            e.Key = k;
            _parameterType1[0] = k;
            e.method = task.GetType().GetMethod(nameof(task.TrySetResult), _parameterType1);
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
            e.Key = k;
            _parameterType1[0] = k;
            e.method = task.GetType().GetMethod(nameof(task.TrySetResult), _parameterType1);
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
            if (_listenerMethodCache.TryGetValue(t, out MethodData[] ms))
            {
                for (int i = 0; i < ms.Length; i++)
                    if (_evtMap.TryGetValue(ms[i].key, out var queue))
                        queue.Remove(target);
            }
        }
        public void RemoveRPCEvent(long rpc, object target)
        {
            if (target == null)
            {
                Loger.Error("移除事件对象为空");
                return;
            }
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return;
            Type t = target.GetType();
            if (_listenerMethodCache.TryGetValue(t, out MethodData[] ms))
            {
                for (int i = 0; i < ms.Length; i++)
                    if (map.TryGetValue(ms[i].key, out var queue))
                        queue.Remove(target);
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
                if (rpc == 0) Loger.Error($"事件执行队列循环 key={key}");
                else Loger.Error($"事件执行队列循环 key={key} rpc={rpc}");
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
                if (rpc == 0) Loger.Error($"事件执行队列循环 key={key}");
                else Loger.Error($"事件执行队列循环 key={key} rpc={rpc}");
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
                if (rpc == 0) Loger.Error($"事件执行队列循环 key={key}");
                else Loger.Error($"事件执行队列循环 key={key} rpc={rpc}");
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
                    _parameterType2[0] = ed.Key;
                    _parameterType2[1] = typeof(EventHandler);
                    ed.action = ed.method.CreateDelegate(typeof(Action<,>).MakeGenericType(_parameterType2), o);
                }
                else
                {
                    _parameterType1[0] = ed.Key;
                    ed.action = ed.method.CreateDelegate(typeof(Action<>).MakeGenericType(_parameterType1), o);
                }
            }
            else
            {
                if (ed.setHandler)
                {
                    _parameterType3[0] = ed.Key;
                    _parameterType3[1] = typeof(EventHandler);
                    _parameterType3[2] = ed.returnType;
                    ed.action = ed.method.CreateDelegate(typeof(Func<,,>).MakeGenericType(_parameterType3), o);
                }
                else
                {
                    _parameterType2[0] = ed.Key;
                    _parameterType2[1] = ed.returnType;
                    ed.action = ed.method.CreateDelegate(typeof(Func<,>).MakeGenericType(_parameterType2), o);
                }
            }
        }

        class EvtData
        {
            public Type Key;
            public Type returnType;

            public Delegate action;
            public MethodInfo method;

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

            static object[] parameters1 = new object[1];
            static object[] parameters2 = new object[2];

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
                EventHandler eh = ObjectPool.Get<EventHandler>();
                eh.Reset();
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
                ObjectPool.Return(eh);
            }
            public async STask RunAsync<T>(T data)
            {
                List<STask> ts = ObjectPool.Get<List<STask>>();
                cnt = evts.Count;
                EventHandler eh = ObjectPool.Get<EventHandler>();
                eh.Reset();
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
                ObjectPool.Return(eh);
                await STask.All(ts);
                ts.Clear();
                ObjectPool.Return(ts);
            }
            public async STask<K> RunAsync<K>(object data)
            {
                bool get = false;
                K ret = default;
                STask<K> task = default;
                cnt = evts.Count;
                EventHandler eh = ObjectPool.Get<EventHandler>();
                eh.Reset();
                for (index = 0; index < cnt;)
                {
                    EvtData e = evts[index];
                    if (e.isOnece)
                    {
                        evts.RemoveAt(index);
                        --cnt;
                    }
                    else ++index;
                    object o = invoke2(e, data, eh);
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
                ObjectPool.Return(eh);
                if (task != null) ret = await task;
                return ret;
            }

            object invoke<T>(EvtData e, T data, EventHandler eh)
            {
                object o = default;
                try
                {
                    if (!e.setHandler)
                    {
                        if (e.returnType == typeof(void))
                            ((Action<T>)e.action).Invoke(data);
                        else
                        {
                            parameters1[0] = data;
                            o = e.method.Invoke(e.target, parameters1);
                        }
                    }
                    else
                    {
                        if (e.returnType == typeof(void))
                            ((Action<T, EventHandler>)e.action).Invoke(data, eh);
                        else
                        {
                            parameters2[0] = data;
                            parameters2[1] = eh;
                            o = e.method.Invoke(e.target, parameters2);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
                if (eh.isBreak) cnt = index;
                return o;
            }
            object invoke2(EvtData e, object data, EventHandler eh)
            {
                object o = default;
                try
                {
                    if (!e.setHandler)
                    {
                        parameters1[0] = data;
                        o = e.method.Invoke(e.target, parameters1);
                    }
                    else
                    {
                        parameters2[0] = data;
                        parameters2[1] = eh;
                        o = e.method.Invoke(e.target, parameters2);
                    }
                }
                catch (Exception ex)
                {
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
                if (eh.isBreak) cnt = index;
                return o;
            }
        }
        class MethodData
        {
            public Type key;
            public Type returnType;

            public MethodInfo method;
            public EventAttribute attribute;
            public bool setHandler;
        }
    }
}
