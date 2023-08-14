using PB;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Main
{
    public class TCP : BaseNet
    {
        public TCP(IPEndPoint ip) : base(ip)
        {
            _client = new TcpClient();
            _client.NoDelay = true;
            _client.ReceiveTimeout = 3000;
            _client.ReceiveBufferSize = ushort.MaxValue;
            _client.SendTimeout = 3000;
            _client.SendBufferSize = ushort.MaxValue;
        }

        bool _sendHeart = false;
        TcpClient _client;
        Task _connectTask;
        byte[] _heart = new byte[8] { 8, 0, 0x03 + 0xE8, 0, 0xE8, 0x03, 0, 0 };

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
            return _client.Connected;
        }

        public override void DisConnect()
        {
            if (!_client.Connected)
                return;
            _client.Close();
            _client.Dispose();
            states = NetStates.None;
        }

        public override void Error(NetError error, Exception ex)
        {
            //除了解析出错 其他都是非正常出错
            if (error != NetError.ParseError)
            {
                _client.Dispose();
                states = NetStates.None;
            }
            Loger.Error($"网络错误 error={ex} \n stack={Loger.GetStackTrace()}");
            ThreadSynchronizationContext.Instance.Post(() => onError?.Invoke((int)error));
        }

        protected override async void SendBuffer()
        {
            PBWriter writer = new PBWriter(new MemoryStream(new byte[1024], 0, 1024, true, true));
            while (states != NetStates.None)
            {
                if (_sendHeart)
                {
                    try
                    {
                        await _client.GetStream().WriteAsync(_heart, 0, _heart.Length);
                    }
                    catch (Exception e)
                    {
                        //被动断开链接
                        if (states != NetStates.None)
                            this.DisConnect();
                        Loger.Error("发送消息错误 ex=" + e);
                        return;
                    }
                    _sendHeart = false;
                }
                while (queues.TryDequeue(out var message))
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
                            Loger.Error($"数据过大 len={len}");
                            continue;
                        }

                        var bs = (writer.Stream as MemoryStream).GetBuffer();
                        uint cmd = Types.GetCMDCode(message.GetType());
                        bs[0] = (byte)(len - 2);
                        bs[1] = (byte)((len - 2) >> 8);
                        bs[3] = (byte)(message.rpc > 0 ? 1 : 0);
                        bs[4] = (byte)cmd;
                        bs[5] = (byte)(cmd >> 8);
                        bs[6] = (byte)(cmd >> 16);
                        bs[7] = (byte)(cmd >> 24);
                        if (message.rpc > 0)
                        {
                            bs[8] = (byte)message.rpc;
                            bs[9] = (byte)(message.rpc >> 8);
                            bs[10] = (byte)(message.rpc >> 16);
                            bs[11] = (byte)(message.rpc >> 24);
                        }
                        byte checkCode = 0;
                        for (int i = 3; i < len; i++)
                            checkCode += bs[i];
                        bs[2] = (byte)(~checkCode + 1);

                        await _client.GetStream().WriteAsync(bs, 0, len);
                    }
                    catch (Exception e)
                    { 
                        //被动断开链接
                        if (states != NetStates.None)
                            this.DisConnect();
                        Loger.Error("发送消息错误 ex=" + e);
                        return;
                    }
                }
                Thread.Sleep(1);
            }
        }

        protected override async void ReceiveBuffer()
        {
            var bs = ArrayPool<byte>.Shared.Rent(ushort.MaxValue);
            PBReader reader = new PBReader(new MemoryStream(bs, 0, bs.Length), 0, bs.Length);
            while (states != NetStates.None)
            {
                try
                {
                    int len;
                    try
                    {
                        await _client.GetStream().ReadAsync(bs, 0, 2);
                        len = (bs[0] | bs[1] << 8) + 2;

                        if (len < 8)
                        {
                            Error(NetError.DataError, new Exception($"数据长度不对 len={len}"));
                            break;
                        }

                        await _client.GetStream().ReadAsync(bs, 2, len - 2);
                    }
                    catch (Exception ex)
                    {
                        //被动断开链接
                        if (states != NetStates.None)
                            this.DisConnect();
                        Error(NetError.ReadError, ex);
                        break;
                    }

                    uint cmd = bs[4]
                        | (uint)bs[5] << 8
                        | (uint)bs[6] << 16
                        | (uint)bs[7] << 24;

                    byte checkCode = bs[2];
                    for (int i = 3; i < len; i++)
                        checkCode += bs[i];

                    if (checkCode != 0)
                    {
                        Error(NetError.DataError, new Exception($"数据校验不正确 cmd:[{(ushort)cmd},{cmd >> 16}]"));
                        break;
                    }

                    byte msgType = bs[3];
                    uint rpcid = 0;
                    if (msgType == 1)
                    {
                        rpcid = bs[8]
                            | (uint)bs[9] << 8
                            | (uint)bs[10] << 16
                            | (uint)bs[11] << 24;
                    }

                    //心跳
                    if (cmd == 1 << 16)
                    {
                        _sendHeart = true;
                        continue;
                    }
                    try
                    {
                        Type t = Types.GetCMDType(cmd);
                        int index = msgType == 0 ? 8 : 12;
                        reader.SetLimit(index, len);
                        reader.Seek(index);
                        var msg = (PB.PBMessage)Activator.CreateInstance(t);
                        msg.rpc = rpcid;
                        msg.Read(reader);

                        onReceive(msg);
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
            ArrayPool<byte>.Shared.Return(bs);
        }
    }
}
