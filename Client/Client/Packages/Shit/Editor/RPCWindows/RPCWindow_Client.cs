using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

partial class RPCWindow
{
    internal class RPCWindow_Client
    {
        string path = $"{Application.dataPath}/../ProjectSettings/RPCConfig.json";

        RPCWindow window;
        bool isDisposed = false;
        RPCConfig cfg = new();
        bool _isAddingTab = false;
        string _newTabName = "新页";
        int _selectedTabIndex = 0;
        int _animationIndex = 0;
        float _lastAnimationTime = 0f;
        LogFilter _logFilter = LogFilter.All;
        string _LogSearch = "";
        string _selectedLogTabId = null;

        public RPCWindow_Client(RPCWindow window)
        {
            this.window = window;

            if (File.Exists(path))
            {
                try
                {
                    string json = File.ReadAllText(path);
                    this.cfg = JsonConvert.DeserializeObject<RPCConfig>(json);
                }
                catch (Exception e)
                { Debug.LogError($"Error occurred while reading RPCConfig.json: {e.Message}"); }
            }
            for (int i = 0; i < this.cfg.tabs.Count; i++)
            {
                for (int j = 0; j < this.cfg.tabs[i].serverEntries.Count; j++)
                {
                    this.cfg.tabs[i].serverEntries[j].Init(this, this.cfg.tabs[i]);
                    _ = this.cfg.tabs[i].serverEntries[j].Connect();
                }
            }
        }
        void SaveConfigSettings()
        {
            try { File.WriteAllText(path, JsonConvert.SerializeObject(cfg, Formatting.Indented)); }
            catch (Exception e)
            { Debug.LogError($"Error occurred while saving RPCConfig.json: {e.Message}"); }
        }
        private void AppendLog(string msg, LogType type = LogType.Log, RPCTab tab = null)
        {
            if (string.IsNullOrEmpty(msg)) return;
            msg = $"[{DateTime.Now:HH:mm:ss}]{msg.Trim('\0').Trim()}";
            var entry = new LogEntry() { message = msg, type = type };
            window._allLogs.Add(entry);
            if (tab != null)
                tab.tabLog.Add(entry);
            window.WaitRepaint();
        }
        public void Dispose()
        {
            isDisposed = true;
            for (int i = 0; i < cfg.tabs.Count; i++)
            {
                for (int j = 0; j < cfg.tabs[i].serverEntries.Count; j++)
                    cfg.tabs[i].serverEntries[j].Dispose();
            }
        }

        public void Update()
        {
            if (Time.realtimeSinceStartup - _lastAnimationTime > 0.7f)
            {
                unchecked { _animationIndex++; }
                _lastAnimationTime = Time.realtimeSinceStartup;
                window.WaitRepaint();
            }
        }
        public void DrawUI()
        {
            EditorGUILayout.BeginHorizontal();
            string clientArrow = cfg._clientGlobalParamsExpanded ? "▼" : "▶";
            if (GUILayout.Button(clientArrow, GUILayout.Width(25)))
                cfg._clientGlobalParamsExpanded = !cfg._clientGlobalParamsExpanded;
            EditorGUILayout.LabelField($"客户端全局参数({cfg.clientGlobalParams.Count})", EditorStyles.boldLabel);
            if (cfg._clientGlobalParamsExpanded)
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("添加参数", GUILayout.Width(80)))
                {
                    cfg.clientGlobalParams.Add(new RPCParamItem());
                    SaveConfigSettings();
                }
            }
            EditorGUILayout.EndHorizontal();

