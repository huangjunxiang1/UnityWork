﻿using PB;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Game
{
    public class STCP : SBaseNet
    {
        public STCP(IPEndPoint ip) : base(ip)
        {
            _client = new TcpClient();
            _client.NoDelay = true;
            _client.ReceiveTimeout = 3000;
            _client.ReceiveBufferSize = ushort.MaxValue;
            _client.SendTimeout = 3000;
            _client.SendBufferSize = ushort.MaxValue;
        }
        public STCP(TcpClient tcp) : base((IPEndPoint)tcp.Client.RemoteEndPoint)
        {
            _client = tcp;
            _client.NoDelay = true;
            _client.ReceiveTimeout = 3000;
            _client.ReceiveBufferSize = ushort.MaxValue;
            _client.SendTimeout = 3000;
            _client.SendBufferSize = ushort.MaxValue;
            states = tcp.Connected ? NetStates.Connect : NetStates.None;
        }

        TcpClient _client;
        Task _connectTask;

        public override ServerType serverType => ServerType.TCP;

        public override async Task<bool> Connect()
        {
            if (_client.Connected)
                return true;
            if (_connectTask != null)
            {
                await _connectTask;
                return _client.Connected;
            }
            await (_connectTask = _client.ConnectAsync(IP.Address, IP.Port));
            _connectTask = null;
            if (_client.Connected)
                states = NetStates.Connect;
            this.Work();
            return _client.Connected;
        }

        public override void DisConnect()
        {
            if (states == NetStates.None)
                return;
            states = NetStates.None;
            _client.Dispose();
            onDisconnect?.Invoke();
        }


        protected override async void ReceiveBuffer()
        {
            PBReader reader = new(new MemoryStream(_rBuffer, 0, _rBuffer.Length), 0, _rBuffer.Length);
            while (states != NetStates.None)
            {
                try
                {
                    int len;
                    try
                    {
                        int index = 0;
                        while (index < 2)
                            index += await _client.GetStream().ReadAsync(_rBuffer, index, 2 - index);
                        len = (_rBuffer[0] | _rBuffer[1] << 8) + 2;

                        if (len < 8 || len > ushort.MaxValue)
                        {
                            Error(NetError.DataError, new Exception($"数据长度不对 len={len}"));
                            break;
                        }

                        index = 2;
                        while (index < len)
                            index += await _client.GetStream().ReadAsync(_rBuffer, index, len - index);
                    }
                    catch (Exception ex)
                    {
                        Error(NetError.ReadError, ex);
                        break;
                    }

                    int cmd = _rBuffer[4]
                        | (int)_rBuffer[5] << 8
                        | (int)_rBuffer[6] << 16
                        | (int)_rBuffer[7] << 24;

                    byte checkCode = _rBuffer[2];
                    for (int i = 3; i < len; i++)
                        checkCode += _rBuffer[i];

                    if (checkCode != 0)
                    {
                        Error(NetError.DataError, new Exception($"数据校验不正确 cmd:[{(ushort)cmd},{cmd >> 16}]"));
                        break;
                    }

                    byte msgType = _rBuffer[3];
                    uint rpcid = 0;
                    if (msgType == 1)
                    {
                        rpcid = _rBuffer[8]
                            | (uint)_rBuffer[9] << 8
                            | (uint)_rBuffer[10] << 16
                            | (uint)_rBuffer[11] << 24;
                    }

                    try
                    {
                        Type t = MessageParser.GetCMDType(cmd);
                        int index = msgType == 0 ? 8 : 12;
                        reader.SetMinMax(index, len);
                        reader.Seek(index);
                        var msg = (PB.PBMessage)Activator.CreateInstance(t);
                        msg.rpc = rpcid;
                        msg.Read(reader);

                        this.ReceiveMessage(msg);
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
                        if (message.rpc > 0)
                            writer.Seek(12);
                        else
                            writer.Seek(8);

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

                        int cmd = MessageParser.GetCMDCode(message.GetType());
                        _sBuffer[0] = (byte)(len - 2);
                        _sBuffer[1] = (byte)((len - 2) >> 8);
                        _sBuffer[3] = (byte)(message.rpc > 0 ? 1 : 0);
                        _sBuffer[4] = (byte)cmd;
                        _sBuffer[5] = (byte)(cmd >> 8);
                        _sBuffer[6] = (byte)(cmd >> 16);
                        _sBuffer[7] = (byte)(cmd >> 24);
                        if (message.rpc > 0)
                        {
                            _sBuffer[8] = (byte)message.rpc;
                            _sBuffer[9] = (byte)(message.rpc >> 8);
                            _sBuffer[10] = (byte)(message.rpc >> 16);
                            _sBuffer[11] = (byte)(message.rpc >> 24);
                        }
                        byte checkCode = 0;
                        for (int i = 3; i < len; i++)
                            checkCode += _sBuffer[i];
                        _sBuffer[2] = (byte)(~checkCode + 1);

                        await _client.GetStream().WriteAsync(_sBuffer, 0, len);
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
    }
}
