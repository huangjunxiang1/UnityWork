using System;
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
        readonly Dictionary<Type, List<EvtData>> _evtMap = new(97);
        readonly Dictionary<long, Dictionary<Type, List<EvtData>>> _rpcEvtMap = new(97);
        readonly Dictionary<Type, int> _evtCalling = new(5);
        readonly Dictionary<long, Dictionary<Type, int>> _evtRpcCalling = new(5);

        readonly static Dictionary<Type, MethodData[]> _listenerMethodCache = new(97);
        readonly static Dictionary<Type, MethodData[]> _rpcListenerMethodCache = new(97);
        readonly static List<MethodInfo> _methodInfos = new List<MethodInfo>();
        readonly static object[] _ilRuntimePs = new object[1];
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

                    var ea = method.GetCustomAttributes(typeof(EventAttribute), false).FirstOrDefault() as EventAttribute;
                    if (ea != null)
                    {
                        if (method.IsVirtual)
                        {
                            Loger.Error("监听消息不使用virtual来实现");
                            continue;
                        }

                        MethodData e = new();
                        if (!_checkEventMethod(method, out e.key))
                            continue;
                        e.info = method;
                        e.sortOrder = ea.SortOrder;
                        evts.Add(e);
                    }
                }
                _methodInfos.Clear();
                result = _listenerMethodCache[t] = evts.ToArray();
            }
            return result;
        }
        static MethodData[] _getRPCListenerMethods(Type t)
        {
            if (!_rpcListenerMethodCache.TryGetValue(t, out MethodData[] result))
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
                    var ea = method.GetCustomAttributes(typeof(RPCEventAttribute), false).FirstOrDefault() as RPCEventAttribute;
                    if (ea != null)
                    {
                        if (method.IsVirtual)
                        {
                            Loger.Error("监听消息不使用virtual来实现");
                            continue;
                        }

                        MethodData e = new();
                        if (!_checkEventMethod(method, out e.key))
                            continue;
                        e.info = method;
                        e.sortOrder = ea.SortOrder;
                        evts.Add(e);
                    }
                }
                _methodInfos.Clear();
                result = _rpcListenerMethodCache[t] = evts.ToArray();
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
                        EvtData e = new();
                        if (!_checkEventMethod(method, out e.Key))
                            continue;

                        if (!_evtMap.TryGetValue(e.Key, out var evts))
                            _evtMap[e.Key] = evts = new();

                        e.method = method;
                        e.sortOrder = ea.SortOrder;

                        if (evts.Count == 0 || ea.SortOrder >= evts[evts.Count - 1].sortOrder)
                            evts.Add(e);
                        else
                        {
                            int inserIdx = evts.FindLastIndex(t => t.sortOrder < ea.SortOrder) + 1;
                            evts.Insert(inserIdx, e);
                        }
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
            MethodData[] ms;
#if ILRuntime
            if (target is ILRuntime.Runtime.Intepreter.ILTypeInstance ilInstance)
                ms = _getListenerMethods(ilInstance.Type.ReflectionType);
            else if (target is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilWarp)
                ms = _getListenerMethods(ilWarp.ILInstance.Type.ReflectionType);
            else
