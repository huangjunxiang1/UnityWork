using PB;
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
                        int read = 0;
                        while (read < 2)
                            read += await _client.GetStream().ReadAsync(_rBuffer, read, 2 - read);
                        len = (_rBuffer[0] | _rBuffer[1] << 8) + 2;

                        if (len < 8 || len > ushort.MaxValue)
                        {
                            Error(NetError.DataError, new Exception($"数据长度不对 len={len}"));
                            break;
                        }

                        read = 2;
                        while (read < len)
                            read += await _client.GetStream().ReadAsync(_rBuffer, read, len - read);
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
