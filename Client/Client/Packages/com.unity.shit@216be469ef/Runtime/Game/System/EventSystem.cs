using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Main;

namespace Game
{
    public class EventSystem
    {
        bool _rigistedStaticMethodEvt = false;
        readonly Dictionary<Type, EvtQueue> _evtMap = new(97);
        readonly Dictionary<long, Dictionary<Type, EvtQueue>> _rpcEvtMap = new(97);
        bool _breakCurrentEvent;

        readonly static Dictionary<Type, MethodData[]> _listenerMethodCache = new(97);
        readonly static List<MethodInfo> _methodInfos = new List<MethodInfo>();
        readonly static object[] _parameters = new object[1];
        static Type[] _parameterType = new Type[1];

        static bool _checkEventMethod(MethodInfo method, out Type key)
        {
            var ps = method.GetParameters();
            key = null;
            if (ps.Length != 1)
            {
                Loger.Error("无法解析的参数类型 class:" + method.ReflectedType.FullName + " method:" + method.Name);
                return false;
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
                        if (!_checkEventMethod(method, out e.key))
                            continue;
                        e.info = method;
                        e.sortOrder = ea.SortOrder;
                        e.isRPC = ea is RPCEventAttribute || ea is QueueRPCEventAttribute;
                        e.isQueue = ea is QueueEventAttribute || ea is QueueRPCEventAttribute;
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
                        if (ea is RPCEventAttribute || ea is QueueRPCEventAttribute)
                        {
                            Loger.Error("rpc事件不支持静态");
                            continue;
                        }

                        EvtData e = new();
                        if (!_checkEventMethod(method, out e.Key))
                            continue;

                        if (!_evtMap.TryGetValue(e.Key, out var queue))
                            _evtMap[e.Key] = queue = new();

                        e.method = method;
                        e.sortOrder = ea.SortOrder;
                        e.isQueue = ea is QueueEventAttribute || ea is QueueRPCEventAttribute;

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
                if (m.isRPC) continue;

                if (!_evtMap.TryGetValue(m.key, out var queue))
                    _evtMap[m.key] = queue = new();

                EvtData e = new();
                e.Key = m.key;
                e.method = m.info;
                e.sortOrder = m.sortOrder;
                e.target = target;
                e.isQueue = m.isQueue;

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
                if (!m.isRPC) continue;

                if (!map.TryGetValue(m.key, out var queue))
                    map[m.key] = queue = new();

                EvtData e = new();
                e.Key = m.key;
                e.method = m.info;
                e.sortOrder = m.sortOrder;
                e.target = target;
                e.isQueue = m.isQueue;

                queue.Add(e);
            }
        }
        public TaskAwaiter<T> WaitEvent<T>(int sortOrder = 0)
        {
            TaskAwaiter<T> task = new();
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
        public TaskAwaiter<T> WaitRPCEvent<T>(long rpc, int sortOrder = 0)
        {
            TaskAwaiter<T> task = new();
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
        public TaskAwaiter RunEventAsync(object data)
        {
            return _runEventAsync(_evtMap, 0, data);
        }
        public TaskAwaiter<T> RunEventReturn<T>(object data)
        {
            return _runEventAsyncReturn<T>(_evtMap, 0, data);
        }

        public void RunRPCEvent(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return;
            _runEvent(map, rpc, data);
        }
        public TaskAwaiter RunRPCEventAsync(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return TaskAwaiter.Completed;
            return _runEventAsync(map, rpc, data);
        }
        public async TaskAwaiter<T> RunRPCEventReturn<T>(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map)) return default;
            return await _runEventAsyncReturn<T>(map, rpc, data);
        }

        /// <summary>
        /// 跳出当前正在执行的事件（下一个不执行）
        /// </summary>
        public void BreakCurrentEvent()
        {
            _breakCurrentEvent = true;
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
            queue.Run(data, this);
        }
        TaskAwaiter _runEventAsync(Dictionary<Type, EvtQueue> evtMap, long rpc, object data)
        {
            var key = data.GetType();
            if (!evtMap.TryGetValue(key, out var queue)) return TaskAwaiter.Completed;
            if (queue.index > -1)
            {
                if (rpc == 0) Loger.Error($"事件执行队列循环 key={key}");
                else Loger.Error($"事件执行队列循环 key={key} rpc={rpc}");
                return TaskAwaiter.Completed;
            }
            return queue.RunAsync(data, this);
        }
        async TaskAwaiter<T> _runEventAsyncReturn<T>(Dictionary<Type, EvtQueue> evtMap, long rpc, object data)
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
            var task = queue.RunAsyncReturn<T>(data, this);
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
            public async void Run(object data, EventSystem eventSystem)
            {
                cnt = evts.Count;
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
                        _parameters[0] = data;
                        object o = e.method.Invoke(e.target, default, default, _parameters, default);
                        if (eventSystem._breakCurrentEvent)
                        {
                            eventSystem._breakCurrentEvent = false;
                            cnt = index;
                        }
                        if (o is TaskAwaiter task && e.isQueue)
                            await task;
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("事件执行出错 error:" + ex.ToString());
                    }
                }
                _parameters[0] = null;
                index = -1;
            }
            public async TaskAwaiter RunAsync(object data, EventSystem eventSystem)
            {
                List<TaskAwaiter> ts = ObjectPool.Get<List<TaskAwaiter>>();
                cnt = evts.Count;
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
                        _parameters[0] = data;
                        object o = e.method.Invoke(e.target, default, default, _parameters, default);
                        if (eventSystem._breakCurrentEvent)
                        {
                            eventSystem._breakCurrentEvent = false;
                            cnt = index;
                        }
                        if (o is TaskAwaiter task)
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
                _parameters[0] = null;
                index = -1;
                await TaskAwaiter.All(ts);
                ts.Clear();
                ObjectPool.Return(ts);
            }
            public async TaskAwaiter<T> RunAsyncReturn<T>(object data, EventSystem eventSystem)
            {
                bool get = false;
                T ret = default;
                TaskAwaiter<T> task = default;
                cnt = evts.Count;
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
                        _parameters[0] = data;
                        object o = e.method.Invoke(e.target, default, default, _parameters, default);
                        if (!get && o is T)
                        {
                            get = true;
                            ret = (T)o;
                        }
                        if (!get && o is TaskAwaiter<T>)
                        {
                            get = true;
                            task = (TaskAwaiter<T>)o;
                        }
                        if (eventSystem._breakCurrentEvent)
                        {
                            eventSystem._breakCurrentEvent = false;
                            cnt = index;
                        }
                        if (o is TaskAwaiter t && e.isQueue) await t;
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("事件执行出错 error:" + ex.ToString());
                    }
                }
                _parameters[0] = null;
                index = -1;
                if (task != null) ret = await task;
                return ret;
            }
        }
        class MethodData
        {
            public Type key;

            public MethodInfo info;
            public int sortOrder;
            public bool isRPC;
            public bool isQueue;
        }
    }
}
