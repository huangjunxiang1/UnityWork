using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Main;

namespace Game
{
    public class EventSystem
    {
        bool _rigistedStaticMethodEvt = false;
        readonly Dictionary<Type, List<EvtData>> _evtMap = new(97);
        readonly Dictionary<long, Dictionary<Type, List<EvtData>>> _rpcEvtMap = new(97);
        readonly Dictionary<Type, int> _evtCalling = new(5);
        readonly Dictionary<Type, int> _rpcEvtCalling = new(5);

        readonly static Dictionary<Type, MethodData[]> _listenerMethodCache = new(97);
        readonly static Dictionary<Type, MethodData[]> _rpcListenerMethodCache = new(97);
        readonly static List<MethodInfo> _methodInfos = new List<MethodInfo>();
        readonly static object[] _ilRuntimePs = new object[1];

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
                _methodInfos.Clear();
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
                result = _listenerMethodCache[t] = evts.ToArray();
            }
            return result;
        }
        static MethodData[] _getRPCListenerMethods(Type t)
        {
            if (!_rpcListenerMethodCache.TryGetValue(t, out MethodData[] result))
            {
                _methodInfos.Clear();
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
                        {
                            evts = new List<EvtData>();
                            _evtMap[e.Key] = evts;
                        }

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
                {
                    evts = new List<EvtData>();
                    _evtMap[m.key] = evts;
                }

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
                {
                    map = new();
                    _rpcEvtMap[rpc] = map;
                }
                if (!map.TryGetValue(m.key, out var evts))
                {
                    evts = new();
                    map[m.key] = evts;
                }

                EvtData e = new();
                e.Key = m.key;
                e.method = m.info;
                e.sortOrder = m.sortOrder;
                e.target = target;

                if (!_rpcEvtCalling.TryGetValue(m.key, out var idx))
                    evts.Add(e);
                else
                {
                    if (evts.Count == 0 || m.sortOrder >= evts[evts.Count - 1].sortOrder)
                        evts.Add(e);
                    else
                    {
                        int inserIdx = evts.FindLastIndex(t => t.sortOrder < m.sortOrder) + 1;
                        if (inserIdx <= idx) _rpcEvtCalling[m.key]++;
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
                                if (exIdx <= j)
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
                    var lst = _rpcEvtMap[rpc][m.key];
                    if (_rpcEvtCalling.TryGetValue(m.key, out int exIdx))
                    {
                        for (int j = lst.Count - 1; j >= 0; j--)
                        {
                            if (lst[j].target == target)
                            {
                                lst.RemoveAt(j);
                                if (exIdx <= j)
                                    _rpcEvtCalling[m.key] = --exIdx;
                            }
                        }
                    }
                    else
                        lst.RemoveAll(t => t.target == target);
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
                    Loger.Error("事件执行队列循环 id=" + key);
                    return;
                }
                int i = _evtCalling[key] = 0;
                for (; i < evts.Count; i = ++_evtCalling[key])
                {
                    EvtData e = evts[i];
                    try
                    {
                        _ilRuntimePs[0] = data;
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

        public void RunRPCEvent(long rpc, object data)
        {
            var key = data.GetType();
            if (!_rpcEvtMap.TryGetValue(rpc, out var map))
                return;
            if (!map.TryGetValue(key, out var evts))
                return;

            if (_rpcEvtCalling.ContainsKey(key))
            {
                Loger.Error("事件执行队列循环 id=" + key);
                return;
            }
            int i = _rpcEvtCalling[key] = 0;
            for (; i < evts.Count; i = ++_rpcEvtCalling[key])
            {
                EvtData e = evts[i];
                try
                {
                    _ilRuntimePs[0] = data;
                    e.method.Invoke(e.target, default, default, _ilRuntimePs, default);
                }
                catch (Exception ex)
                {
                    Loger.Error("事件执行出错 error:" + ex.ToString());
                }
            }
            _rpcEvtCalling.Remove(key);
        }

        public void Clear()
        {
            _rigistedStaticMethodEvt = false;
            _evtMap.Clear();
            _rpcEvtMap.Clear();
        }

        class EvtData
        {
            public Type Key;

            public MethodInfo method;
            public int sortOrder;

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
