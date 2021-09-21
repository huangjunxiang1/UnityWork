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
        struct ResponseAction
        {
            public bool isDispose;
            public Action<IMessage> action;
        }
        readonly static Dictionary<Type, List<ResponseAction>> _ResponseStaticAction = new Dictionary<Type, List<ResponseAction>>();
        static bool _isExcuteEvent = false;

        public static void AutoRigisterEvent()
        {
            int len = TypesCache.Types.Count;
            for (int i = 0; i < len; i++)
            {
                Type type = TypesCache.Types[i];
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                int len1 = methods.Length;
                for (int j = 0; j < len1; j++)
                {
                    var method = methods[j];
                    var atts = method.GetCustomAttributes(typeof(EventAttribute), false);
                    if (atts.Length <= 0)
                        continue;
                    var att = atts[0] as EventAttribute;
                    if (att.EventType != EventType.AutoRigister)
                        continue;

                    var parameters = method.GetParameters();
                    if (parameters.Length != 1 || parameters[0].ParameterType != typeof(IResponse))
                    {
                        Loger.Error("自动事件函数参数类型不正确  class:" + type.Name + "  method:" + method.Name);
                        continue;
                    }
                    if (!_ResponseStaticAction.TryGetValue(att.ResponseType, out var actions))
                    {
                        actions = new List<ResponseAction>();
                        _ResponseStaticAction[att.ResponseType] = actions;
                    }
                    ResponseAction resAct = new ResponseAction();
                    resAct.isDispose = false;
                    resAct.action = (Action<IMessage>)method.CreateDelegate(typeof(Action<IMessage>));
                    actions.Add(resAct);
                }
            }
        }
        public static void RigisterEvent(object target)
        {
            var methods = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < methods.Length; i++)
            {
                var method = methods[i];
                var atts = method.GetCustomAttributes(typeof(EventAttribute), false);
                if (atts.Length <= 0)
                    continue;
                var att = atts[0] as EventAttribute;
                if (att.EventType != EventType.NoAutoRigister)
                    continue;

                var parameters = method.GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(IResponse))
                {
                    Loger.Error("自动事件函数参数类型不正确  class:" + target.GetType().Name + "  method:" + method.Name);
                    continue;
                }
                if (!_ResponseStaticAction.TryGetValue(att.ResponseType, out var actions))
                {
                    actions = new List<ResponseAction>();
                    _ResponseStaticAction[att.ResponseType] = actions;
                }
                ResponseAction resAct = new ResponseAction();
                resAct.isDispose = false;
                resAct.action = (Action<IMessage>)method.CreateDelegate(typeof(Action<IMessage>), target);
                actions.Add(resAct);
            }
        }
        public static void CancelEvent(object target)
        {
            var methods = target.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            for (int i = 0; i < methods.Length; i++)
            {
                var method = methods[i];
                var atts = method.GetCustomAttributes(typeof(EventAttribute), false);
                if (atts.Length <= 0)
                    continue;
                var att = atts[0] as EventAttribute;
                if (att.EventType != EventType.NoAutoRigister)
                    continue;

                var parameters = method.GetParameters();
                if (parameters.Length != 1 || parameters[0].ParameterType != typeof(IResponse))
                {
                    Loger.Error("自动事件函数参数类型不正确  class:" + target.GetType().Name + "  method:" + method.Name);
                    continue;
                }
                if (_ResponseStaticAction.TryGetValue(att.ResponseType, out var actions))
                {
                    int len = actions.Count;
                    for (int j = len - 1; j >= 0; j--)
                    {
                        if (actions[j].action.Method == method)
                        {
                            if (_isExcuteEvent)
                            {
                                ResponseAction resAct = actions[j];
                                resAct.isDispose = true;
                                actions[j] = resAct;
                            }
                            else
                                actions.RemoveAt(j);
                        }
                    }
                }
            }
        }

        public static void Excute(IMessage message)
        {
            if (_ResponseStaticAction.TryGetValue(message.GetType(), out var actions))
            {
                _isExcuteEvent = true;
                int len = actions.Count;
                for (int i = 0; i < len; i++)
                {
                    ResponseAction resAct = actions[i];
                    if (resAct.isDispose) continue;
                    try
                    {
                        resAct.action(message);
                    }
                    catch (Exception ex)
                    {
                        Loger.Error("事件执行出错 error:" + ex.ToString());
                    }
                }
                _isExcuteEvent = false;
            }
        }
    }
}
