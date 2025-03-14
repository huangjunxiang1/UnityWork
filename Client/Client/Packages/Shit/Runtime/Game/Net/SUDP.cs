﻿using PB;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public class SUDP : SBaseNet
    {
        public SUDP(IPEndPoint ip) : base(ip)
        {
            if (Socket.OSSupportsIPv4)
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            else if (Socket.OSSupportsIPv6)
                _socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
        }
        public SUDP(UdpClient udp) : base((IPEndPoint)udp.Client.RemoteEndPoint)
        {
            _socket = udp.Client;
            states = NetStates.Connect;
        }

        Socket _socket;
        int _sendLen;

        public override ServerType serverType => ServerType.UDP;

        public override async Task<bool> Connect()
        {
            if (_socket.Connected)
                return true;
            _socket.Connect(IP);
            await Task.CompletedTask;
            if (_socket.Connected)
                states = NetStates.Connect;
            return _socket.Connected;
        }

        public override void DisConnect()
        {
            if (states == NetStates.None)
                return;
            states = NetStates.None;
            _socket.Dispose();
            onDisconnect.Invoke();
        }

        protected async override void ReceiveBuffer()
        {
            PBReader reader = new(new MemoryStream(_rBuffer, 0, _rBuffer.Length), 0, _rBuffer.Length);
            while (states != NetStates.None)
            {
                try
                {
                    int len;
                    try
                    {
                        await Task<int>.Factory.FromAsync(BeginReceive, EndReceive, null);

                        len = (_rBuffer[0] | _rBuffer[1] << 8) + 2;

                        if (len < 8)
                        {
                            Error(NetError.DataError, new Exception($"数据长度不对 len={len}"));
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Error(NetError.ReadError, ex);
                        break;
                    }

                    reader.SetMax(len);
                    reader.Seek(3);
                    int cmd = reader.Readfixed32();

                    byte checkCode = _rBuffer[2];
                    for (int i = 3; i < len; i++)
                        checkCode += _rBuffer[i];

                    if (checkCode != 0)
                    {
                        Error(NetError.DataError, new Exception($"数据校验不正确 cmd:[{cmd}]"));
                        break;
                    }

                    try
                    {
                        Type t = MessageParser.GetCMDType(cmd);
                        var message = (PB.PBMessage)Activator.CreateInstance(t);
                        message.rpc = reader.Readint64();
                        message.actorId = reader.Readint64();
                        message.error = reader.Readstring();
                        message.Read(reader);

                        this.ReceiveMessage(message);
                    }
                    catch (Exception ex)
                    {
                        Error(NetError.ParseError, ex);
                    }
                }
                catch (Exception ex)
                {
                    Error(NetError.UnKnown, ex);
                    break;
                }
            }
        }

        protected override async void SendBuffer()
        {
            PBWriter writer = new(new MemoryStream(_sBuffer, 0, _sBuffer.Length, true, true));
            while (states != NetStates.None)
            {
                while (sendQueues.TryDequeue(out var message))
                {
                    try
                    {
                        int cmd = MessageParser.GetCMDCode(message.GetType());
                        writer.Seek(3);
                        writer.Writefixed32(cmd);
                        writer.Writeint64(message.rpc);
                        writer.Writeint64(message.actorId);
                        writer.Writestring(message.error);

                        try
                        {
                            message.Write(writer);
                        }
                        catch (Exception e)
                        {
                            Loger.Error("序列化出错 ex=" + e);
                            continue;
                        }

                        int len = writer.Position;
                        if (len > ushort.MaxValue)
                        {
                            Loger.Error($"数据过大 len={len}  class={message.GetType().FullName}");
                            continue;
                        }

                        _sBuffer[0] = (byte)(len - 2);
                        _sBuffer[1] = (byte)((len - 2) >> 8);

                        byte checkCode = 0;
                        for (int i = 3; i < len; i++)
                            checkCode += _sBuffer[i];
                        _sBuffer[2] = (byte)(~checkCode + 1);

                        await Task<int>.Factory.FromAsync(BeginSend, EndSend, null);
                    }
                    catch (Exception e)
                    {
                        //被动断开链接
                        if (states != NetStates.None)
                            this.DisConnect();
                        Loger.Error("Send message error :" + e);
                        return;
                    }
                }
                Thread.Sleep(1);
            }
        }

        IAsyncResult BeginSend(AsyncCallback callback, object state)
        {
            return _socket.BeginSendTo(_sBuffer, 0, _sendLen, SocketFlags.None, IP, callback, state);
        }
        int EndSend(IAsyncResult asyncResult)
        {
            return _socket.EndSend(asyncResult);
        }
        IAsyncResult BeginReceive(AsyncCallback callback, object state)
        {
            EndPoint ip = IP;
            return _socket.BeginReceiveFrom(_rBuffer, 0, _rBuffer.Length, SocketFlags.None, ref ip, callback, state);
        }
        int EndReceive(IAsyncResult asyncResult)
        {
            return _socket.EndReceive(asyncResult);
        }
    }
}
