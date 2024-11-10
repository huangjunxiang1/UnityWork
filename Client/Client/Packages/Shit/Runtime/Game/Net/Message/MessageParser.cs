using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game
{
    public static class MessageParser
    {
        readonly static Dictionary<Type, MessageAttribute> _typeCmd = new();
        readonly static Dictionary<int, Type> _cmdType = new();

        public static void Parse(List<Type> types)
        {
            _typeCmd.Clear();
            _cmdType.Clear();
            int len = types.Count;
            for (int i = 0; i < len; i++)
            {
                Type type = types[i];

                if (typeof(IMessage).IsAssignableFrom(type))
                {
                    var m = type.GetCustomAttribute<MessageAttribute>();
                    if (m != null)
                    {
                        int cmd = m.cmd;
                        _typeCmd.Add(type, m);
                        if (_cmdType.TryGetValue(cmd, out var type1))
                            Loger.Error($"cmd重复 {type1}  {type}");
                        else
                            _cmdType.Add(cmd, type);
                    }
                }
            }
        }
        public static int GetCMDCode(Type type)
        {
            if (!_typeCmd.TryGetValue(type, out var m))
                Loger.Error("消息没有cmdCode type:" + type.FullName);
            return m.cmd;
        }
        public static Type GetCMDType(int cmd)
        {
            if (!_cmdType.TryGetValue(cmd, out var type))
                Loger.Error($"cmd没有类型 cmd: [{cmd}]");
            return type;
        }
        public static Type GetResponseType(Type request)
        {
            if (!_typeCmd.TryGetValue(request, out var m) || m.ResponseType == null)
                Loger.Error("request没有Response  requestType:" + request.FullName);
            return m.ResponseType;
        }
    }
}
