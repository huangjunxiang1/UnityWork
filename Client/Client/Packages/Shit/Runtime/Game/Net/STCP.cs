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
            _client = new TcpClient
            {
                NoDelay = true,
                ReceiveTimeout = 3000,
                ReceiveBufferSize = ushort.MaxValue,
                SendTimeout = 3000,
                SendBufferSize = ushort.MaxValue,
            };
        }

        public STCP(TcpClient tcp)
            : base((IPEndPoint)(tcp?.Client?.RemoteEndPoint
                   ?? throw new ArgumentNullException(nameof(tcp))))
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
        NetworkStream _stream;
        readonly SemaphoreSlim _connectLock = new(1, 1); 
        int _disconnected;

        public override ServerType serverType => ServerType.TCP;

        public override async Task<bool> Connect()
        {
            if (_client.Connected) return true;

            await _connectLock.WaitAsync();
            try
            {
                if (_client.Connected) return true;

                await _client.ConnectAsync(IP.Address, IP.Port);
                if (!_client.Connected) return false;

                _stream = _client.GetStream();
                Interlocked.Exchange(ref _disconnected, 0);
                states = NetStates.Connect;

                return true;
            }
            catch (Exception e)
            {
                Loger.Error("Connect error: " + e);
                return false;
            }
            finally
            {
                _connectLock.Release();
            }
        }

        public override void DisConnect()
        {
            if (Interlocked.Exchange(ref _disconnected, 1) != 0) return;
            states = NetStates.None;

            try { _stream?.Dispose(); } catch { }
            try { _client?.Dispose(); } catch { }

            onDisconnect?.Invoke();
        }

        protected async override void ReceiveBuffer()
        {
            try
            {
                var stream = _stream ??= _client.GetStream();
                var reader = new PBReader(new MemoryStream(_rBuffer, 0, _rBuffer.Length), 0, _rBuffer.Length);

                while (states != NetStates.None)
                {
                    int len;
                    try
                    {
                        // 读 2 字节长度头
                        int offset = 0;
                        while (offset < 2)
                        {
                            int n = await stream.ReadAsync(_rBuffer, offset, 2 - offset).ConfigureAwait(false);
                            if (n == 0) { DisConnect(); return; }
                            offset += n;
                        }

                        len = (_rBuffer[0] | _rBuffer[1] << 8) + 2;

                        if (len < 8 || len > _rBuffer.Length)
                        {
                            Error(NetError.DataError, new Exception($"数据长度不对 len={len}"));
                            break;
                        }

                        // 继续读满整包
                        while (offset < len)
                        {
                            int n = await stream.ReadAsync(_rBuffer, offset, len - offset).ConfigureAwait(false);
                            if (n == 0) { DisConnect(); return; }
                            offset += n;
                        }
                    }
                    catch (ObjectDisposedException) { break; }
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
            }
            catch (Exception ex)
            {
                Loger.Error("ReceiveBuffer fatal: " + ex);
            }
        }

        protected async override void SendBuffer()
        {
            try
            {
                var stream = _stream ??= _client.GetStream();
                var writer = new PBWriter(new MemoryStream(_sBuffer, 0, _sBuffer.Length, true, true));

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

                            await stream.WriteAsync(_sBuffer, 0, len).ConfigureAwait(false);
                        }
                        catch (ObjectDisposedException)
                        {
                            return;
                        }
                        catch (Exception e)
                        {
                            if (states != NetStates.None)
                                this.DisConnect();
                            Loger.Error("Send message error :" + e);
                            return;
                        }
                    }

                    await Task.Delay(1).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Loger.Error("SendBuffer fatal: " + ex);
            }
        }
    }
}