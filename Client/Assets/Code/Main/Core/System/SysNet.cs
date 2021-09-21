using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            object response = ProtoBuf.Serializer.Deserialize(MessageTypeCache.GetopType(opcode), memoryStream);
            MInit.GetRecv(response);
        }
       
        public static void Send(IMessage message)
        {
            var ms = new MemoryStream(Packet.OpcodeLength);
            ms.Seek(Packet.OpcodeLength, SeekOrigin.Begin);
            ms.SetLength(Packet.OpcodeLength);
            ushort opCode = MessageTypeCache.GetopCode(message.GetType());
            ms.GetBuffer().WriteTo(0, opCode);
            ProtoBuf.Serializer.Serialize(ms, message);
            ms.Seek(0, SeekOrigin.Begin);
            Service.SendStream(ChannelID, 0, ms);
        }
        public static void Send(long actorId, IMessage message)
        {
            var ms = new MemoryStream(Packet.OpcodeLength);
            ms.Seek(Packet.OpcodeLength, SeekOrigin.Begin);
            ms.SetLength(Packet.OpcodeLength);
            ushort opCode = MessageTypeCache.GetopCode(message.GetType());
            ms.GetBuffer().WriteTo(0, opCode);
            ProtoBuf.Serializer.Serialize(ms, message);
            ms.Seek(0, SeekOrigin.Begin);
            Service.SendStream(ChannelID, actorId, ms);
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
