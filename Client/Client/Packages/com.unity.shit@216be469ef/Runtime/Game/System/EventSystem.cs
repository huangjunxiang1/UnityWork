using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game
{
    public class EventSystem
    {
        bool _rigistedStaticMethodEvt = false;
        readonly Dictionary<Type, EvtQueue> _evtMap = new(97);
        readonly Dictionary<long, Dictionary<Type, EvtQueue>> _rpcEvtMap = new(97);

        readonly static Dictionary<Type, MethodData[]> _listenerMethodCache = new(97);
        readonly static List<MethodInfo> _methodInfos = new List<MethodInfo>();
        static Type[] _parameterType = new Type[1];

        static bool _checkEventMethod(MethodInfo method, out Type key, out bool setHandler)
        {
            key = null;
            setHandler = false;
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
                setHandler = ps[1].ParameterType == typeof(SEventHandler);
                if (!setHandler)
                {
                    Loger.Error("无法解析的参数类型 class:" + method.ReflectedType.FullName + " method:" + method.Name);
                    return false;
                }
            }
            key = ps[0].ParameterType;
#if ILRuntime
            if (key is ILRuntime.Reflection.ILRuntimeWrapperType warp)
                key = warp.RealType;
#endif
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
                        if (!_checkEventMethod(method, out e.key, out e.setHandler))
                            continue;
                        e.info = method;
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
        public void RigisteAllStaticListener()
        {
            if (_rigistedStaticMethodEvt) return;
            _rigistedStaticMethodEvt = true;
            int cnt = Types.AllTypes.Length;
            for (int i = 0; i < cnt; i++)
            {
                Type type = Types.AllTypes[i];
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                int len = methods.Length;
                for (int j = 0; j < len; j++)
                {
                    var method = methods[j];

                    var ea = method.GetCustomAttributes(typeof(EventAttribute), false).FirstOrDefault() as EventAttribute;
                    if (ea != null)
                    {
                        if (ea.RPC)
                        {
                            Loger.Error("rpc事件不支持静态");
                            continue;
                        }

                        EvtData e = new();
                        if (!_checkEventMethod(method, out e.Key, out e.setHandler))
                            continue;

                        if (!_evtMap.TryGetValue(e.Key, out var queue))
                            _evtMap[e.Key] = queue = new();

                        e.method = method;
                        e.sortOrder = ea.SortOrder;
                        e.isQueue = ea.Queue;

                        queue.Add(e);
                    }
                }
            }
        }

        /// <summary>
        /// 注册消息和事件监听
        /// </summary>
        /// <param name="target"></param>
        public void RigisteListener(object target)
        {
            if (target == null)
            {
                Loger.Error("添加事件对象为空");
                return;
            }
            Type t = _getType(target);
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
                e.method = m.info;
                e.sortOrder = m.attribute.SortOrder;
                e.target = target;
                e.isQueue = m.attribute.Queue;
                e.setHandler = m.setHandler;

                queue.Add(e);
            }
        }
        public void RigisteRPCListener(long rpc, object target)
        {
            if (target == null)
            {
                Loger.Error("添加事件对象为空");
                return;
            }
            Type t = _getType(target);
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
                e.method = m.info;
                e.sortOrder = m.attribute.SortOrder;
                e.target = target;
                e.isQueue = m.attribute.Queue;
                e.setHandler = m.setHandler;

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
            _parameterType[0] = k;
            e.method = task.GetType().GetMethod(nameof(task.TrySetResult), _parameterType);
            _parameterType[0] = null;
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
            _parameterType[0] = k;
            e.method = task.GetType().GetMethod(nameof(task.TrySetResult), _parameterType);
            _parameterType[0] = null;
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
        public void RemoveListener(object target)
        {
            if (target == null)
            {
                Loger.Error("移除事件对象为空");
                return;
            }
            Type t = _getType(target);
            if (_listenerMethodCache.TryGetValue(t, out MethodData[] ms))
            {
                for (int i = 0; i < ms.Length; i++)
                    if (_evtMap.TryGetValue(ms[i].key, out var queue))
                        queue.Remove(target);
            }
        }
        public void RemoveRPCListener(long rpc, object target)
        {
            if (target == null)
            {
                Loger.Error("移除事件对象为空");
                return;
            }
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return;
            Type t = _getType(target);
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
        public void RunEvent(object data)
        {
            _runEvent(_evtMap, 0, data);
        }
        public STask RunEventAsync(object data)
        {
            return _runEventAsync(_evtMap, 0, data);
        }
        public STask<T> RunEventReturn<T>(object data)
        {
            return _runEventAsyncReturn<T>(_evtMap, 0, data);
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
        public async STask<T> RunRPCEventReturn<T>(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return default;
            return await _runEventAsyncReturn<T>(map, rpc, data);
        }

        public void Clear()
        {
            _rigistedStaticMethodEvt = false;
            _evtMap.Clear();
            _rpcEvtMap.Clear();
        }

        Type _getType(object target)
        {
#if ILRuntime
            if (target is ILRuntime.Runtime.Intepreter.ILTypeInstance ilInstance)
                return ilInstance.Type.ReflectionType;
            else if (target is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilWarp)
                return ilWarp.ILInstance.Type.ReflectionType;
            else
#endif
            return target.GetType();
        }
        void _runEvent(Dictionary<Type, EvtQueue> evtMap, long rpc, object data)
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
        STask _runEventAsync(Dictionary<Type, EvtQueue> evtMap, long rpc, object data)
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
        async STask<T> _runEventAsyncReturn<T>(Dictionary<Type, EvtQueue> evtMap, long rpc, object data)
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
            var task = queue.RunAsyncReturn<T>(data);
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
            public Type Key;

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
            static object[] _parameters1 = new object[1];
            static object[] _parameters2 = new object[2];

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
            public async void Run(object data)
            {
                cnt = evts.Count;
                SEventHandler eh = SObjectPool.Get<SEventHandler>();
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
                    try
                    {
                        object o = invoke(e, data, eh);
                        if (o is STask t && e.isQueue) await t;
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("事件执行出错 error:" + ex.ToString());
                    }
                }
                index = -1;
                SObjectPool.Return(eh);
            }
            public async STask RunAsync(object data)
            {
                List<STask> ts = SObjectPool.Get<List<STask>>();
                cnt = evts.Count;
                SEventHandler eh = SObjectPool.Get<SEventHandler>();
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
                    try
                    {
                        object o = invoke(e, data, eh);
                        if (o is STask task)
                        {
                            if (e.isQueue) await task;
                            else ts.Add(task);
                        }
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("事件执行出错 error:" + ex.ToString());
                    }
                }
                index = -1;
                SObjectPool.Return(eh);
                await STask.All(ts);
                ts.Clear();
                SObjectPool.Return(ts);
            }
            public async STask<T> RunAsyncReturn<T>(object data)
            {
                bool get = false;
                T ret = default;
                STask<T> task = default;
                cnt = evts.Count;
                SEventHandler eh = SObjectPool.Get<SEventHandler>();
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
                    try
                    {
                        object o = invoke(e, data, eh);
                        if (!get && o is T)
                        {
                            get = true;
                            ret = (T)o;
                        }
                        if (!get && o is STask<T>)
                        {
                            get = true;
                            task = (STask<T>)o;
                        }
                        if (o is STask t && e.isQueue) await t;
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("事件执行出错 error:" + ex.ToString());
                    }
                }
                index = -1;
                SObjectPool.Return(eh);
                if (task != null) ret = await task;
                return ret;
            }

            object invoke(EvtData e, object data, SEventHandler eh)
            {
                object o;
                if (!e.setHandler)
                {
                    _parameters1[0] = data;
                    o = e.method.Invoke(e.target, default, default, _parameters1, default);
                    _parameters1[0] = default;
                }
                else
                {
                    _parameters2[0] = data;
                    _parameters2[1] = eh;
                    o = e.method.Invoke(e.target, default, default, _parameters2, default);
                    _parameters2[0] = default;
                }
                if (eh.isBreak) cnt = index;
                return o;
            }
        }
        class MethodData
        {
            public Type key;

            public MethodInfo info;
            public EventAttribute attribute;
            public bool setHandler;
        }
    }
    public class SEventHandler
    {
        public bool isBreak { get; private set; }
        public void BreakEvent()
        {
            isBreak = true;
        }
        internal void Reset()
        {
            this.isBreak = false;
        }
    }
}