#endif
            ms = _getListenerMethods(target.GetType());

            for (int i = 0; i < ms.Length; i++)
            {
                MethodData m = ms[i];

                if (!_evtMap.TryGetValue(m.key, out var evts))
                    _evtMap[m.key] = evts = new();

                EvtData e = new();
                e.Key = m.key;
                e.method = m.info;
                e.sortOrder = m.sortOrder;
                e.target = target;

                if (!_evtCalling.TryGetValue(m.key, out var idx))
                    evts.Add(e);
                else
                {
                    if (evts.Count == 0 || m.sortOrder >= evts[evts.Count - 1].sortOrder)
                        evts.Add(e);
                    else
                    {
                        int inserIdx = evts.FindLastIndex(t => t.sortOrder < m.sortOrder) + 1;
                        if (inserIdx <= idx) _evtCalling[m.key]++;
                        evts.Insert(inserIdx, e);
                    }
                }
            }
        }
        public void RigisteRPCListener(long rpc, object target)
        {
            MethodData[] ms;
#if ILRuntime
            if (target is ILRuntime.Runtime.Intepreter.ILTypeInstance ilInstance)
                ms = _getRPCListenerMethods(ilInstance.Type.ReflectionType);
            else if (target is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilWarp)
                ms = _getRPCListenerMethods(ilWarp.ILInstance.Type.ReflectionType);
            else
#endif
            ms = _getRPCListenerMethods(target.GetType());

            for (int i = 0; i < ms.Length; i++)
            {
                MethodData m = ms[i];

                if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                    _rpcEvtMap[rpc] = map = new();

                if (!map.TryGetValue(m.key, out var evts))
                    map[m.key] = evts = new();

                EvtData e = new();
                e.Key = m.key;
                e.method = m.info;
                e.sortOrder = m.sortOrder;
                e.target = target;

                if (!_evtRpcCalling.TryGetValue(rpc, out var c) || !c.TryGetValue(m.key, out var idx))
                    evts.Add(e);
                else
                {
                    if (evts.Count == 0 || m.sortOrder >= evts[evts.Count - 1].sortOrder)
                        evts.Add(e);
                    else
                    {
                        int inserIdx = evts.FindLastIndex(t => t.sortOrder < m.sortOrder) + 1;
                        if (inserIdx <= idx) c[m.key]++;
                        evts.Insert(inserIdx, e);
                    }
                }
            }
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        /// <param name="target"></param>
        public void RemoveListener(object target)
        {
            Type t;
#if ILRuntime
            if (target is ILRuntime.Runtime.Intepreter.ILTypeInstance ilInstance)
                t = ilInstance.Type.ReflectionType;
            else if (target is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilWarp)
                t = ilWarp.ILInstance.Type.ReflectionType;
            else
#endif
            t = target.GetType();

            if (_listenerMethodCache.TryGetValue(t, out MethodData[] ms))
            {
                for (int i = 0; i < ms.Length; i++)
                {
                    MethodData m = ms[i];
                    var lst = _evtMap[m.key];
                    if (_evtCalling.TryGetValue(m.key, out int exIdx))
                    {
                        for (int j = lst.Count - 1; j >= 0; j--)
                        {
                            if (lst[j].target == target)
                            {
                                lst.RemoveAt(j);
                                if (j <= exIdx)
                                    _evtCalling[m.key] = --exIdx;
                            }
                        }
                    }
                    else
                        lst.RemoveAll(t => t.target == target);
                }
            }
        }
        public void RemoveRPCListener(long rpc, object target)
        {
            if (_rpcEvtMap.TryGetValue(rpc, out var map))
            {
                Type t;
#if ILRuntime
                if (target is ILRuntime.Runtime.Intepreter.ILTypeInstance ilInstance)
                    t = ilInstance.Type.ReflectionType;
                else if (target is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilWarp)
                    t = ilWarp.ILInstance.Type.ReflectionType;
                else
#endif
                    t = target.GetType();

                if (_rpcListenerMethodCache.TryGetValue(t, out MethodData[] ms))
                {
                    for (int i = 0; i < ms.Length; i++)
                    {
                        MethodData m = ms[i];
                        var lst = map[m.key];
                        if (_evtRpcCalling.TryGetValue(rpc, out var c) && c.TryGetValue(m.key, out int exIdx))
                        {
                            for (int j = lst.Count - 1; j >= 0; j--)
                            {
                                if (lst[j].target == target)
                                {
                                    lst.RemoveAt(j);
                                    if (j <= exIdx)
                                        c[m.key] = --exIdx;
                                }
                            }
                        }
                        else
                            lst.RemoveAll(t => t.target == target);
                    }
                }
            }
        }

        public void RunEvent(object data)
        {
            var key = data.GetType();
            if (_evtMap.TryGetValue(key, out var evts))
            {
                if (_evtCalling.ContainsKey(key))
                {
                    Loger.Error("事件执行队列循环 key=" + key);
                    return;
                }
                int i = _evtCalling[key] = 0;
                _ilRuntimePs[0] = data;
                for (; i < evts.Count;)
                {
                    EvtData e = evts[i];
                    if (e.isOnece) evts.RemoveAt(i);
                    else i = ++_evtCalling[key];
                    try
                    {
                        e.method.Invoke(e.target, default, default, _ilRuntimePs, default);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("事件执行出错 error:" + ex.ToString());
                    }
                }
                _evtCalling.Remove(key);
            }
        }
        public async TaskAwaiter RunEventAsync(object data)
        {
            var key = data.GetType();
            if (_evtMap.TryGetValue(key, out var evts))
            {
                if (_evtCalling.ContainsKey(key))
                {
                    Loger.Error("事件执行队列循环 key=" + key);
                    return;
                }
                int i = _evtCalling[key] = 0;
                _ilRuntimePs[0] = data;
                List<TaskAwaiter> ts = ObjectPool.Get<List<TaskAwaiter>>();
                for (; i < evts.Count;)
                {
                    EvtData e = evts[i];
                    if (e.isOnece) evts.RemoveAt(i);
                    else i = ++_evtCalling[key];
                    try
                    {
                        if (e.method.Invoke(e.target, default, default, _ilRuntimePs, default) is TaskAwaiter t)
                            ts.Add(t);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("事件执行出错 error:" + ex.ToString());
                    }
                }
                _evtCalling.Remove(key);
                await TaskAwaiter.All(ts);
                ts.Clear();
                ObjectPool.Return(ts);
            }
        }

        public void RunRPCEvent(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                return;
            var key = data.GetType();
            if (!map.TryGetValue(key, out var evts))
                return;

            if (!_evtRpcCalling.TryGetValue(rpc, out var c))
                _evtRpcCalling[rpc] = c = new();
            if (c.ContainsKey(key))
            {
                Loger.Error($"事件执行队列循环 rpc={rpc} key={key}");
                return;
            }
            int i = c[key] = 0;
            _ilRuntimePs[0] = data;
            for (; i < evts.Count;)
            {
                EvtData e = evts[i];
                if (e.isOnece) evts.RemoveAt(i);
                else i = ++c[key];
                try
                {
                    e.method.Invoke(e.target, default, default, _ilRuntimePs, default);
                }
                catch (Exception ex)
                {
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
            }
            c.Remove(key);
            if (c.Count == 0)
                _evtRpcCalling.Remove(rpc);
        }
        public async TaskAwaiter RunRPCEventAsync(long rpc, object data)
        {
            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                return;
            var key = data.GetType();
            if (!map.TryGetValue(key, out var evts))
                return;

            if (!_evtRpcCalling.TryGetValue(rpc, out var c))
                _evtRpcCalling[rpc] = c = new();
            if (c.ContainsKey(key))
            {
                Loger.Error($"事件执行队列循环 rpc={rpc} key={key}");
                return;
            }
            int i = c[key] = 0;
            _ilRuntimePs[0] = data;
            List<TaskAwaiter> ts = ObjectPool.Get<List<TaskAwaiter>>();
            for (; i < evts.Count;)
            {
                EvtData e = evts[i];
                if (e.isOnece) evts.RemoveAt(i);
                else i = ++c[key];
                try
                {
                    if (e.method.Invoke(e.target, default, default, _ilRuntimePs, default) is TaskAwaiter t)
                        ts.Add(t);
                }
                catch (Exception ex)
                {
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
            }
            c.Remove(key);
            if (c.Count == 0)
                _evtRpcCalling.Remove(rpc);
            await TaskAwaiter.All(ts);
            ts.Clear();
            ObjectPool.Return(ts);
        }

        public TaskAwaiter<T> WaitEvent<T>(int sortOrder = 0)
        {
            TaskAwaiter<T> task = new();
            var k = typeof(T);

            if (!_evtMap.TryGetValue(k, out var evts))
                _evtMap[k] = evts = new();

            EvtData e = new();
            e.Key = k;
            _parameterType[0] = k;
            e.method = task.GetType().GetMethod(nameof(task.TrySetResult), _parameterType);
            e.sortOrder = sortOrder;
            e.isOnece = true;
            e.target = task;

            if (!_evtCalling.TryGetValue(k, out var idx))
                evts.Add(e);
            else
            {
                if (evts.Count == 0 || sortOrder >= evts[evts.Count - 1].sortOrder)
                    evts.Add(e);
                else
                {
                    int inserIdx = evts.FindLastIndex(t => t.sortOrder < sortOrder) + 1;
                    if (inserIdx <= idx) _evtCalling[k]++;
                    evts.Insert(inserIdx, e);
                }
            }
            return task;
        }
        public TaskAwaiter<T> WaitRPCEvent<T>(long rpc, int sortOrder = 0)
        {
            TaskAwaiter<T> task = new();
            var k = typeof(T);

            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                _rpcEvtMap[rpc] = map = new();

            if (!map.TryGetValue(k, out var evts))
                map[k] = evts = new();

            EvtData e = new();
            e.Key = k;
            _parameterType[0] = k;
            e.method = task.GetType().GetMethod(nameof(task.TrySetResult), _parameterType);
            e.sortOrder = sortOrder;
            e.isOnece = true;
            e.target = task;

            if (!_evtRpcCalling.TryGetValue(rpc, out var c) || !c.TryGetValue(k, out var idx))
                evts.Add(e);
            else
            {
                if (evts.Count == 0 || sortOrder >= evts[evts.Count - 1].sortOrder)
                    evts.Add(e);
                else
                {
                    int inserIdx = evts.FindLastIndex(t => t.sortOrder < sortOrder) + 1;
                    if (inserIdx <= idx) c[k]++;
                    evts.Insert(inserIdx, e);
                }
            }
            return task;
        }

        public void Clear()
        {
            _rigistedStaticMethodEvt = false;
            _evtMap.Clear();
            _rpcEvtMap.Clear();
            _evtCalling.Clear();
            _evtRpcCalling.Clear();
        }

        class EvtData
        {
            public Type Key;

            public MethodInfo method;
            public int sortOrder;
            public bool isOnece;

            public object target;
        }
        class MethodData
        {
            public Type key;

            public MethodInfo info;
            public int sortOrder;
        }
    }
}
