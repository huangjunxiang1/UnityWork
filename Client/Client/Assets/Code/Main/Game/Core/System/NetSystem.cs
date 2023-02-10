using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Main;

public enum NetType
{
    TCP,
    KCP,
}
namespace Game
{
    public class NetSystem
    {
        class Engine : MonoBehaviour
        {
            public NetSystem net;
            private void Update()
            {
                net.Update();
            }
        }

        public NetSystem()
        {
            _engine = new GameObject($"[{nameof(NetSystem)}]");
            _engine.AddComponent<Engine>().net = this;
            GameObject.DontDestroyOnLoad(_engine);
        }

        GameObject _engine;
        AService _Service;
        long _ChannelID;
        readonly Dictionary<Type, Queue<TaskAwaiter<IMessage>>> _requestTask = new();
        Queue<TaskAwaiter<IMessage>> _swap = new();

        void _onError(long channelId, int error)
        {
            Loger.Error("Net Error Code:" + error);
            _ChannelID = 0;
            GameM.Event.RunEvent((int)EventIDM.NetError, error);
        }
        void _onResponse(long channelId, MemoryStream memoryStream)
        {
            ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.KcpOpcodeIndex);
            Type type = Types.GetOPType(opcode);
            bool hasRsp = type != null;
            IMessage message = null;
            if (hasRsp)
            {
                message = (IMessage)ProtoBuf.Serializer.Deserialize(type, memoryStream);

                if (opcode != OuterOpcode.G2C_Ping)
                    PrintField.Print($"收到消息 opCode:" + opcode + "  content:{0}", message);
            }

            //自动注册的事件一般是底层事件 所以先执行底层监听
            bool has = GameM.Event.RunMsg(opcode, message);
#if DebugEnable
            if (!has && (hasRsp && !_requestTask.ContainsKey(type)))
                Loger.Error("没有注册的消息返回 opCode:" + opcode + "  msg:" + type);
#endif

            if (hasRsp)
            {
                if (_requestTask.TryGetValue(type, out var queue))
                {
                    //防止TrySetResult执行过程中 有另外的异步发送同时执行
                    _requestTask[type] = _swap;
                    while (queue.Count > 0)
                        queue.Dequeue().TrySetResult(message);
                    _swap = queue;
                }
            }
        }

        /// <summary>
        /// 链接服务器IP
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ipEndPoint"></param>
        public void Connect(NetType type, IPEndPoint ipEndPoint)
        {
            _Service?.Dispose();
            switch (type)
            {
                case NetType.TCP:
                    _Service = new TService(ThreadSynchronizationContext.Instance, ServiceType.Outer);
                    break;
                case NetType.KCP:
                    _Service = new KService(ThreadSynchronizationContext.Instance, ServiceType.Outer);
                    break;
                default:
                    Loger.Error($"未识别的链接类型->{type}");
                    return;
            }
            _Service.ErrorCallback += _onError;
            _Service.ReadCallback += _onResponse;

            byte[] byte8 = new byte[8];
            System.Random random = new(Guid.NewGuid().GetHashCode());
            random.NextBytes(byte8);
            _ChannelID = BitConverter.ToInt64(byte8, 0);
            _Service.GetOrCreate(_ChannelID, ipEndPoint);
        }

        /// <summary>
        /// 非返回的消息发送
        /// </summary>
        /// <param name="message"></param>
        public void Send(IRequest message)
        {
            Send(0, message);
        }
        public void Send(long actorId, IRequest message)
        {
            if (_ChannelID == 0) return;
            var ms = new MemoryStream(Packet.OpcodeLength);
            ms.Seek(Packet.OpcodeLength, SeekOrigin.Begin);
            ms.SetLength(Packet.OpcodeLength);
            ushort opCode = Types.GetOPCode(message.GetType());
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
        public TaskAwaiter<IMessage> SendAsync(IRequest request)
        {
            return SendAsync(0, request);
        }
        public TaskAwaiter<IMessage> SendAsync(long actorId, IRequest request)
        {
            Type t;
#if ILRuntime
            if (request is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilRequest)
                t = ilRequest.ILInstance.Type.ReflectionType;
            else
#endif
            t = request.GetType();

            var rsp = Types.GetResponseType(t);
            if (rsp == null)
            {
                Loger.Error("没有responseType类型 req=" + t);
                return null;
            }
            if (!_requestTask.TryGetValue(rsp, out Queue<TaskAwaiter<IMessage>> queue))
            {
                queue = new Queue<TaskAwaiter<IMessage>>();
                _requestTask[rsp] = queue;
            }
            TaskAwaiter<IMessage> task = new();
            queue.Enqueue(task);
            Send(actorId, request);
            return task;
        }

        public TaskAwaiter<IMessage> SendAsync(IRequest request, TaskManager taskManager)
        {
            return SendAsync(0, request, taskManager);
        }
        public TaskAwaiter<IMessage> SendAsync(long actorId, IRequest request, TaskManager taskManager)
        {
            Type t;
#if ILRuntime
            if (request is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilRequest)
                t = ilRequest.ILInstance.Type.ReflectionType;
            else
#endif
            t = request.GetType();

            var rsp = Types.GetResponseType(t);
            if (rsp == null)
            {
                Loger.Error("没有responseType类型 req=" + t);
                return null;
            }
            if (!_requestTask.TryGetValue(rsp, out Queue<TaskAwaiter<IMessage>> queue))
            {
                queue = new Queue<TaskAwaiter<IMessage>>();
                _requestTask[rsp] = queue;
            }
            TaskAwaiter<IMessage> task = taskManager.Create<IMessage>();
            queue.Enqueue(task);
            Send(actorId, request);
            return task;
        }

        /// <summary>
        /// 断开当前链接
        /// </summary>
        public void DisConnect()
        {
            if (_ChannelID == 0) return;
            _Service.Remove(_ChannelID);
            _ChannelID = 0;
        }

        public void Dispose()
        {
            _Service?.Dispose();
            _ChannelID = 0;
            _requestTask.Clear();
            GameObject.DestroyImmediate(_engine);
        }

        /// <summary>
        /// 更新网络状态和数据
        /// </summary>
        void Update()
        {
            if (_ChannelID == 0) return;
            _Service.Update();
        }
    }
}
