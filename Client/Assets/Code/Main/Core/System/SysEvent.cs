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
            public bool isCanceled;
            public bool isP0;//无参回调
            public int sortOrder;
            public Action action0;
            public Action<IMessage> action1;
        }
        class EvtData
        {
            public bool isCanceled;
            public bool isP0;//无参回调
            public int sortOrder;
            public Action action0;
            public Action<object> action1;
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

        readonly static Dictionary<int, List<MsgData>> _msgMap = new Dictionary<int, List<MsgData>>();
        readonly static Dictionary<int, List<EvtData>> _evtMap = new Dictionary<int, List<EvtData>>();
        static bool _rigistedStaticMethodEvt = false;
        static bool _isCalling = false;//是否正在call事件或消息
        static bool _isCallMsg;//正在call的是消息
        static int _isCallID;//正在call的事件或消息ID
        static int _isCallIndex = -1;//当前正在执行的事件下标
        static Dictionary<Type, MethodData[]> _methodCache = new Dictionary<Type, MethodData[]>();

        static MethodData[] _getFilterMethods(Type t)
        {
            if (!_methodCache.TryGetValue(t, out MethodData[] result))
            {
                var methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                List<MethodData> evts = new List<MethodData>();
                for (int i = 0; i < methods.Length; i++)
                {
                    var method = methods[i];

                    var mas = method.GetCustomAttributes(typeof(MsgAttribute), false);
                    if (mas.Length > 0)
                    {
                        var ps = method.GetParameters();

                        MsgAttribute a = (MsgAttribute)mas[0];
                        MethodData e = new MethodData();
                        e.info = method;
                        e.type = 0;
                        e.opCode = a.OpCode;
                        e.sortOrder = a.SortOrder;
                        if (ps.Length == 0)
                            e.pCnt = 0;
                        else if (ps.Length == 1 && ps[0].ParameterType == typeof(IMessage))
                            e.pCnt = 1;
                        else
                        {
                            Loger.Error("无法解析的参数类型 class:" + t.FullName + " method:" + method.Name);
                            continue;
                        }

                        evts.Add(e);
                    }
                    else
                    {
                        var eas = method.GetCustomAttributes(typeof(EventAttribute), false);
                        if (eas.Length > 0)
                        {
                            var ps = method.GetParameters();

                            EventAttribute a = (EventAttribute)mas[0];
                            MethodData e = new MethodData();
                            e.info = method;
                            e.type = 1;
                            e.eventID = a.EventID;
                            e.sortOrder = a.SortOrder;
                            if (ps.Length == 0)
                                e.pCnt = 0;
                            else if (ps.Length == 1 && ps[0].ParameterType == typeof(object))
                                e.pCnt = 1;
                            else
                            {
                                Loger.Error("无法解析的参数类型 class:" + t.FullName + " method:" + method.Name);
                                continue;
                            }

                            evts.Add(e);
                        }
                    }
                }
                result = _methodCache[t] = evts.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 反射注册所有静态函数的消息和事件监听
        /// </summary>
        internal static void RigisterAllStaticListener()
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

                    var mas = method.GetCustomAttributes(typeof(MsgAttribute), false);
                    if (mas.Length > 0)
                    {
                        var a = mas[0] as MsgAttribute;
                        if (!_msgMap.TryGetValue(a.OpCode, out var evts))
                        {
                            evts = new List<MsgData>();
                            _msgMap[a.OpCode] = evts;
                        }

                        var ps = method.GetParameters();
                        MsgData e = new MsgData();
                        e.isCanceled = false;
                        e.sortOrder = a.SortOrder;
                        if (ps.Length == 0)
                        {
                            e.isP0 = true;
                            e.action0 = (Action)method.CreateDelegate(typeof(Action));
                        }
                        else if (ps.Length == 1 && ps[0].ParameterType == typeof(IMessage))
                        {
                            e.isP0 = false;
                            e.action1 = (Action<IMessage>)method.CreateDelegate(typeof(Action<IMessage>));
                        }
                        else
                        {
                            Loger.Error("参数类型不正确  class:" + type.Name + "  method:" + method.Name);
                            continue;
                        }

                        if (evts.Count == 0 || a.SortOrder >= evts[evts.Count - 1].sortOrder)
                            evts.Add(e);
                        else
                        {
                            int idx = evts.FindLastIndex(t => t.sortOrder < a.SortOrder);
                            if (idx == -1) idx = 0;
                            evts.Insert(idx, e);
                        }
                    }
                    else
                    {
                        var eas = method.GetCustomAttributes(typeof(EventAttribute), false);
                        if (eas.Length > 0)
                        {
                            var a = eas[0] as EventAttribute;
                            if (!_evtMap.TryGetValue(a.EventID, out var evts))
                            {
                                evts = new List<EvtData>();
                                _evtMap[a.EventID] = evts;
                            }

                            var ps = method.GetParameters();
                            EvtData e = new EvtData();
                            e.isCanceled = false;
                            e.sortOrder = a.SortOrder;
                            if (ps.Length == 0)
                            {
                                e.isP0 = true;
                                e.action0 = (Action)method.CreateDelegate(typeof(Action));
                            }
                            else if (ps.Length == 1 && ps[0].ParameterType == typeof(object))
                            {
                                e.isP0 = false;
                                e.action1 = (Action<object>)method.CreateDelegate(typeof(Action<object>));
                            }
                            else
                            {
                                Loger.Error("参数类型不正确  class:" + type.Name + "  method:" + method.Name);
                                continue;
                            }

                            if (evts.Count == 0 || a.SortOrder >= evts[evts.Count - 1].sortOrder)
                                evts.Add(e);
                            else
                            {
                                int idx = evts.FindLastIndex(t => t.sortOrder < a.SortOrder);
                                if (idx == -1) idx = 0;
                                evts.Insert(idx, e);
                            }
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
            MethodData[] ms = _getFilterMethods(target.GetType());
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

                    MsgData e = new MsgData();
                    e.isCanceled = false;
                    e.sortOrder = m.sortOrder;
                    if (m.pCnt == 0)
                    {
                        e.isP0 = true;
                        e.action0 = (Action)m.info.CreateDelegate(typeof(Action), target);
                    }
                    else if (m.pCnt == 1)
                    {
                        e.isP0 = false;
                        e.action1 = (Action<IMessage>)m.info.CreateDelegate(typeof(Action<IMessage>), target);
                    }

                    if (evts.Count == 0 || m.sortOrder >= evts[evts.Count - 1].sortOrder)
                        evts.Add(e);
                    else
                    {
                        int idx = evts.FindLastIndex(t => t.sortOrder < m.sortOrder);
                        if (idx == -1) idx = 0;
                        if (_isCalling && _isCallMsg && _isCallID == m.opCode && idx < _isCallIndex) _isCallIndex++;
                        evts.Insert(idx, e);
                    }
                }
                else if (m.type == 1)
                {
                    if (!_evtMap.TryGetValue(m.eventID, out var evts))
                    {
                        evts = new List<EvtData>();
                        _evtMap[m.eventID] = evts;
                    }

                    EvtData e = new EvtData();
                    e.isCanceled = false;
                    e.sortOrder = m.sortOrder;
                    if (m.pCnt == 0)
                    {
                        e.isP0 = true;
                        e.action0 = (Action)m.info.CreateDelegate(typeof(Action), target);
                    }
                    else if (m.pCnt == 1)
                    {
                        e.isP0 = false;
                        e.action1 = (Action<object>)m.info.CreateDelegate(typeof(Action<object>), target);
                    }

                    if (evts.Count == 0 || m.sortOrder >= evts[evts.Count - 1].sortOrder)
                        evts.Add(e);
                    else
                    {
                        int idx = evts.FindLastIndex(t => t.sortOrder < m.sortOrder);
                        if (idx == -1) idx = 0;
                        if (_isCalling && !_isCallMsg && _isCallID == m.eventID && idx < _isCallIndex) _isCallIndex++;
                        evts.Insert(idx, e);
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
            if (_methodCache.TryGetValue(target.GetType(), out MethodData[] ms))
            {
                for (int i = 0; i < ms.Length; i++)
                {
                    MethodData m = ms[i];
                    if (m.type == 0)
                    {
                        var lst = _msgMap[m.opCode];
                        for (int j = lst.Count - 1; j >= 0; j--)
                        {
                            if ((m.pCnt == 0 && lst[j].action0.Target == target)
                                || (m.pCnt == 1 && lst[j].action1.Target == target))
                            {
                                if (_isCalling) lst[j].isCanceled = true;
                                else lst.RemoveAt(j);
                            }
                        }
                    }
                    else if (m.type == 1)
                    {
                        var lst = _evtMap[m.eventID];
                        for (int j = lst.Count - 1; j >= 0; j--)
                        {
                            if ((m.pCnt == 0 && lst[j].action0.Target == target)
                                || (m.pCnt == 1 && lst[j].action1.Target == target))
                            {
                                if (_isCalling) lst[j].isCanceled = true;
                                else lst.RemoveAt(j);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 推送接受到的网络消息
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="message"></param>
        public static bool ExcuteMessage(int opCode, IMessage message = null)
        {
            bool hasEvt = false;
            if (_msgMap.TryGetValue(opCode, out var evts))
            {
                hasEvt = true;
                _isCalling = true;
                _isCallMsg = true;
                _isCallID = opCode;
                _isCallIndex = 0;
                for (; _isCallIndex < evts.Count; _isCallIndex++)
                {
                    MsgData e = evts[_isCallIndex];
                    if (e.isCanceled) continue;
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
                _isCalling = false;
            }
            return hasEvt;
        }

        /// <summary>
        /// 推送事件
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="data"></param>
        public static void ExcuteEvent(int eventID, object data = null)
        {
            if (_evtMap.TryGetValue(eventID, out var evts))
            {
                _isCalling = true;
                _isCallMsg = false;
                _isCallID = eventID;
                _isCallIndex = 0;
                for (; _isCallIndex < evts.Count; _isCallIndex++)
                {
                    EvtData e = evts[_isCallIndex];
                    if (e.isCanceled) continue;
                    try
                    {
                        if (e.isP0) e.action0();
                        else e.action1(data);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("事件执行出错 error:" + ex.ToString());
                    }
                }
                _isCalling = false;
            }
        }
    }
}
