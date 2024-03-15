using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game
{
    public static class Types
    {
        readonly static Dictionary<Type, uint> _typeCmd = new();
        readonly static Dictionary<uint, Type> _cmdType = new();
        readonly static Dictionary<Type, Type> _requestResponse = new();

        internal static void Parse(List<Type> types)
        {
            _typeCmd.Clear();
            _cmdType.Clear();
            _requestResponse.Clear();
            int len = types.Count;
            for (int i = 0; i < len; i++)
            {
                Type type = types[i];

                if (typeof(IMessage).IsAssignableFrom(type))
                {
                    var m = type.GetCustomAttribute<MessageAttribute>();
                    if (m != null)
                    {
                        uint cmd = m.cmd;
                        _typeCmd.Add(type, cmd);
                        _cmdType.Add(cmd, type);
                        if (m.ResponseType != null)
                            _requestResponse[type] = m.ResponseType;
                    }
                    continue;//协议类不需要进行下一步解析
                }
            }
        }
        public static uint GetCMDCode(Type type)
        {
            if (!_typeCmd.TryGetValue(type, out var code))
                Loger.Error("消息没有cmdCode type:" + type.FullName);
            return code;
        }
        public static Type GetCMDType(uint cmd)
        {
            if (!_cmdType.TryGetValue(cmd, out var type))
                Loger.Error($"cmd没有类型 cmd: main={(ushort)cmd} sub={cmd >> 16}");
            return type;
        }
        public static Type GetResponseType(Type request)
        {
            if (!_requestResponse.TryGetValue(request, out var type))
                Loger.Error("request没有Response  requestType:" + type.FullName);
            return type;
        }
    }
}
