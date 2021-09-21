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
        static AService Service;
        static long ChannelID;
        static Dictionary<Type, Queue<TaskCompletionSource<IMessage>>> asyncResponseTask  = new Dictionary<Type, Queue<TaskCompletionSource<IMessage>>>();

        public static void Connect(NetType type, IPEndPoint ipEndPoint)
        {
            switch (type)
            {
                case NetType.TCP:
                    {
                        Service?.Dispose();
                        Service = new TService(ThreadSynchronizationContext.Instance, ServiceType.Outer);
                        Service.ErrorCallback += OnError;
                        Service.ReadCallback += OnRead;

                        byte[] byte8 = new byte[8];
                        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
                        random.NextBytes(byte8);
                        ChannelID = BitConverter.ToInt64(byte8, 0);
                        Service.GetOrCreate(ChannelID, ipEndPoint);
                    }
                    break;
                case NetType.KCP:
                    {
                        Service?.Dispose();
                        Service = new KService(ThreadSynchronizationContext.Instance, ServiceType.Outer);
                        Service.ErrorCallback += OnError;
                        Service.ReadCallback += OnRead;

                        byte[] byte8 = new byte[8];
                        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
                        random.NextBytes(byte8);
                        ChannelID = BitConverter.ToInt64(byte8, 0);
                        Service.GetOrCreate(ChannelID, ipEndPoint);
                    }
                    break;
                default:
                    break;
            }
        }

        static void OnError(long channelId, int error)
        {
            Loger.Error("Net Error Code:" + error);
            ChannelID = 0;
        }
        static void OnRead(long channelId, MemoryStream memoryStream)
        {
            ushort opcode = BitConverter.ToUInt16(memoryStream.GetBuffer(), Packet.KcpOpcodeIndex);
            Type type = TypesCache.GetOPType(opcode);
            if (type == null)
            {
                Loger.Error("未知返回类型 code:" + opcode);
                return;
            }
            IMessage message = (IMessage)ProtoBuf.Serializer.Deserialize(type, memoryStream);

            //自动注册的事件一般是底层事件 所以先执行底层监听
            SysEvent.Excute(message);

            if (asyncResponseTask.TryGetValue(message.GetType(), out var queue))
            {
                while (queue.Count > 0)
                    queue.Dequeue().TrySetResult(message);
            }
        }
       
        public static void Send(IRequest message)
        {
            Send(0,message);
        }
        public static void Send(long actorId, IRequest message)
        {
            var ms = new MemoryStream(Packet.OpcodeLength);
            ms.Seek(Packet.OpcodeLength, SeekOrigin.Begin);
            ms.SetLength(Packet.OpcodeLength);
            ushort opCode =TypesCache.GetOPCode(message.GetType());
            ms.GetBuffer().WriteTo(0, opCode);
            ProtoBuf.Serializer.Serialize(ms, message);
            ms.Seek(0, SeekOrigin.Begin);
            Service.SendStream(ChannelID, actorId, ms);
        }
        public static Task<IMessage> SendAsync(IRequest message)
        {
            return SendAsync(0, message);
        }
        public static Task<IMessage> SendAsync(long actorId, IRequest message)
        {
            TaskCompletionSource<IMessage> task = new TaskCompletionSource<IMessage>();
            var responseType = TypesCache.GetResponseType(message.GetType());
            if (!asyncResponseTask .TryGetValue(responseType, out var queue))
            {
                queue = new Queue<TaskCompletionSource<IMessage>>();
                asyncResponseTask[responseType] = queue;
            }
            queue.Enqueue(task);
            Send(actorId, message);
            return task.Task;
        }

        public static void DisConnect()
        {
            if (ChannelID == 0) return;
            Service.Remove(ChannelID);
            ChannelID = 0;
        }

        public static void Update()
        {
            if (ChannelID == 0) return;
            Service.Update();
        }

    }
}