            if (cfg._clientGlobalParamsExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(45);
                EditorGUILayout.BeginVertical();
                window.DrawParamContent(cfg.clientGlobalParams, SaveConfigSettings);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("服务器页", EditorStyles.boldLabel, GUILayout.Width(80));
            if (_isAddingTab)
            {
                _newTabName = EditorGUILayout.TextField(_newTabName);
                if (GUILayout.Button("确认", GUILayout.Width(50)))
                {
                    if (!string.IsNullOrWhiteSpace(_newTabName))
                    {
                        var newTab = new RPCTab { id = Guid.NewGuid().ToString("N"), tabName = _newTabName.Trim() };
                        cfg.tabs.Add(newTab);
                        _selectedTabIndex = cfg.tabs.Count - 1;
                        SaveConfigSettings();
                        _isAddingTab = false;
                        _newTabName = "新页";
                    }
                    else
                        EditorUtility.DisplayDialog("提示", "页名称不能为空", "确定");
                }
                if (GUILayout.Button("取消", GUILayout.Width(50)))
                {
                    _isAddingTab = false;
                    _newTabName = "新页";
                }
            }
            else
            {
                if (GUILayout.Button("＋", GUILayout.Width(30)))
                {
                    _isAddingTab = true;
                    _newTabName = $"页{cfg.tabs.Count + 1}";
                }
            }
            EditorGUILayout.EndHorizontal();

            if (cfg.tabs.Count > 0)
            {
                float spacing = 0;
                var tabButtonStyle = new GUIStyle(GUI.skin.button);
                tabButtonStyle.fontStyle = FontStyle.Bold;
                tabButtonStyle.fontSize = 13;
                tabButtonStyle.fixedHeight = 28;
                tabButtonStyle.padding = new RectOffset(8, 8, 4, 4);
                tabButtonStyle.margin = new RectOffset(0, 0, 0, 0);
                tabButtonStyle.richText = true;

                EditorGUILayout.BeginHorizontal();

                for (int i = 0; i < cfg.tabs.Count; i++)
                {
                    var tab = cfg.tabs[i];
                    bool hasExecuting = tab.serverEntries.Any(e => e.info.isExecuting);
                    string displayName = tab.tabName;

                    if (hasExecuting)
                    {
                        var progressList = tab.serverEntries
                            .Where(e => e.info.isExecuting)
                            .Select(e => e.info.currentMethodIndex.ToString() + "/" + e.info.totalMethodCount.ToString())
                            .ToList();
                        string progressStr = progressList.Count > 0 ? $"({string.Join(" ", progressList)})" : "";

                        Color[] dotColors = new Color[] { Color.white, Color.gray };
                        int offset = _animationIndex % 4;
                        string dot1 = ColorUtility.ToHtmlStringRGB(offset > 0 ? dotColors[0] : dotColors[1]);
                        string dot2 = ColorUtility.ToHtmlStringRGB(offset > 1 ? dotColors[0] : dotColors[1]);
                        string dot3 = ColorUtility.ToHtmlStringRGB(offset > 2 ? dotColors[0] : dotColors[1]);
                        string dotsHtml = $"<color=#{dot1}>.</color><color=#{dot2}>.</color><color=#{dot3}>.</color>";
                        displayName = $"{tab.tabName} {dotsHtml}{progressStr}";
                    }

                    Vector2 size = tabButtonStyle.CalcSize(new GUIContent(displayName));
                    float width = size.x + 8;
                    bool isSelected = (i == _selectedTabIndex);
                    GUI.backgroundColor = isSelected ? Color.cyan : Color.white;
                    if (GUILayout.Button(displayName, tabButtonStyle, GUILayout.Width(width)))
                    {
                        _selectedTabIndex = i;
                    }
                    GUI.backgroundColor = Color.white;
                    GUILayout.Space(spacing);
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(4);
            }

            GUILayout.Box("", GUILayout.Height(1), GUILayout.ExpandWidth(true));

            if (cfg.tabs.Count > 0)
            {
                var currentTab = cfg.tabs[_selectedTabIndex];

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("页 Remote:", GUILayout.Width(80));
                string newRemote = EditorGUILayout.TextField(currentTab.remote, GUILayout.Width(100));
                if (newRemote != currentTab.remote)
                {
                    currentTab.remote = newRemote;
                    SaveConfigSettings();
                }
                if (GUILayout.Button("删除页", GUILayout.Width(60)))
                {
                    foreach (var entry in currentTab.serverEntries)
                        EditorPrefs.DeleteKey($"{window.GetCombineKey("RPC_VerifyCode")}_{entry.id}");
                    cfg.tabs.RemoveAt(_selectedTabIndex);
                    if (_selectedTabIndex >= cfg.tabs.Count) _selectedTabIndex = cfg.tabs.Count - 1;
                    SaveConfigSettings();
                    window.WaitRepaint();
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10);

                EditorGUILayout.LabelField($"服务器列表 ({currentTab.serverEntries.Count})", EditorStyles.boldLabel);

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                for (int i = 0; i < currentTab.serverEntries.Count; i++)
                {
                    var entry = currentTab.serverEntries[i];
                    DrawServerEntry(entry, i, currentTab);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("增加服务器", GUILayout.Height(30), GUILayout.ExpandWidth(true)))
                {
                    var newEntry = new ServerEntry { };
                    newEntry.id = Guid.NewGuid().ToString("N");
                    currentTab.serverEntries.Add(newEntry);
                    newEntry.Init(this, currentTab);
                    SaveConfigSettings();
                }
                if (GUILayout.Button("Ping服务器", GUILayout.Height(30), GUILayout.ExpandWidth(true)))
                {
                    for (int i = 0; i < currentTab.serverEntries.Count; i++)
                    {
                        _ = currentTab.serverEntries[i].Connect();
                        currentTab.serverEntries[i].GetStatus();
                    }
                }
                if (GUILayout.Button("执行全部远程", GUILayout.Height(30), GUILayout.ExpandWidth(true)))
                {
                    for (int i = 0; i < currentTab.serverEntries.Count; i++)
                        currentTab.serverEntries[i].ExcuteRemote(null);
                }
                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(20);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("客户端响应日志", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            GUILayout.Box("", GUILayout.Height(1), GUILayout.ExpandWidth(true));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("日志筛选:", GUILayout.Width(60));
            if (GUILayout.Toggle(_logFilter == LogFilter.All, "全部", GUILayout.Width(50)))
                _logFilter = LogFilter.All;
            if (GUILayout.Toggle(_logFilter == LogFilter.Normal, "普通", GUILayout.Width(50)))
                _logFilter = LogFilter.Normal;
            if (GUILayout.Toggle(_logFilter == LogFilter.Error, "错误", GUILayout.Width(50)))
                _logFilter = LogFilter.Error;

            GUILayout.Label("搜索:", GUILayout.Width(35));
            _LogSearch = EditorGUILayout.TextField(_LogSearch, GUILayout.Width(150));

            GUILayout.Space(10);
            if (GUILayout.Button("清理日志", GUILayout.Width(80)))
            {
                window._allLogs.Clear();
                foreach (var tab in cfg.tabs)
                    tab.tabLog.Clear();
                window.WaitRepaint();
            }
            EditorGUILayout.EndHorizontal();

            if (cfg.tabs.Count > 0)
            {
                float spacing = 0;
                var tabButtonStyle = new GUIStyle(GUI.skin.button);
                tabButtonStyle.fontStyle = FontStyle.Bold;
                tabButtonStyle.fontSize = 13;
                tabButtonStyle.fixedHeight = 28;
                tabButtonStyle.padding = new RectOffset(8, 8, 4, 4);
                tabButtonStyle.margin = new RectOffset(0, 0, 0, 0);

                float availableWidth = EditorGUIUtility.currentViewWidth - 40;
                float currentX = 0;

                EditorGUILayout.BeginHorizontal();

                string allLogsName = "全部日志";
                Vector2 allSize = tabButtonStyle.CalcSize(new GUIContent(allLogsName));
                float allWidth = allSize.x + 8;
                float totalWidth = allWidth + spacing;

                if (currentX > 0 && currentX + totalWidth > availableWidth)
                {
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    currentX = 0;
                }

                GUI.backgroundColor = (_selectedLogTabId == null) ? Color.cyan : Color.white;
                if (GUILayout.Button(allLogsName, tabButtonStyle, GUILayout.Width(allWidth)))
                {
                    _selectedLogTabId = null;
                }
                GUI.backgroundColor = Color.white;
                currentX += totalWidth;
                GUILayout.Space(spacing);

                for (int i = 0; i < cfg.tabs.Count; i++)
                {
                    var tab = cfg.tabs[i];
                    string displayName = tab.tabName + "日志";
                    Vector2 size = tabButtonStyle.CalcSize(new GUIContent(displayName));
                    float width = size.x + 8;
                    float totalWidth2 = width + spacing;

                    if (currentX > 0 && currentX + totalWidth2 > availableWidth)
                    {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        currentX = 0;
                    }

                    GUI.backgroundColor = (_selectedLogTabId == tab.id) ? Color.cyan : Color.white;
                    if (GUILayout.Button(displayName, tabButtonStyle, GUILayout.Width(width)))
                    {
                        _selectedLogTabId = tab.id;
                    }
                    GUI.backgroundColor = Color.white;
                    currentX += totalWidth2;
                    GUILayout.Space(spacing);
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(4);
            }

            List<LogEntry> sourceLogs;
            if (_selectedLogTabId == null)
                sourceLogs = window._allLogs;
            else
            {
                var tab = cfg.tabs.FirstOrDefault(t => t.id == _selectedLogTabId);
                sourceLogs = tab?.tabLog ?? new List<LogEntry>();
            }

            IEnumerable<LogEntry> filteredLogs = sourceLogs;
            switch (_logFilter)
            {
                case LogFilter.Normal:
                    filteredLogs = sourceLogs.Where(e => e.type == LogType.Log);
                    break;
                case LogFilter.Error:
                    filteredLogs = sourceLogs.Where(e => e.type == LogType.Error);
                    break;
                case LogFilter.All:
                default:
                    break;
            }
            List<LogEntry> logToShow = filteredLogs.ToList();

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.ExpandWidth(true));
            window.DrawColoredLog(logToShow, _LogSearch);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        void DrawServerEntry(ServerEntry entry, int index, RPCTab parentTab)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            Color oldColor = GUI.color;
            GUI.color = (entry.tcp != null && entry.tcp.Connected) ? Color.green : Color.red;
            GUILayout.Label("●", GUILayout.Width(15));
            GUI.color = oldColor;

            EditorGUILayout.LabelField("名称:", GUILayout.Width(35));
            string newName = EditorGUILayout.TextField(entry.name, GUILayout.MinWidth(60));
            if (newName != entry.name)
            {
                entry.name = newName;
                SaveConfigSettings();
            }

            EditorGUILayout.LabelField("校验码:", GUILayout.Width(40));
            string newCode = EditorGUILayout.TextField(entry.verifyCode, GUILayout.MinWidth(80));
            if (newCode != entry.verifyCode)
            {
                entry.verifyCode = newCode;
                EditorPrefs.SetString($"{window.GetCombineKey("RPC_VerifyCode")}_{entry.id}", entry.verifyCode ?? "");
            }

            EditorGUILayout.LabelField("IP:", GUILayout.Width(25));
            var newip = EditorGUILayout.TextField(entry.ip, GUILayout.Width(100));
            if (entry.ip != newip)
            {
                entry.ip = newip;
                SaveConfigSettings();
            }
            EditorGUILayout.LabelField("Port:", GUILayout.Width(30));
            var newport = EditorGUILayout.IntField(entry.port, GUILayout.Width(60));
            if (entry.port != newport)
            {
                entry.port = newport;
                SaveConfigSettings();
            }

            GUI.enabled = index > 0;
            if (GUILayout.Button("↑", GUILayout.Width(25)))
            {
                var tmp = parentTab.serverEntries[index];
                parentTab.serverEntries[index] = parentTab.serverEntries[index - 1];
                parentTab.serverEntries[index - 1] = tmp;
                SaveConfigSettings();
            }
            GUI.enabled = true;
            GUI.enabled = index < parentTab.serverEntries.Count - 1;
            if (GUILayout.Button("↓", GUILayout.Width(25)))
            {
                var tmp = parentTab.serverEntries[index];
                parentTab.serverEntries[index] = parentTab.serverEntries[index + 1];
                parentTab.serverEntries[index + 1] = tmp;
                SaveConfigSettings();
            }
            GUI.enabled = true;
            if (GUILayout.Button("✕", GUILayout.Width(25)))
            {
                if (entry.info.isExecuting)
                    EditorUtility.DisplayDialog("提示", "该服务器正在执行任务，请等待完成", "确定");
                else
                {
                    EditorPrefs.DeleteKey($"{window.GetCombineKey("RPC_VerifyCode")}_{entry.id}");
                    parentTab.serverEntries.RemoveAt(index);
                    SaveConfigSettings();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            var envVarArrow = entry.evnVarExpanded ? "▼" : "▶";
            if (GUILayout.Button(envVarArrow, GUILayout.Width(25)))
                entry.evnVarExpanded = !entry.evnVarExpanded;
            string envVarCount = entry.remoteEnvVar?.Count > 0 ? $" ({entry.remoteEnvVar.Count})" : "";
            EditorGUILayout.LabelField($"环境变量{envVarCount}", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            if (entry.evnVarExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(45 + 20);
                if (entry.remoteEnvVar != null)
                {
                    GUI.enabled = false;
                    foreach (var kv in entry.remoteEnvVar)
                    {
                        EditorGUILayout.TextField(kv.Key, GUILayout.Width(100));
                        EditorGUILayout.TextField(kv.Value, GUILayout.ExpandWidth(true));
                    }
                    GUI.enabled = true;
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            string methodsArrow = entry.methodsExpanded ? "▼" : "▶";
            if (GUILayout.Button(methodsArrow, GUILayout.Width(25)))
                entry.methodsExpanded = !entry.methodsExpanded;
            string methodCount = entry.remoteMethods?.Count > 0 ? $" ({entry.remoteMethods.Count})" : "";
            EditorGUILayout.LabelField($"远程函数{methodCount}", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            if (entry.methodsExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(45);
                EditorGUILayout.BeginVertical();
                if (entry.remoteMethods != null && entry.remoteMethods.Count > 0)
                {
                    foreach (var methodName in entry.remoteMethods)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(methodName, GUILayout.ExpandWidth(true));
                        if (GUILayout.Button("执行", GUILayout.Width(50)))
                            entry.ExcuteRemote(methodName);
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("  (无数据)", EditorStyles.miniLabel);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            if (20 > 0) GUILayout.Space(20);
            string arrow = entry.paramExpanded ? "▼" : "▶";
            if (GUILayout.Button(arrow, GUILayout.Width(25)))
                entry.paramExpanded = !entry.paramExpanded;
            EditorGUILayout.LabelField($"{"重载参数"} ({entry.paramItems.Count})", GUILayout.Width(150));
            if (entry.paramExpanded)
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("添加参数", GUILayout.Width(80)))
                {
                    entry.paramItems.Add(new RPCParamItem());
                    SaveConfigSettings();
                }
            }
            EditorGUILayout.EndHorizontal();

            if (entry.paramExpanded)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(45 + 20);
                EditorGUILayout.BeginVertical();
                window.DrawParamContent(entry.paramItems, SaveConfigSettings);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

            GUI.enabled = !entry.info.isExecuting;
            if (GUILayout.Button(entry.info.isExecuting ? "执行中..." : "执行 Remote", GUILayout.Height(30)))
            {
                if (string.IsNullOrEmpty(parentTab.remote))
                {
                    EditorUtility.DisplayDialog("提示", "页 Remote 名称为空", "确定");
                }
                else
                {
                    if (!entry.info.isExecuting)
                        entry.ExcuteRemote(null);
                }
            }
            GUI.enabled = true;

            if (entry.info.isExecuting)
            {
                Rect progressRect = EditorGUILayout.GetControlRect(false, 18);
                string progressText = $"{entry.info.currentMethodIndex}/{entry.info.totalMethodCount}";
                EditorGUI.ProgressBar(progressRect, (float)entry.info.currentMethodIndex / Math.Max(1, entry.info.totalMethodCount), progressText);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
        }


        class RPCConfig
        {
            public List<RPCParamItem> clientGlobalParams = new();
            public List<RPCTab> tabs = new();

            [NonSerialized] public bool _clientGlobalParamsExpanded = false;
        }
        [Serializable]
        private class RPCTab
        {
            public string id;
            public string tabName = "页签";
            public string remote = "remote";
            public List<ServerEntry> serverEntries = new List<ServerEntry>();

            [NonSerialized] public List<LogEntry> tabLog = new List<LogEntry>();
        }

        [Serializable]
        class ServerEntry
        {
            public string id = "";
            public string name = "服务器";
            public string ip = "127.0.0.1";
            public int port = 8888;
            public List<RPCParamItem> paramItems = new();

            [NonSerialized] public RPCWindow_Client client;
            [NonSerialized] public RPCTab tab;
            [NonSerialized] public bool isDisposed = false;
            [NonSerialized] public RemoteExcuteInfo info = new();
            [NonSerialized] public string verifyCode = "";
            [NonSerialized] public bool methodsExpanded = false;
            [NonSerialized] public List<string> remoteMethods = new List<string>();
            [NonSerialized] public bool evnVarExpanded = false;
            [NonSerialized] public Dictionary<string, string> remoteEnvVar = new();
            [NonSerialized] public TcpClient tcp;
            [NonSerialized] public bool paramExpanded = false;
            [NonSerialized] Dictionary<string, TaskCompletionSource<RPCResult>> waiter = new();

            public void Init(RPCWindow_Client client, RPCTab tab)
            {
                this.client = client;
                this.tab = tab;
                verifyCode = EditorPrefs.GetString($"{client.window.GetCombineKey("RPC_VerifyCode")}_{id}");
            }
            public async Task Connect()
            {
                if (tcp != null && tcp.Connected && (tcp.Client.RemoteEndPoint as IPEndPoint).Equals(new IPEndPoint(IPAddress.Parse(ip), port)))
                    return;
                tcp?.Dispose();
                tcp = new();
                try
                { await tcp.ConnectAsync(ip, port); }
                catch (Exception)
                { return; }
                if (isDisposed)
                {
                    tcp.Dispose();
                    return;
                }
                ReceiveLoop(tcp);
                this.GetStatus();
            }
            async void ReceiveLoop(TcpClient tcp)
            {
                byte[] buffer = new byte[2048];
                while (tcp.Connected)
                {
                    try
                    {
                        int len = await tcp.GetStream().ReadAsync(buffer, 0, 4).ConfigureAwait(false);
                        if (len <= 0)
                        {
                            client.AppendLog($"[{name}] 服务器断开连接", LogType.Error, tab);
                            tcp.Close();
                            tcp.Dispose();

                            if (info.isExecuting)
                            {
                                while (!this.isDisposed)
                                {
                                    await Connect();
                                    //一直等待重连
                                    if (tcp != null && tcp.Connected)
                                        return;
                                    await Task.Delay(100);
                                }
                            }
                            else
                                info = new();
                            return;
                        }
                        int byteCnt = buffer[0];
                        byteCnt |= buffer[1] << 8;
                        byteCnt |= buffer[2] << 16;
                        byteCnt |= buffer[3] << 32;
                        await tcp.GetStream().ReadAsync(buffer, 0, byteCnt);
                        var json = Encoding.UTF8.GetString(buffer, 0, byteCnt);
                        try
                        {
                            var result = JsonConvert.DeserializeObject<RPCResult>(json);
                            if (result.cmd == (int)RPCCmdId.ExcuteRemote && result.code == -1)
                                info = result.currentExcuteInfo;
                            if (!string.IsNullOrEmpty(result.requestId) && waiter.TryGetValue(result.requestId, out var tcs))
                            {
                                waiter.Remove(result.requestId);
                                tcs.SetResult(result);
                            }
                            if (result.code != -1)
                                client.AppendLog(result.message, (LogType)result.code, tab);
                        }
                        catch (Exception e)
                        {
                            client.AppendLog($"[{name}] 解析数据异常: {json}", LogType.Error, tab);
                        }
                    }
                    catch (Exception e)
                    {
                        client.AppendLog($"[{name}] 服务器断开", LogType.Log, tab);
                        break;
                    }
                }
            }
            public void Dispose()
            {
                isDisposed = true;
                tcp?.Close();
                tcp?.Dispose();
            }
            public async void GetStatus()
            {
                if (isDisposed || this.tcp == null || !this.tcp.Connected)
                    return;
                RPCCommand cmd = new RPCCommand() { cmd = (int)RPCCmdId.GetStatus, remote = tab.remote};
                for (int i = 0; i < client.cfg.clientGlobalParams.Count; i++)
                {
                    if (client.cfg.clientGlobalParams[i].enabled)
                        cmd.paramItems[client.cfg.clientGlobalParams[i].key] = client.cfg.clientGlobalParams[i].value;
                }
                for (int i = 0; i < paramItems.Count; i++)
                {
                    if (paramItems[i].enabled)
                        cmd.paramItems[paramItems[i].key] = paramItems[i].value;
                }
                var ret = await Send(cmd);
                this.info = ret.currentExcuteInfo;
                this.remoteMethods.Clear();
                this.remoteMethods.AddRange(ret.methods);
                this.remoteEnvVar = ret.envVar;
                client.window.WaitRepaint();
            }
            public async void ExcuteRemote(string method)
            {
                if (isDisposed)
                    return;
                RPCCommand cmd = new RPCCommand() { cmd = (int)RPCCmdId.ExcuteRemote, remote = tab.remote, methodName = method, verifyCode = this.verifyCode };
                for (int i = 0; i < client.cfg.clientGlobalParams.Count; i++)
                {
                    if (client.cfg.clientGlobalParams[i].enabled)
                        cmd.paramItems[client.cfg.clientGlobalParams[i].key] = client.cfg.clientGlobalParams[i].value;
                }
                for (int i = 0; i < paramItems.Count; i++)
                {
                    if (paramItems[i].enabled)
                        cmd.paramItems[paramItems[i].key] = paramItems[i].value;
                }
                await Send(cmd);
            }

            async Task<RPCResult> Send(RPCCommand cmd)
            {
                await Connect();
                if (!tcp.Connected)
                {
                    client.AppendLog($"[{name}] 设备不在线 ", LogType.Error, tab);
                    return new();
                }
                else
                {
                    cmd.requestId = Guid.NewGuid().ToString("N");
                    var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(cmd) + "\n");
                    var tmp = new byte[bytes.Length + 4];
                    Array.Copy(bytes, 0, tmp, 4, bytes.Length);
                    tmp[0] = (byte)(bytes.Length >> 0);
                    tmp[1] = (byte)(bytes.Length >> 8);
                    tmp[2] = (byte)(bytes.Length >> 16);
                    tmp[3] = (byte)(bytes.Length >> 24);

                    if (!waiter.TryGetValue(cmd.requestId, out var ret) || ret.Task.IsCompleted)
                        ret = waiter[cmd.requestId] = new();

                    await tcp.GetStream().WriteAsync(tmp, 0, tmp.Length);

                    return await ret.Task;
                }
            }
        }
    }
}