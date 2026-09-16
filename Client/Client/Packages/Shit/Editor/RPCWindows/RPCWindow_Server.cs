using Newtonsoft;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

partial class RPCWindow
{
    internal class RPCWindow_Server
    {
        RPCWindow window;
        bool isDisposed = false;

        string localIP = "127.0.0.1";
        TcpListener _listener;
        bool _isRunning = false;
        int _serverPort = 8888;
        bool _enableVerifyCode = true;
        string _VerifyCode = "";
        List<RPCParamItem> _serverParams = new List<RPCParamItem>();
        bool _serverParamsExpanded = false;
        LogFilter _logFilter = LogFilter.All;
        string _LogSearch = "";
        ConcurrentDictionary<TcpClient, bool> clients = new();
        RemoteExcuteInfo currentExcuteInfo = new();
        SynchronizationContext _unityContext;

        Dictionary<string, List<(MethodInfo method, RPCAttribute attr)>> _rpcMethodsByRemote = null;
        Dictionary<string, MethodInfo> _rpcMethodsByQuery = new();
        public RPCWindow_Server(RPCWindow window)
        {
            this.window = window;
            _unityContext = SynchronizationContext.Current;

            _rpcMethodsByRemote = new Dictionary<string, List<(MethodInfo, RPCAttribute)>>();
#if UNITY_6000_0_OR_NEWER
            foreach (var asm in UnityEngine.Assemblies.CurrentAssemblies.GetLoadedAssemblies())
#else
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
#endif
            {
                foreach (var type in asm.GetTypes())
                {
                    {
                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                        .Where(m => m.GetCustomAttributes<RPCAttribute>().Any())
                        .ToList();

                        foreach (var method in methods)
                            foreach (var attr in method.GetCustomAttributes<RPCAttribute>())
                            {
                                if (string.IsNullOrEmpty(attr.Remote)) continue;
                                if (!_rpcMethodsByRemote.ContainsKey(attr.Remote))
                                    _rpcMethodsByRemote[attr.Remote] = new List<(MethodInfo, RPCAttribute)>();
                                if (!_rpcMethodsByRemote[attr.Remote].Any(t => t.Item1 == method && t.Item2.Remote == attr.Remote))
                                    _rpcMethodsByRemote[attr.Remote].Add((method, attr));
                            }
                    }
                    {
                        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                        .Where(m => m.GetCustomAttributes<RPCQueryAttribute>().Any())
                        .ToList();

                        foreach (var method in methods)
                            foreach (var attr in method.GetCustomAttributes<RPCQueryAttribute>())
                            {
                                if (string.IsNullOrEmpty(attr.Remote)) continue;
                                _rpcMethodsByQuery[attr.Remote] = method;
                            }
                    }
                }
            }
            foreach (var kv in _rpcMethodsByRemote)
                kv.Value.Sort((a, b) => a.attr.Order.CompareTo(b.attr.Order));
            AppendLog($"[RPC] 已注册 {_rpcMethodsByRemote.Count} 个 Remote 分组");

            getHostIP();

            _enableVerifyCode = EditorPrefs.GetBool(window.GetCombineKey("RPC_EnableVerifyCode"), true);

            string code = EditorPrefs.GetString(window.GetCombineKey("RPC_VerifyCode"), "");
            if (string.IsNullOrEmpty(code))
            {
                code = Guid.NewGuid().ToString("N");
                EditorPrefs.SetString(window.GetCombineKey("RPC_VerifyCode"), code);
            }
            _VerifyCode = code;

            if (EditorPrefs.HasKey(window.GetCombineKey("RPC_ServerPort")))
                _serverPort = EditorPrefs.GetInt(window.GetCombineKey("RPC_ServerPort"));
            else
            {
                _serverPort = UnityEngine.Random.Range(1024, 65535);
                EditorPrefs.SetInt(window.GetCombineKey("RPC_ServerPort"), _serverPort);
            }

            string json = EditorPrefs.GetString(window.GetCombineKey("RPC_ServerParams"), "");
            if (!string.IsNullOrEmpty(json))
                try { _serverParams = JsonConvert.DeserializeObject<List<RPCParamItem>>(json); }
                catch { _serverParams = new List<RPCParamItem>(); }
            else _serverParams = new List<RPCParamItem>();

            if (EditorPrefs.GetBool(window.GetCombineKey("RPC_ServerShouldRun"), false))
                StartServer();

            AssemblyReloadEvents.beforeAssemblyReload += reload;

            try
            {
                var currentExcuteInfo_json = SessionState.GetString("RPC_currentExcuteInfo", "");
                SessionState.EraseString("RPC_currentExcuteInfo");
                if (!string.IsNullOrEmpty(currentExcuteInfo_json))
                {
                    var info = JsonConvert.DeserializeObject<RemoteExcuteInfo>(currentExcuteInfo_json);

                    if (!info.isExecuting)
                        return;
                    if (!_rpcMethodsByRemote.TryGetValue(info.remote, out var list))
                        return;
                    if (list.Count != info.methods.Count)
                        return;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].method.Name != info.methods[i])
                            return;
                    }
                    currentExcuteInfo = info;
                    currentExcuteInfo.currentMethodIndex++;
                    reStatus();
                }
            }
            catch (Exception e)
            { AppendLog("重启状态失败 e=" + e); }
        }
        async void getHostIP()
        {
            try
            {
                var host = await Dns.GetHostEntryAsync(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        localIP = ip.ToString();
            }
            catch { }
        }
        private void AppendLog(string msg, LogType type = LogType.Log)
        {
            if (string.IsNullOrEmpty(msg)) return;
            msg = $"[{DateTime.Now:HH:mm:ss}]{msg.Trim('\0').Trim()}";
            var entry = new LogEntry() { message = msg, type = type };
            window._allLogs.Add(entry);
            window.WaitRepaint();
        }
        async void StartServer()
        {
            try
            {
                _listener?.Stop();
                _listener = new TcpListener(IPAddress.Any, _serverPort);
                _listener.Start();
                _isRunning = true;
                EditorPrefs.SetBool(window.GetCombineKey("RPC_ServerShouldRun"), true);
                AppendLog($"服务器启动，监听端口 {_serverPort}");
            }
            catch (Exception e)
            {
                AppendLog($"启动失败: {e.Message}", LogType.Error);
                return;
            }
            window.WaitRepaint();

            while (true)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    clients.TryAdd(client, true);
                    handleClient(client);
                }
                catch (Exception)
                { return; }
            }
        }
        void StopServer()
        {
            _isRunning = false;
            _listener?.Stop();
            AppendLog("停止服务");
        }
        async void handleClient(TcpClient tcp)
        {
            AppendLog($"[{tcp.Client.RemoteEndPoint as IPEndPoint}]连接到服务");
            byte[] buffer = new byte[2048];
            while (_isRunning)
            {
                try
                {
                    int len = await tcp.GetStream().ReadAsync(buffer, 0, 4).ConfigureAwait(false);
                    if (len <= 0)
                    {
                        AppendLog($"断开连接: {tcp.Client.RemoteEndPoint as IPEndPoint}", LogType.Log);
                        tcp.Close();
                        tcp.Dispose();
                        clients.TryRemove(tcp, out _);
                        return;
                    }
                    int byteCnt = buffer[0];
                    byteCnt |= buffer[1] << 8;
                    byteCnt |= buffer[2] << 16;
                    byteCnt |= buffer[3] << 32;
                    await tcp.GetStream().ReadAsync(buffer, 0, byteCnt);
                    var json = Encoding.UTF8.GetString(buffer, 0, byteCnt);

                    RPCCommand cmd;
                    try
                    {
                        cmd = JsonConvert.DeserializeObject<RPCCommand>(json);
                    }
                    catch (Exception)
                    {
                        AppendLog($"异常命令: {json}", LogType.Error);
                        RPCResult error = new() { code = (int)LogType.Error, message = $"异常命令: {json}" };
                        send(tcp, error);
                        continue;
                    }
                    AppendLog($"收到命令: {(RPCCmdId)cmd.cmd}");
                    RPCResult result = new() { cmd = cmd.cmd, requestId = cmd.requestId };
                    for (int i = 0; i < _serverParams.Count; i++)
                    {
                        if (_serverParams[i].enabled && !cmd.paramItems.ContainsKey(_serverParams[i].key))
                            cmd.paramItems[_serverParams[i].key] = _serverParams[i].value;
                    }
                    var args = new RPCArgs(cmd.paramItems);
                    if (string.IsNullOrEmpty(cmd.remote))
                    {
                        result.code = (int)LogType.Error;
                        result.message = "远程函数为空";
                        send(tcp, result);
                        continue;
                    }
                    switch ((RPCCmdId)cmd.cmd)
                    {
                        case RPCCmdId.GetStatus:
                            {
                                result.currentExcuteInfo = currentExcuteInfo;
                                if (_rpcMethodsByRemote.TryGetValue(cmd.remote, out var methods))
                                    result.methods.AddRange(methods.Select(t => t.method.Name));
                                if (_rpcMethodsByQuery.TryGetValue(cmd.remote, out var m))
                                {
                                    try
                                    {
                                        object ro = null;
                                        await window.WaitMainThread(() =>
                                        {
                                            var ps = m.GetParameters();
                                            if (ps.Length == 0)
                                                ro = m.Invoke(null, null);
                                            else if (ps.Length == 1 && ps[0].ParameterType == typeof(RPCArgs))
                                                ro = m.Invoke(null, new object[] { args });
                                            else
                                            {
                                                result.code = (int)LogType.Error;
                                                result.message = "远程函数参数定义不正确";
                                            }
                                        });
                                        if (result.code != -1)
                                        {
                                            send(tcp, result);
                                            continue;
                                        }
                                        result.envVar = ro as Dictionary<string, string>;
                                    }
                                    catch (Exception e)
                                    {
                                        result.code = (int)LogType.Error;
                                        result.message = "远程查询执行出错 e=" + e;
                                        send(tcp, result);
                                        continue;
                                    }
                                }
                                send(tcp, result);

                                if (currentExcuteInfo.isExecuting)
                                {
                                    result.code = (int)LogType.Log;
                                    result.message = $"正在执行({currentExcuteInfo.currentMethodIndex + 1}/{currentExcuteInfo.totalMethodCount})... {currentExcuteInfo.methods[currentExcuteInfo.currentMethodIndex]}";
                                    send(tcp, result);
                                }
                            }
                            break;
                        case RPCCmdId.ExcuteRemote:
                            {
                                if (_enableVerifyCode)
                                {
                                    if (string.IsNullOrEmpty(cmd.verifyCode) || cmd.verifyCode != _VerifyCode)
                                    {
                                        result.code = (int)LogType.Error;
                                        result.message = "校验码错误";
                                        send(tcp, result);
                                        continue;
                                    }
                                }

                                if (!_rpcMethodsByRemote.TryGetValue(cmd.remote, out var methods))
                                {
                                    result.code = (int)LogType.Error;
                                    result.message = "远程函数不存在";
                                    send(tcp, result);
                                    continue;
                                }

                                var list = string.IsNullOrEmpty(cmd.methodName) ? methods : methods.FindAll(t => t.method.Name == cmd.methodName);
                                if (list.Count == 0)
                                {
                                    result.code = (int)LogType.Error;
                                    result.message = "可执行远程列表为空";
                                    send(tcp, result);
                                    continue;
                                }

                                if (currentExcuteInfo.isExecuting)
                                {
                                    result.code = (int)LogType.Log;
                                    result.message = "任务进行中 请稍后";
                                    send(tcp, result);
                                    continue;
                                }

                                currentExcuteInfo.currentMethodIndex = 0;
                                ExcuteRemote(list, args, result);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    AppendLog($"断开连接: {tcp.Client.RemoteEndPoint as IPEndPoint}", LogType.Log);
                    tcp.Close();
                    tcp.Dispose();
                    clients.TryRemove(tcp, out _);
                    return;
                }
            }
        }
        async void send(TcpClient client, RPCResult result)
        {
            if (result.code != -1)
                AppendLog($"发送消息到[{client.Client.RemoteEndPoint as IPEndPoint}]  [{result.message}]", (LogType)result.code);
            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(result));
            var tmp = new byte[bytes.Length + 4];
            Array.Copy(bytes, 0, tmp, 4, bytes.Length);
            tmp[0] = (byte)(bytes.Length >> 0);
            tmp[1] = (byte)(bytes.Length >> 8);
            tmp[2] = (byte)(bytes.Length >> 16);
            tmp[3] = (byte)(bytes.Length >> 24);
            try
            {
                await client.GetStream().WriteAsync(tmp, 0, tmp.Length);
            }
            catch (Exception e)
            {
                AppendLog($"断开连接: {client.Client.RemoteEndPoint as IPEndPoint}", LogType.Log);
                client.Close();
                clients.TryRemove(client, out _);
            }
        }
        void sendToAll(RPCResult result)
        {
            if (result.code != -1)
                AppendLog($"发送消息到所有端  [{result.message}]", (LogType)result.code);
            foreach (var item in clients.Keys)
                send(item, result);
        }
        async void ExcuteRemote(List<(MethodInfo method, RPCAttribute attr)> methods, RPCArgs args, RPCResult result)
        {
            if (methods.Count > 0)
                currentExcuteInfo.remote = methods[0].attr.Remote;
            currentExcuteInfo.isExecuting = true;
            currentExcuteInfo.methods.Clear();
            currentExcuteInfo.methods.AddRange(_rpcMethodsByRemote[this.currentExcuteInfo.remote].Select(t => t.method.Name));
            currentExcuteInfo.args = args.map;
            currentExcuteInfo.totalMethodCount = methods.Count;
            result.currentExcuteInfo = currentExcuteInfo;
            result.code = -1;
            sendToAll(result);
            bool hasError = false;

            for (int i = currentExcuteInfo.currentMethodIndex; i < methods.Count; i++)
            {
                var method = methods[i];
                currentExcuteInfo.currentMethodIndex = i;

                result.code = (int)LogType.Log;
                result.message = $"正在执行({currentExcuteInfo.currentMethodIndex + 1}/{currentExcuteInfo.totalMethodCount})... {method.method.Name}";
                sendToAll(result);
                window.WaitRepaint();
                await Task.Delay(100);

                result.code = -1;
                result.currentExcuteInfo = currentExcuteInfo;
                sendToAll(result);

                object invokeResult = null;
                var ps = method.method.GetParameters();
                if (ps.Length == 0)
                {
                    await window.WaitMainThread(() =>
                    {
                        try
                        {
                            invokeResult = method.method.Invoke(null, null);
                            result.code = (int)LogType.Log;
                        }
                        catch (Exception e)
                        {
                            result.code = (int)LogType.Error;
                            result.message = $"远程函数 {method.method.Name} 执行出错 error={(e.InnerException != null ? e.InnerException : e.Message)}";
                        }
                    });
                    if (result.code == (int)LogType.Error)
                    {
                        hasError = true;
                        sendToAll(result);
                        break;
                    }
                }
                else if (ps.Length == 1 && ps[0].ParameterType == typeof(RPCArgs))
                {
                    await window.WaitMainThread(() =>
                    {
                        try
                        {
                            invokeResult = method.method.Invoke(null, new object[] { args });
                            result.code = (int)LogType.Log;
                        }
                        catch (Exception e)
                        {
                            result.code = (int)LogType.Error;
                            result.message = $"远程函数 {method.method.Name} 执行出错 error={(e.InnerException != null ? e.InnerException : e.Message)}";
                        }
                    });
                    if (result.code == (int)LogType.Error)
                    {
                        hasError = true;
                        sendToAll(result);
                        break;
                    }
                }
                else
                {
                    result.code = (int)LogType.Error;
                    result.message = $"不支持的参数类型";
                    hasError = true;
                    sendToAll(result);
                    break;
                }
                if (invokeResult != null)
                {
                    if (invokeResult is IAsyncEnumerable<string> asyncEnum)
                    {
                        var ie = asyncEnum.GetAsyncEnumerator();
                        while (true)
                        {
                            bool moveNext = false;
                            TaskCompletionSource<bool> task = new();
                            await window.WaitMainThread(async () =>
                            {
                                try
                                {
                                    if (moveNext = await ie.MoveNextAsync())
                                    {
                                        result.code = (int)LogType.Log;
                                        result.message = ie.Current;
                                        sendToAll(result);
                                        task.TrySetResult(true);
                                    }
                                    else
                                        task.TrySetResult(false);
                                }
                                catch (Exception e)
                                {
                                    result.code = (int)LogType.Error;
                                    result.message = $"远程函数 {method.method.Name} 执行出错 error={(e.InnerException != null ? e.InnerException : e.Message)}";
                                    sendToAll(result);
                                    task.TrySetResult(true);
                                }
                            });
                            await task.Task;
                            if (result.code == (int)LogType.Error)
                            {
                                hasError = true;
                                sendToAll(result);
                                break;
                            }
                            else if (!moveNext)
                                break;
                        }
                    }
                    else if (invokeResult is Task<string> taskResult)
                    {
                        try
                        {
                            result.message = await taskResult;
                            result.code = (int)LogType.Log;
                            sendToAll(result);
                        }
                        catch (Exception e)
                        {
                            result.code = (int)LogType.Error;
                            result.message = e.Message;
                            sendToAll(result);
                            hasError = true;
                            break;
                        }
                    }
                    else if (invokeResult is Task task)
                    {
                        try
                        {
                            await task;
                        }
                        catch (Exception e)
                        {
                            result.code = (int)LogType.Error;
                            result.message = e.Message;
                            sendToAll(result);
                            hasError = true;
                            break;
                        }
                    }
                    else
                    {
                        result.code = (int)LogType.Log;
                        result.message = $"远程函数 {method.method.Name} 执行结果={invokeResult}";
                        sendToAll(result);
                    }
                }
            }

            currentExcuteInfo.isExecuting = false;
            result.code = -1;
            sendToAll(result);
            if (!hasError)
            {
                result.code = (int)LogType.Log;
                result.message = $"远程函数执行完成";
                sendToAll(result);
            }
        }
        async void reload()
        {
            if (currentExcuteInfo.isExecuting && currentExcuteInfo.currentMethodIndex < currentExcuteInfo.totalMethodCount - 1)
            {
                try
                {
                    var json = JsonConvert.SerializeObject(currentExcuteInfo);
                    SessionState.SetString("RPC_currentExcuteInfo", json);
                }
                catch (Exception e)
                { Debug.LogError("保存服务状态失败 " + e); }
            }
            else
                SessionState.EraseString("RPC_currentExcuteInfo");
            sendToAll(new RPCResult { message = "程序集重载", code = (int)LogType.Log });
            await Task.Delay(1);//延迟一帧  能发送到就发送   不能就不管了
            this.StopServer();
        }
        void reStatus()
        {
            var rst = new RPCResult { cmd = (int)RPCCmdId.ExcuteRemote };
            ExcuteRemote(_rpcMethodsByRemote[currentExcuteInfo.remote], new RPCArgs(new Dictionary<string, string>(currentExcuteInfo.args)), rst);
        }

        public void Dispose()
        {
            isDisposed = true;
            AssemblyReloadEvents.beforeAssemblyReload -= reload;
            StopServer();
        }
        public void Update()
        {

        }

        public void DrawUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("本机局域网 IP:", GUILayout.Width(120));
            GUI.enabled = false;
            EditorGUILayout.TextField(localIP, GUILayout.ExpandWidth(true));
            GUI.enabled = true;
            if (GUILayout.Button("复制", GUILayout.Width(60)))
                GUIUtility.systemCopyBuffer = localIP;
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);

            EditorGUILayout.LabelField("服务端配置", EditorStyles.boldLabel);
            int newPort = EditorGUILayout.IntField("监听端口", _serverPort);
            if (newPort != _serverPort)
            {
                _serverPort = newPort;
                EditorPrefs.SetInt(window.GetCombineKey("RPC_ServerPort"), _serverPort);
            }

            EditorGUILayout.BeginHorizontal();
            var newEnable = EditorGUILayout.Toggle(_enableVerifyCode, GUILayout.Width(20));
            if (newEnable != _enableVerifyCode)
            {
                _enableVerifyCode = newEnable;
                EditorPrefs.SetBool(window.GetCombineKey("RPC_EnableVerifyCode"), _enableVerifyCode);
            }
            EditorGUILayout.LabelField("校验码:", GUILayout.Width(50));
            string newCode = EditorGUILayout.TextField(_VerifyCode);
            if (newCode != _VerifyCode)
            {
                _VerifyCode = newCode;
                EditorPrefs.SetString(window.GetCombineKey("RPC_VerifyCode"), _VerifyCode);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            string serverArrow = _serverParamsExpanded ? "▼" : "▶";
            if (GUILayout.Button(serverArrow, GUILayout.Width(25)))
                _serverParamsExpanded = !_serverParamsExpanded;
            EditorGUILayout.LabelField($"服务器全局参数({_serverParams.Count})", EditorStyles.boldLabel);
            if (_serverParamsExpanded)
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("添加参数", GUILayout.Width(80)))
                {
                    _serverParams.Add(new RPCParamItem());
                    EditorPrefs.SetString(window.GetCombineKey("RPC_ServerParams"), JsonConvert.SerializeObject(_serverParams));
                }
            }
            EditorGUILayout.EndHorizontal();

            if (_serverParamsExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(45);
                EditorGUILayout.BeginVertical();
                window.DrawParamContent(_serverParams, () =>
                {
                    EditorPrefs.SetString(window.GetCombineKey("RPC_ServerParams"), JsonConvert.SerializeObject(_serverParams));
                });
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            GUI.enabled = !_isRunning;
            if (GUILayout.Button("启动服务器", GUILayout.Height(30)))
            {
                EditorPrefs.SetBool(window.GetCombineKey("RPC_ServerShouldRun"), true);
                StartServer();
            }
            GUI.enabled = _isRunning;
            if (GUILayout.Button("停止服务器", GUILayout.Height(30)))
            {
                EditorPrefs.SetBool(window.GetCombineKey("RPC_ServerShouldRun"), false);
                StopServer();
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUILayout.LabelField($"服务器状态: {(_isRunning ? "运行中" : "已停止")}", _isRunning ? EditorStyles.boldLabel : EditorStyles.label);

            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("服务端日志", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("搜索:", GUILayout.Width(35));
            _LogSearch = EditorGUILayout.TextField(_LogSearch, GUILayout.Width(150));
            GUILayout.Space(10);
            if (GUILayout.Button("清理日志", GUILayout.Width(80)))
            {
                window._allLogs.Clear();
                window.WaitRepaint();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.ExpandHeight(true));
            window.DrawColoredLog(window._allLogs, _LogSearch);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}