using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Main
{
    public enum NetType
    {
        TCP,
        KCP,
    }
    public static class SysNet
    {
        static AService _Service;
        static long _ChannelID;
        static Dictionary<Type, Queue<TaskAwaiter<IMessage>>> _requestTask = new Dictionary<Type, Queue<TaskAwaiter<IMessage>>>();

        static void _onError(long channelId, int error)
        {
            Loger.Error("Net Error Code:" + error);
            _ChannelID = 0;
            SysEvent.ExcuteEvent((int)EventIDM.NetError, error);
        }
        static void _onResponse(long channelId, MemoryStream memoryStream)
        {
            ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.KcpOpcodeIndex);
            Type type = TypesCache.GetOPType(opcode);
            bool hasRsp = type != null;
            IMessage message = null;
            if (hasRsp)
            {
                message = (IMessage)ProtoBuf.Serializer.Deserialize(type, memoryStream);
                if (opcode != OuterOpcode.G2C_Ping)
                    PrintField.Print($"收到消息 opCode:" + opcode + "  content:{0}", message);
            }
            else
                Loger.Log($"收到消息 opCode:{opcode}");

            //自动注册的事件一般是底层事件 所以先执行底层监听
            bool has = SysEvent.ExcuteMessage(opcode, message);
            if (AppSetting.Debug)
            {
                if (!has && (hasRsp && !_requestTask.ContainsKey(type)))
                    Loger.Error("没有注册的消息返回 msgID:" + opcode + "  msg:" + type);
            }

            if (hasRsp)
            {
                if (_requestTask.TryGetValue(type, out var queue))
                {
                    while (queue.Count > 0)
                        queue.Dequeue().TrySetResult(message);
                }
            }
        }

        /// <summary>
        /// 链接服务器IP
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ipEndPoint"></param>
        public static void Connect(NetType type, IPEndPoint ipEndPoint)
        {
            switch (type)
            {
                case NetType.TCP:
                    {
                        _Service?.Dispose();
                        _Service = new TService(ThreadSynchronizationContext.Instance, ServiceType.Outer);
                        _Service.ErrorCallback += _onError;
                        _Service.ReadCallback += _onResponse;

                        byte[] byte8 = new byte[8];
                        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
                        random.NextBytes(byte8);
                        _ChannelID = BitConverter.ToInt64(byte8, 0);
                        _Service.GetOrCreate(_ChannelID, ipEndPoint);
                    }
                    break;
                case NetType.KCP:
                    {
                        _Service?.Dispose();
                        _Service = new KService(ThreadSynchronizationContext.Instance, ServiceType.Outer);
                        _Service.ErrorCallback += _onError;
                        _Service.ReadCallback += _onResponse;

                        byte[] byte8 = new byte[8];
                        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
                        random.NextBytes(byte8);
                        _ChannelID = BitConverter.ToInt64(byte8, 0);
                        _Service.GetOrCreate(_ChannelID, ipEndPoint);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 非返回的消息发送
        /// </summary>
        /// <param name="message"></param>
        public static void Send(IRequest message)
        {
            Send(0, message);
        }
        public static void Send(long actorId, IRequest message)
        {
            var ms = new MemoryStream(Packet.OpcodeLength);
            ms.Seek(Packet.OpcodeLength, SeekOrigin.Begin);
            ms.SetLength(Packet.OpcodeLength);
            ushort opCode = TypesCache.GetOPCode(message.GetType());
            ms.GetBuffer().WriteTo(0, opCode);
            ProtoBuf.Serializer.Serialize(ms, message);
            ms.Seek(0, SeekOrigin.Begin);
            _Service.SendStream(_ChannelID, actorId, ms);

            if (opCode != OuterOpcode.C2G_Ping)
                PrintField.Print($"发送消息 opCode:" + opCode + "  content:{0}", message);
        }

        /// <summary>
        /// 异步返回的消息发送
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static TaskAwaiter<IMessage> SendAsync(IRequest request)
        {
            return SendAsync(0, request);
        }
        public static TaskAwaiter<IMessage> SendAsync(long actorId, IRequest request)
        {
            Type t;
#if ILRuntime
            if (request is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilRequest)
                t = ilRequest.ILInstance.Type.ReflectionType;
            else
#endif
                t = request.GetType();

            var responseType = TypesCache.GetResponseType(request.GetType());
            if (responseType == null)
            {
                Loger.Error("没有responseType类型");
                return null;
            }
            if (!_requestTask.TryGetValue(responseType, out Queue<TaskAwaiter<IMessage>> queue))
            {
                queue = new Queue<TaskAwaiter<IMessage>>();
                _requestTask[responseType] = queue;
            }
            TaskAwaiter<IMessage> task = new TaskAwaiter<IMessage>();
            queue.Enqueue(task);
            Send(actorId, request);
            return task;
        }
        public static TaskAwaiter<IMessage> SendAsync(IRequest request, Action<TaskAwaiter<IMessage>> onComplete)
        {
            return SendAsync(0, request, onComplete);
        }
        public static TaskAwaiter<IMessage> SendAsync(long actorId, IRequest request, Action<TaskAwaiter<IMessage>> onComplete)
        {
            Type t;
#if ILRuntime
            if (request is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilRequest)
                t = ilRequest.ILInstance.Type.ReflectionType;
            else
#endif
                t = request.GetType();

            var responseType = TypesCache.GetResponseType(t);
            if (responseType == null)
            {
                Loger.Error("没有responseType类型 requestType:"+ request.GetType());
                return null;
            }
            if (!_requestTask.TryGetValue(responseType, out Queue<TaskAwaiter<IMessage>> queue))
            {
                queue = new Queue<TaskAwaiter<IMessage>>();
                _requestTask[responseType] = queue;
            }
            TaskAwaiter<IMessage> task = new TaskAwaiter<IMessage>(onComplete);
            queue.Enqueue(task);
            Send(actorId, request);
            return task;
        }

        /// <summary>
        /// 断开当前链接
        /// </summary>
        public static void DisConnect()
        {
            if (_ChannelID == 0) return;
            _Service.Remove(_ChannelID);
            _ChannelID = 0;
        }

        /// <summary>
        /// 更新网络状态和数据
        /// </summary>
        public static void Update()
        {
            if (_ChannelID == 0) return;
            _Service.Update();
        }
    }
}
