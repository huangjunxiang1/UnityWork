using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    public static class SysEvent
    {
        class MsgData
        {
            public bool isP0;//无参回调
            public int sortOrder;
            public Action action0;
            public Action<IMessage> action1;
            public object target;
        }
        class EvtData
        {
            public bool isP0;//无参回调
            public int sortOrder;
            public Action action0;
            public Action<EventerContent> action1;
            public object target;
        }
        class MethodData
        {
            public MethodInfo info;
            public byte type;//0 消息  1事件
            public int pCnt;//0无参 1一个参数
            public int sortOrder;

            public ushort opCode;//消息ID

            public int eventID;//事件ID
        }

        readonly static Dictionary<int, List<MsgData>> _msgMap = new(97);
        readonly static Dictionary<int, List<EvtData>> _evtMap = new(97);
        readonly static Dictionary<int, int> _msgCalling = new(5);
        readonly static Dictionary<int, int> _evtCalling = new(5);
        readonly static Dictionary<Type, MethodData[]> _methodCache = new(97);
        readonly static List<MethodInfo> _methodInfos = new List<MethodInfo>();
        readonly static object[] _ilRuntimePs = new object[1];
        static bool _rigistedStaticMethodEvt = false;

        static MethodData[] _getFilterMethods(Type t)
        {
            if (!_methodCache.TryGetValue(t, out MethodData[] result))
            {
                _methodInfos.Clear();
                Type tt = t;
                _methodInfos.AddRange(tt.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
                while ((tt = tt.BaseType) != null)
                    _methodInfos.AddRange(tt.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance));

                List<MethodData> evts = new();
                for (int i = 0; i < _methodInfos.Count; i++)
                {
                    var method = _methodInfos[i];
                    var ps = method.GetParameters();
                    var mas = method.GetCustomAttributes(typeof(MsgAttribute), false);
                    if (mas.Length > 0 && method.IsVirtual)
                    {
                        Loger.Error("监听消息不使用virtual来实现");
                        continue;
                    }
                    for (int k = 0; k < mas.Length; k++)
                    {
                        MsgAttribute a = (MsgAttribute)mas[k];
                        MethodData e = new();
                        e.info = method;
                        e.type = 0;
                        e.opCode = a.OpCode;
                        e.sortOrder = a.SortOrder;

#if ILRuntime
                        if (t is ILRuntime.Reflection.ILRuntimeType)
                        {
                            if (ps.Length == 0)
                                e.pCnt = 0;
                            else if (ps.Length == 1 && ((ILRuntime.Reflection.ILRuntimeWrapperType)ps[0].ParameterType).RealType == typeof(IMessage))
                                e.pCnt = 1;
                            else
                            {
                                Loger.Error("无法解析的参数类型 class:" + t.FullName + " method:" + method.Name);
                                continue;
                            }
                        }
                        else
#endif
                        {
                            if (ps.Length == 0)
                                e.pCnt = 0;
                            else if (ps.Length == 1 && ps[0].ParameterType == typeof(IMessage))
                                e.pCnt = 1;
                            else
                            {
                                Loger.Error("无法解析的参数类型 class:" + t.FullName + " method:" + method.Name);
                                continue;
                            }
                        }

                        evts.Add(e);
                    }

                    var eas = method.GetCustomAttributes(typeof(EventAttribute), false);
                    if (eas.Length > 0 && method.IsVirtual)
                    {
                        Loger.Error("监听消息不使用virtual来实现");
                        continue;
                    }
                    for (int k = 0; k < eas.Length; k++)
                    {
                        EventAttribute a = (EventAttribute)eas[k];
                        MethodData e = new();
                        e.info = method;
                        e.type = 1;
                        e.eventID = a.EventID;
                        e.sortOrder = a.SortOrder;

#if ILRuntime
                        if (t is ILRuntime.Reflection.ILRuntimeType)
                        {
                            if (ps.Length == 0)
                                e.pCnt = 0;
                            else if (ps.Length == 1 && ((ILRuntime.Reflection.ILRuntimeWrapperType)ps[0].ParameterType).RealType == typeof(EventerContent))
                                e.pCnt = 1;
                            else
                            {
                                Loger.Error("无法解析的参数类型 class:" + t.FullName + " method:" + method.Name);
                                continue;
                            }
                        }
                        else
#endif
                        {
                            if (ps.Length == 0)
                                e.pCnt = 0;
                            else if (ps.Length == 1 && ps[0].ParameterType == typeof(EventerContent))
                                e.pCnt = 1;
                            else
                            {
                                Loger.Error("无法解析的参数类型 class:" + t.FullName + " method:" + method.Name);
                                continue;
                            }
                        }

                        evts.Add(e);
                    }
                }
                result = _methodCache[t] = evts.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        public static void RigisterAllStaticListener()
        {
            if (_rigistedStaticMethodEvt) return;
            _rigistedStaticMethodEvt = true;
            int cnt = TypesCache.Types.Length;
            for (int i = 0; i < cnt; i++)
            {
                Type type = TypesCache.Types[i];
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                int len = methods.Length;
                for (int j = 0; j < len; j++)
                {
                    var method = methods[j];
                    var ps = method.GetParameters();
                    var mas = method.GetCustomAttributes(typeof(MsgAttribute), false);
                    for (int k = 0; k < mas.Length; k++)
                    {
                        var a = (MsgAttribute)mas[k];
                        if (!_msgMap.TryGetValue(a.OpCode, out var evts))
                        {
                            evts = new List<MsgData>();
                            _msgMap[a.OpCode] = evts;
                        }

                        MsgData e = new();
                        e.sortOrder = a.SortOrder;
                        e.isP0 = ps.Length == 0;

#if ILRuntime
                        if (type is ILRuntime.Reflection.ILRuntimeType)
                        {
                            if (ps.Length == 0)
                                e.action0 = () => ((ILRuntime.Reflection.ILRuntimeMethodInfo)method).Invoke(null, default, default, default, default);
                            else if (ps.Length == 1 && ((ILRuntime.Reflection.ILRuntimeWrapperType)ps[0].ParameterType).RealType == typeof(IMessage))
                                e.action1 = p =>
                                {
                                    _ilRuntimePs[0] = p;
                                    ((ILRuntime.Reflection.ILRuntimeMethodInfo)method).Invoke(null, default, default, _ilRuntimePs, default);
                                    _ilRuntimePs[0] = null;
                                };
                            else
                            {
                                Loger.Error("参数类型不正确  class:" + type.FullName + "  method:" + method.Name);
                                continue;
                            }
                        }
                        else
#endif
                        {
                            if (ps.Length == 0)
                                e.action0 = (Action)method.CreateDelegate(typeof(Action));
                            else if (ps.Length == 1 && ps[0].ParameterType == typeof(IMessage))
                                e.action1 = (Action<IMessage>)method.CreateDelegate(typeof(Action<IMessage>));
                            else
                            {
                                Loger.Error("参数类型不正确  class:" + type.FullName + "  method:" + method.Name);
                                continue;
                            }
                        }

                        if (evts.Count == 0 || a.SortOrder >= evts[evts.Count - 1].sortOrder)
                            evts.Add(e);
                        else
                        {
                            int inserIdx = evts.FindLastIndex(t => t.sortOrder < a.SortOrder) + 1;
                            evts.Insert(inserIdx, e);
                        }
                    }

                    var eas = method.GetCustomAttributes(typeof(EventAttribute), false);
                    for (int k = 0; k < eas.Length; k++)
                    {
                        var a = (EventAttribute)eas[k];
                        if (!_evtMap.TryGetValue(a.EventID, out var evts))
                        {
                            evts = new List<EvtData>();
                            _evtMap[a.EventID] = evts;
                        }

                        EvtData e = new();
                        e.sortOrder = a.SortOrder;
                        e.isP0 = ps.Length == 0;

#if ILRuntime
                        if (type is ILRuntime.Reflection.ILRuntimeType)
                        {
                            if (ps.Length == 0)
                                e.action0 = () => ((ILRuntime.Reflection.ILRuntimeMethodInfo)method).Invoke(null, default, default, default, default);
                            else if (ps.Length == 1 && ((ILRuntime.Reflection.ILRuntimeWrapperType)ps[0].ParameterType).RealType == typeof(EventerContent))
                                e.action1 = p =>
                                {
                                    _ilRuntimePs[0] = p;
                                    ((ILRuntime.Reflection.ILRuntimeMethodInfo)method).Invoke(null, default, default, _ilRuntimePs, default);
                                    _ilRuntimePs[0] = null;
                                };
                            else
                            {
                                Loger.Error("参数类型不正确  class:" + type.FullName + "  method:" + method.Name);
                                continue;
                            }
                        }
                        else
#endif
                        {
                            if (ps.Length == 0)
                                e.action0 = (Action)method.CreateDelegate(typeof(Action));
                            else if (ps.Length == 1 && ps[0].ParameterType == typeof(EventerContent))
                                e.action1 = (Action<EventerContent>)method.CreateDelegate(typeof(Action<EventerContent>));
                            else
                            {
                                Loger.Error("参数类型不正确  class:" + type.FullName + "  method:" + method.Name);
                                continue;
                            }
                        }

                        if (evts.Count == 0 || a.SortOrder >= evts[evts.Count - 1].sortOrder)
                            evts.Add(e);
                        else
                        {
                            int inserIdx = evts.FindLastIndex(t => t.sortOrder < a.SortOrder) + 1;
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
        public static void RigisterListener(object target)
        {
            MethodData[] ms;
#if ILRuntime
            if (target is ILRuntime.Runtime.Intepreter.ILTypeInstance ilInstance)
                ms = _getFilterMethods(ilInstance.Type.ReflectionType);
            else if (target is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilWarp)
                ms = _getFilterMethods(ilWarp.ILInstance.Type.ReflectionType);
            else
#endif
            ms = _getFilterMethods(target.GetType());

            for (int i = 0; i < ms.Length; i++)
            {
                MethodData m = ms[i];

                if (m.type == 0)
                {
                    if (!_msgMap.TryGetValue(m.opCode, out var evts))
                    {
                        evts = new List<MsgData>();
                        _msgMap[m.opCode] = evts;
                    }

                    MsgData e = new();
                    e.sortOrder = m.sortOrder;
                    e.isP0 = m.pCnt == 0;
                    e.target = target;

                    if (m.pCnt == 0)
                        e.action0 = (Action)m.info.CreateDelegate(typeof(Action), target);
                    else if (m.pCnt == 1)
                        e.action1 = (Action<IMessage>)m.info.CreateDelegate(typeof(Action<IMessage>), target);

                    if (!_msgCalling.TryGetValue(m.opCode, out var idx))
                        evts.Add(e);
                    else
                    {
                        if (evts.Count == 0 || m.sortOrder >= evts[evts.Count - 1].sortOrder)
                            evts.Add(e);
                        else
                        {
                            int inserIdx = evts.FindLastIndex(t => t.sortOrder < m.sortOrder) + 1;
                            if (inserIdx <= idx) _msgCalling[m.opCode]++;
                            evts.Insert(inserIdx, e);
                        }
                    }
                }
                else if (m.type == 1)
                {
                    if (!_evtMap.TryGetValue(m.eventID, out var evts))
                    {
                        evts = new List<EvtData>();
                        _evtMap[m.eventID] = evts;
                    }

                    EvtData e = new();
                    e.sortOrder = m.sortOrder;
                    e.isP0 = m.pCnt == 0;
                    e.target = target;

                    if (m.pCnt == 0)
                        e.action0 = (Action)m.info.CreateDelegate(typeof(Action), target);
                    else if (m.pCnt == 1)
                        e.action1 = (Action<EventerContent>)m.info.CreateDelegate(typeof(Action<EventerContent>), target);

                    if (!_evtCalling.TryGetValue(m.eventID, out var idx))
                        evts.Add(e);
                    else
                    {
                        if (evts.Count == 0 || m.sortOrder >= evts[evts.Count - 1].sortOrder)
                            evts.Add(e);
                        else
                        {
                            int inserIdx = evts.FindLastIndex(t => t.sortOrder < m.sortOrder) + 1;
                            if (inserIdx <= idx) _evtCalling[m.eventID]++;
                            evts.Insert(inserIdx, e);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移除消息和事件监听
        /// </summary>
        /// <param name="target"></param>
        public static void RemoveListener(object target)
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

            if (_methodCache.TryGetValue(t, out MethodData[] ms))
            {
                for (int i = 0; i < ms.Length; i++)
                {
                    MethodData m = ms[i];
                    if (m.type == 0)
                    {
                        var lst = _msgMap[m.opCode];
                        if (_msgCalling.TryGetValue(m.opCode, out int exIdx))
                        {
                            for (int j = lst.Count - 1; j >= 0; j--)
                            {
                                if (lst[j].target == target)
                                {
                                    lst.RemoveAt(j);
                                    if (exIdx <= j)
                                        _msgCalling[m.opCode] = --exIdx;
                                }
                            }
                        }
                        else
                            lst.RemoveAll(t => t.target == target);
                    }
                    else if (m.type == 1)
                    {
                        var lst = _evtMap[m.eventID];
                        if (_evtCalling.TryGetValue(m.eventID, out int exIdx))
                        {
                            for (int j = lst.Count - 1; j >= 0; j--)
                            {
                                if (lst[j].target == target)
                                {
                                    lst.RemoveAt(j);
                                    if (exIdx <= j)
                                        _evtCalling[m.eventID] = --exIdx;
                                }
                            }
                        }
                        else
                            lst.RemoveAll(t => t.target == target);
                    }
                }
            }
        }

        /// <summary>
        /// 推送接受到的网络消息
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="message"></param>
        public static bool ExecuteMessage(int opCode, IMessage message = null)
        {
            bool hasEvt = false;
            if (_msgMap.TryGetValue(opCode, out var evts))
            {
                hasEvt = true;

                if (_msgCalling.ContainsKey(opCode))
                {
                    Loger.Error("消息执行队列循环 msg=" + message);
                    return true;
                }
                int idx = _msgCalling[opCode] = 0;
                for (; idx < evts.Count; idx = ++_msgCalling[opCode])
                {
                    MsgData e = evts[idx];
                    try
                    {
                        if (e.isP0) e.action0();
                        else e.action1(message);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("消息执行出错 error:" + ex.ToString());
                    }
                }
                _msgCalling.Remove(opCode);
            }
            return hasEvt;
        }

        /// <summary>
        /// 推送事件
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="data"></param>
        public static void ExecuteEvent(int eventID)
        {
            ExecuteEvent(eventID, 0, null);
        }
        public static void ExecuteEvent(int eventID, object data)
        {
            ExecuteEvent(eventID, 0, data);
        }
        public static void ExecuteEvent(int eventID, int value)
        {
            ExecuteEvent(eventID, value, null);
        }
        public static void ExecuteEvent(int eventID, int value, object data)
        {
            if (_evtMap.TryGetValue(eventID, out var evts))
            {
                if (_evtCalling.ContainsKey(eventID))
                {
                    Loger.Error("事件执行队列循环 id=" + eventID);
                    return;
                }
                int idx = _evtCalling[eventID] = 0;
                for (; idx < evts.Count; idx = ++_evtCalling[eventID])
                {
                    EvtData e = evts[idx];
                    try
                    {
                        if (e.isP0) e.action0();
                        else e.action1(new EventerContent(e.target, value, data));
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("事件执行出错 error:" + ex.ToString());
                    }
                }
                _evtCalling.Remove(eventID);
            }
        }
    }
}
