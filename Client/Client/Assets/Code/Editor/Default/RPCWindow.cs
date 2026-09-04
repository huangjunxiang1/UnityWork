using System;
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
using UnityEngine;

// ============================================================
// RPCAttribute - 标记 RPC 方法，按 Order 排序执行，Remote 分组
// ============================================================
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RPCAttribute : Attribute
{
    public int Order { get; set; }
    public string Remote { get; set; }
    public RPCAttribute(string remote, int order)
    {
        Remote = remote;
        Order = order;
    }
}

// ============================================================
// RPCParamItem - 参数键值对（公共类型）
// ============================================================
[Serializable]
public class RPCParamItem
{
    public bool enabled = true;
    public string key = "";
    public string value = "";
}

// ============================================================
// RPCArgs - 参数解析器（支持从 RPCParamItem 列表构建）
// ============================================================
public class RPCArgs
{
    private Dictionary<string, string> _dict = new Dictionary<string, string>();

    public RPCArgs(IEnumerable<RPCParamItem> items)
    {
        if (items == null) return;
        foreach (var item in items)
        {
            if (item != null && !string.IsNullOrEmpty(item.key))
            {
                _dict[item.key] = item.value ?? "";
            }
        }
    }

    public string this[string key] => _dict.TryGetValue(key, out string v) ? v : null;
    public string GetValueOrDefault(string key, string def = "") => _dict.TryGetValue(key, out string v) ? v : def;
    public IEnumerable<string> Keys => _dict.Keys;
}

// ============================================================
// RPC 窗口主类
// ============================================================
public class RPCWindow : EditorWindow
{
    // ---------- 私有嵌套类型 ----------
    [Serializable]
    private class RPCMessage
    {
        public string type;
        public int code;
        public string message;
        public float progress = -1f;
    }

    [Serializable]
    private class RPCCommand
    {
        public string cmd;
        public string remote;
        public List<RPCParamItem> paramItems;
        public string methodName;
        public string verifyCode;
    }

    private class LogEntry
    {
        public string message;
        public bool isError;
        public LogEntry(string msg, bool error = false)
        {
            message = msg;
            isError = error;
        }
    }

    // ---------- 窗口字段 ----------
    private enum Mode { Server, Client }

    [SerializeField] private Mode _currentMode = Mode.Client; // 默认 Client 模式
    [SerializeField] private int _serverPort = 8888;
    [SerializeField] private string _serverCode = "";

    private const string SERVER_CODE_KEY = "RPC_ServerCode";
    private const string SERVER_PORT_KEY = "RPC_ServerPort"; // 新增：保存端口
    private const string REMOTE_CONFIG_PATH = "ProjectSettings/RPCConfig.json";
    private const string SERVER_PARAMS_KEY = "RPC_ServerParams";
    private const string VERIFY_CODE_PREFIX = "RPC_VerifyCode_";

    private TcpListener _listener;
    private bool _isRunning = false;
    private List<LogEntry> _serverLog = new List<LogEntry>();
    private Vector2 _serverScrollPos;
    private readonly object _serverLogLock = new object();

    [SerializeField] private List<ServerEntry> _serverEntries = new List<ServerEntry>();
    [SerializeField] private List<RPCParamItem> _clientGlobalParams = new List<RPCParamItem>();
    [SerializeField] private List<RPCParamItem> _serverParams = new List<RPCParamItem>();

    private List<LogEntry> _clientLog = new List<LogEntry>();
    private Vector2 _clientScrollPos;
    private Vector2 _clientMainScrollPos;
    private readonly object _clientLogLock = new object();
    private bool _clientGlobalParamsExpanded = false;
    private bool _serverParamsExpanded = false;

    // 主线程队列
    private readonly Queue<Action> _mainThreadActions = new Queue<Action>();
    private readonly object _mainThreadLock = new object();

    [Serializable]
    private class ServerEntry
    {
        public string name = "";
        public string remote = "";
        public string ip;
        public int port;
        public List<RPCParamItem> paramItems = new List<RPCParamItem>();
        public string id = "";

        [NonSerialized] public string verifyCode = "";
        [NonSerialized] public bool isExecuting = false;
        [NonSerialized] public CancellationTokenSource cts = null;
        [NonSerialized] public bool paramExpanded = false;
        [NonSerialized] public List<string> remoteMethods = new List<string>();
        [NonSerialized] public bool methodsExpanded = false;
        [NonSerialized] public float progress = 0f;
        // 在线状态标记
        [NonSerialized] public bool isOnline = false;
        [NonSerialized] public bool hasOnlineState = false;
    }

    // ---------- RPC 方法注册 ----------
    private static Dictionary<string, List<MethodInfo>> _rpcMethodsByRemote = null;

    // ---------- 续传状态 ----------
    private static string _resumeRemote = null;
    private static int _resumeMethodIndex = 0;
    private static string _resumeParam = "";
    private static bool _isCompiling = false;

    // ============================================================
    // 菜单入口
    // ============================================================
    [MenuItem("Tools/RPC Window")]
    public static void ShowWindow() => GetWindow<RPCWindow>("RPC 窗口");

    // ============================================================
    // 生命周期
    // ============================================================
    private void OnEnable()
    {
        LoadServerCode();
        LoadServerPort(); // 加载端口（如果不存在则随机生成）
        LoadRemoteSettings();
        LoadServerParams();
        RegisterRPCClasses();

        EditorApplication.update += ProcessMainThreadQueue;

        AssemblyReloadEvents.beforeAssemblyReload -= OnBeforeAssemblyReload;
        AssemblyReloadEvents.beforeAssemblyReload += OnBeforeAssemblyReload;
        AssemblyReloadEvents.afterAssemblyReload -= OnAfterAssemblyReload;
        AssemblyReloadEvents.afterAssemblyReload += OnAfterAssemblyReload;

        if (EditorPrefs.GetBool("RPCServer_ShouldRun", false))
            StartServer();
    }

    private void OnDisable()
    {
        EditorApplication.update -= ProcessMainThreadQueue;

        SaveRemoteSettings();
        SaveServerParams();
        StopServer(false);
        foreach (var entry in _serverEntries)
            entry.cts?.Cancel();
    }

    private void OnBeforeAssemblyReload()
    {
        if (!string.IsNullOrEmpty(_resumeRemote))
        {
            _isCompiling = true;
            EditorApplication.delayCall += () =>
            {
                EditorPrefs.SetString("RPC_ResumeRemote", _resumeRemote);
                EditorPrefs.SetInt("RPC_ResumeIndex", _resumeMethodIndex);
                EditorPrefs.SetString("RPC_ResumeParam", _resumeParam);
            };
            Debug.Log($"[RPC] 检测到编译，保存续传状态: {_resumeRemote} 索引 {_resumeMethodIndex}");
        }
        SaveServerParams();
        StopServer(false);
    }

    private void OnAfterAssemblyReload()
    {
        RegisterRPCClasses();
        if (EditorPrefs.GetBool("RPCServer_ShouldRun", false))
        {
            Debug.Log("[RPC] 程序集重载完成，尝试重新启动服务...");
            EditorApplication.delayCall += () =>
            {
                if (this != null && !_isRunning)
                    StartServer();
            };
        }
    }

    // ============================================================
    // 主线程队列处理
    // ============================================================
    private void EnqueueOnMainThread(Action action)
    {
        lock (_mainThreadLock)
        {
            _mainThreadActions.Enqueue(action);
        }
    }

    private void ProcessMainThreadQueue()
    {
        lock (_mainThreadLock)
        {
            while (_mainThreadActions.Count > 0)
            {
                var action = _mainThreadActions.Dequeue();
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"[RPC] 主线程任务执行异常: {e}");
                }
            }
        }
    }

    // ============================================================
    // 注册所有带 RPCAttribute 的方法
    // ============================================================
    private static void RegisterRPCClasses()
    {
        _rpcMethodsByRemote = new Dictionary<string, List<MethodInfo>>();
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (var type in asm.GetTypes())
            {
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                    .Where(m => m.GetCustomAttribute<RPCAttribute>() != null)
                    .ToList();
                foreach (var method in methods)
                {
                    var attr = method.GetCustomAttribute<RPCAttribute>();
                    if (string.IsNullOrEmpty(attr.Remote))
                    {
                        Debug.LogWarning($"方法 {method.DeclaringType?.FullName}.{method.Name} 未指定 Remote，将被忽略");
                        continue;
                    }
                    if (!_rpcMethodsByRemote.ContainsKey(attr.Remote))
                        _rpcMethodsByRemote[attr.Remote] = new List<MethodInfo>();
                    _rpcMethodsByRemote[attr.Remote].Add(method);
                }
            }
        }
        foreach (var kv in _rpcMethodsByRemote)
        {
            kv.Value.Sort((a, b) => a.GetCustomAttribute<RPCAttribute>().Order.CompareTo(b.GetCustomAttribute<RPCAttribute>().Order));
        }
        Debug.Log($"[RPC] 已注册 {_rpcMethodsByRemote.Count} 个 Remote 分组");
    }

    // ============================================================
    // 服务端校验码与端口管理
    // ============================================================
    private void LoadServerCode()
    {
        string code = EditorPrefs.GetString(SERVER_CODE_KEY, "");
        if (string.IsNullOrEmpty(code))
        {
            code = Guid.NewGuid().ToString("N");
            EditorPrefs.SetString(SERVER_CODE_KEY, code);
            Debug.Log($"[RPC] 生成新校验码: {code}");
        }
        _serverCode = code;
    }

    private void SaveServerCode()
    {
        EditorPrefs.SetString(SERVER_CODE_KEY, _serverCode);
    }

    private void LoadServerPort()
    {
        if (EditorPrefs.HasKey(SERVER_PORT_KEY))
        {
            _serverPort = EditorPrefs.GetInt(SERVER_PORT_KEY);
        }
        else
        {
            _serverPort = UnityEngine.Random.Range(1024, 65535);
            EditorPrefs.SetInt(SERVER_PORT_KEY, _serverPort);
            Debug.Log($"[RPC] 首次打开，随机生成服务器端口: {_serverPort}");
        }
    }

    private void SaveServerPort()
    {
        EditorPrefs.SetInt(SERVER_PORT_KEY, _serverPort);
    }

    // ============================================================
    // 服务端参数管理（EditorPrefs）
    // ============================================================
    private void LoadServerParams()
    {
        string json = EditorPrefs.GetString(SERVER_PARAMS_KEY, "");
        if (!string.IsNullOrEmpty(json))
        {
            try
            {
                var wrapper = JsonUtility.FromJson<SerializableList<RPCParamItem>>(json);
                _serverParams = wrapper?.list ?? new List<RPCParamItem>();
            }
            catch
            {
                _serverParams = new List<RPCParamItem>();
            }
        }
        else
        {
            _serverParams = new List<RPCParamItem>();
        }
    }

    private void SaveServerParams()
    {
        var wrapper = new SerializableList<RPCParamItem> { list = _serverParams };
        string json = JsonUtility.ToJson(wrapper);
        EditorPrefs.SetString(SERVER_PARAMS_KEY, json);
    }

    // ============================================================
    // 客户端校验码管理（基于唯一ID）
    // ============================================================
    private string GenerateNewId() => Guid.NewGuid().ToString("N");

    private string GetVerifyCodeKey(string id) => VERIFY_CODE_PREFIX + id;

    private void LoadVerifyCode(ServerEntry entry)
    {
        if (string.IsNullOrEmpty(entry.id))
        {
            entry.id = GenerateNewId();
            SaveRemoteSettings();
        }
        entry.verifyCode = EditorPrefs.GetString(GetVerifyCodeKey(entry.id), "");
    }

    private void SaveVerifyCode(ServerEntry entry)
    {
        if (string.IsNullOrEmpty(entry.id))
        {
            entry.id = GenerateNewId();
            SaveRemoteSettings();
        }
        EditorPrefs.SetString(GetVerifyCodeKey(entry.id), entry.verifyCode ?? "");
    }

    private void DeleteVerifyCode(ServerEntry entry)
    {
        if (!string.IsNullOrEmpty(entry.id))
            EditorPrefs.DeleteKey(GetVerifyCodeKey(entry.id));
    }

    // ============================================================
    // 远程配置存储
    // ============================================================
    [Serializable]
    private class RPCConfig
    {
        public List<ServerEntry> serverEntries = new List<ServerEntry>();
        public List<RPCParamItem> clientGlobalParams = new List<RPCParamItem>();
    }

    private string GetConfigPath()
    {
        string dataPath = Application.dataPath;
        string projectPath = Directory.GetParent(dataPath)?.FullName;
        if (string.IsNullOrEmpty(projectPath))
            projectPath = dataPath + "/..";
        return Path.Combine(projectPath, REMOTE_CONFIG_PATH);
    }

    private void LoadRemoteSettings()
    {
        string configPath = GetConfigPath();
        if (File.Exists(configPath))
        {
            try
            {
                string json = File.ReadAllText(configPath);
                var config = JsonUtility.FromJson<RPCConfig>(json);
                if (config != null)
                {
                    _serverEntries = config.serverEntries ?? new List<ServerEntry>();
                    _clientGlobalParams = config.clientGlobalParams ?? new List<RPCParamItem>();
                    foreach (var entry in _serverEntries)
                    {
                        if (string.IsNullOrEmpty(entry.name))
                            entry.name = entry.remote;
                        LoadVerifyCode(entry);
                    }
                    Debug.Log($"[RPC] 从文件加载配置: {configPath}");
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[RPC] 读取配置文件失败: {e.Message}");
            }
        }

        string oldJson = EditorPrefs.GetString("RPC_ServerEntries", "");
        if (!string.IsNullOrEmpty(oldJson))
        {
            try
            {
                var wrapper = JsonUtility.FromJson<SerializableList<ServerEntry>>(oldJson);
                if (wrapper?.list != null)
                {
                    _serverEntries = wrapper.list;
                    foreach (var entry in _serverEntries)
                    {
                        if (string.IsNullOrEmpty(entry.name))
                            entry.name = entry.remote;
                        LoadVerifyCode(entry);
                    }
                    EditorPrefs.DeleteKey("RPC_ServerEntries");
                    Debug.Log("[RPC] 从 EditorPrefs 迁移服务器列表成功");
                    SaveRemoteSettings();
                    return;
                }
            }
            catch { }
        }

        if (_serverEntries.Count == 0)
        {
            var defaultEntry = new ServerEntry { ip = "127.0.0.1", port = 8888, remote = "DefaultRemote", name = "本地服务器" };
            defaultEntry.id = GenerateNewId();
            _serverEntries.Add(defaultEntry);
        }
        if (_clientGlobalParams == null) _clientGlobalParams = new List<RPCParamItem>();
        SaveRemoteSettings();
    }

    private void SaveRemoteSettings()
    {
        string configPath = GetConfigPath();
        try
        {
            string dir = Path.GetDirectoryName(configPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var config = new RPCConfig
            {
                serverEntries = _serverEntries,
                clientGlobalParams = _clientGlobalParams
            };
            string json = JsonUtility.ToJson(config, true);
            File.WriteAllText(configPath, json);
        }
        catch (Exception e)
        {
            Debug.LogError($"[RPC] 保存远程配置失败: {e.Message}");
        }
    }

    [Serializable]
    private class SerializableList<T> { public List<T> list; }

    private static string GetLocalIPAddress()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork) return ip.ToString();
        }
        catch { }
        return "127.0.0.1";
    }

    // ============================================================
    // GUI
    // ============================================================
    private void OnGUI()
    {
        try
        {
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            _currentMode = (Mode)EditorGUILayout.EnumPopup("模式", _currentMode);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (_currentMode == Mode.Server) DrawServerUI();
            else DrawClientUI();

            if (GUI.changed)
            {
                SaveRemoteSettings();
                SaveServerParams();
                SaveServerPort(); // 端口变化时保存
            }
        }
        catch (Exception e)
        {
            Debug.LogError("RPCWindow OnGUI 异常: " + e.ToString());
        }
    }

    // ---------- 参数列表绘制辅助 ----------
    private void DrawParamList(ref bool expanded, List<RPCParamItem> paramList, string title)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(20);
        string arrow = expanded ? "▼" : "▶";
        if (GUILayout.Button(arrow, GUILayout.Width(25)))
            expanded = !expanded;
        EditorGUILayout.LabelField($"{title} ({paramList.Count})", GUILayout.Width(150));
        if (expanded)
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("添加参数", GUILayout.Width(80)))
            {
                paramList.Add(new RPCParamItem());
                SaveRemoteSettings();
                SaveServerParams();
            }
        }
        EditorGUILayout.EndHorizontal();

        if (expanded)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(45);
            EditorGUILayout.BeginVertical();
            for (int p = 0; p < paramList.Count; p++)
            {
                var param = paramList[p];
                EditorGUILayout.BeginHorizontal();
                param.enabled = EditorGUILayout.Toggle(param.enabled, GUILayout.Width(20));
                param.key = EditorGUILayout.TextField(param.key, GUILayout.Width(100));
                param.value = EditorGUILayout.TextField(param.value, GUILayout.ExpandWidth(true));

                GUI.enabled = p > 0;
                if (GUILayout.Button("↑", GUILayout.Width(25)))
                {
                    var temp = paramList[p];
                    paramList[p] = paramList[p - 1];
                    paramList[p - 1] = temp;
                }
                GUI.enabled = true;
                GUI.enabled = p < paramList.Count - 1;
                if (GUILayout.Button("↓", GUILayout.Width(25)))
                {
                    var temp = paramList[p];
                    paramList[p] = paramList[p + 1];
                    paramList[p + 1] = temp;
                }
                GUI.enabled = true;
                if (GUILayout.Button("✕", GUILayout.Width(25)))
                {
                    paramList.RemoveAt(p);
                    p--;
                    SaveRemoteSettings();
                    SaveServerParams();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawServerUI()
    {
        string localIP = GetLocalIPAddress();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("本机局域网 IP:", GUILayout.Width(120));
        GUI.enabled = false;
        EditorGUILayout.TextField(localIP, GUILayout.ExpandWidth(true));
        GUI.enabled = true;
        if (GUILayout.Button("复制", GUILayout.Width(60)))
        {
            GUIUtility.systemCopyBuffer = localIP;
            Debug.Log("已复制 IP: " + localIP);
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);

        EditorGUILayout.LabelField("服务端配置", EditorStyles.boldLabel);
        int newPort = EditorGUILayout.IntField("监听端口", _serverPort);
        if (newPort != _serverPort)
        {
            _serverPort = newPort;
            if (_isRunning)
            {
                StopServer(false);
                StartServer();
            }
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("校验码:", GUILayout.Width(80));
        string newCode = EditorGUILayout.TextField(_serverCode);
        if (newCode != _serverCode)
        {
            _serverCode = newCode;
            SaveServerCode();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        DrawParamList(ref _serverParamsExpanded, _serverParams, "服务器参数列表");

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = !_isRunning;
        if (GUILayout.Button("启动服务器", GUILayout.Height(30)))
            StartServer();
        GUI.enabled = _isRunning;
        if (GUILayout.Button("停止服务器", GUILayout.Height(30)))
            StopServer(true);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        EditorGUILayout.LabelField($"服务器状态: {(_isRunning ? "运行中" : "已停止")}", _isRunning ? EditorStyles.boldLabel : EditorStyles.label);

        GUILayout.Space(10);
        EditorGUILayout.LabelField("服务端日志", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("清理日志", GUILayout.Width(80)))
        {
            lock (_serverLogLock) { _serverLog.Clear(); }
            _serverScrollPos = Vector2.zero;
            Repaint();
        }
        EditorGUILayout.EndHorizontal();

        _serverScrollPos = EditorGUILayout.BeginScrollView(_serverScrollPos, GUILayout.ExpandHeight(true));
        DrawColoredLog(_serverLog);
        EditorGUILayout.EndScrollView();
    }

    private void DrawClientUI()
    {
        _clientMainScrollPos = EditorGUILayout.BeginScrollView(_clientMainScrollPos, GUILayout.ExpandHeight(true));

        EditorGUILayout.LabelField("客户端全局参数", EditorStyles.boldLabel);
        DrawParamList(ref _clientGlobalParamsExpanded, _clientGlobalParams, "全局参数");

        GUILayout.Space(10);

        EditorGUILayout.LabelField("服务器列表", EditorStyles.boldLabel);
        for (int i = 0; i < _serverEntries.Count; i++)
        {
            var entry = _serverEntries[i];

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // 第一行：在线标记、名称、Remote、校验码、IP、Port、操作按钮
            EditorGUILayout.BeginHorizontal();
            // 在线状态圆点
            if (entry.hasOnlineState)
            {
                Color oldColor = GUI.color;
                GUI.color = entry.isOnline ? Color.green : Color.red;
                GUILayout.Label("●", GUILayout.Width(15));
                GUI.color = oldColor;
            }
            else
            {
                Color oldColor = GUI.color;
                GUI.color = Color.gray;
                GUILayout.Label("●", GUILayout.Width(15));
                GUI.color = oldColor;
            }

            EditorGUILayout.LabelField("名称:", GUILayout.Width(35));
            string newName = EditorGUILayout.TextField(entry.name, GUILayout.MinWidth(60));
            if (newName != entry.name)
            {
                entry.name = newName;
                SaveRemoteSettings();
            }

            EditorGUILayout.LabelField("Remote:", GUILayout.Width(50));
            string newRemote = EditorGUILayout.TextField(entry.remote, GUILayout.MinWidth(80));
            if (newRemote != entry.remote)
            {
                entry.remote = newRemote;
                SaveRemoteSettings();
            }

            EditorGUILayout.LabelField("校验码:", GUILayout.Width(40));
            string newCode = EditorGUILayout.TextField(entry.verifyCode, GUILayout.MinWidth(80));
            if (newCode != entry.verifyCode)
            {
                entry.verifyCode = newCode;
                SaveVerifyCode(entry);
            }

            EditorGUILayout.LabelField("IP:", GUILayout.Width(25));
            entry.ip = EditorGUILayout.TextField(entry.ip, GUILayout.Width(100));
            EditorGUILayout.LabelField("Port:", GUILayout.Width(30));
            entry.port = EditorGUILayout.IntField(entry.port, GUILayout.Width(60));

            GUI.enabled = i > 0;
            if (GUILayout.Button("↑", GUILayout.Width(25)))
            {
                var tmp = _serverEntries[i];
                _serverEntries[i] = _serverEntries[i - 1];
                _serverEntries[i - 1] = tmp;
                SaveRemoteSettings();
            }
            GUI.enabled = true;
            GUI.enabled = i < _serverEntries.Count - 1;
            if (GUILayout.Button("↓", GUILayout.Width(25)))
            {
                var tmp = _serverEntries[i];
                _serverEntries[i] = _serverEntries[i + 1];
                _serverEntries[i + 1] = tmp;
                SaveRemoteSettings();
            }
            GUI.enabled = true;
            if (GUILayout.Button("✕", GUILayout.Width(25)))
            {
                if (entry.isExecuting)
                {
                    EditorUtility.DisplayDialog("提示", "该服务器正在执行任务，请等待完成", "确定");
                }
                else
                {
                    DeleteVerifyCode(entry);
                    _serverEntries.RemoveAt(i);
                    i--;
                    SaveRemoteSettings();
                }
            }
            EditorGUILayout.EndHorizontal();

            // 远程函数标题行，右侧添加刷新按钮
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            string methodsArrow = entry.methodsExpanded ? "▼" : "▶";
            if (GUILayout.Button(methodsArrow, GUILayout.Width(25)))
                entry.methodsExpanded = !entry.methodsExpanded;
            string methodCount = entry.remoteMethods?.Count > 0 ? $" ({entry.remoteMethods.Count})" : "";
            EditorGUILayout.LabelField($"远程函数{methodCount}", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("刷新", GUILayout.Width(50)))
            {
                RefreshRemoteMethodsForEntry(entry);
            }
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
                        {
                            entry.cts?.Cancel();
                            entry.cts = new CancellationTokenSource();
                            _ = ExecuteSingleMethodOnServer(entry, methodName, entry.cts.Token);
                        }
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

            DrawParamList(ref entry.paramExpanded, entry.paramItems, "重载参数");

            GUI.enabled = !entry.isExecuting;
            if (GUILayout.Button(entry.isExecuting ? "执行中..." : "执行 Remote", GUILayout.Height(30)))
            {
                if (string.IsNullOrEmpty(entry.remote))
                {
                    EditorUtility.DisplayDialog("提示", "请输入 Remote 名称", "确定");
                }
                else if (string.IsNullOrEmpty(entry.verifyCode))
                {
                    EditorUtility.DisplayDialog("提示", "请输入校验码", "确定");
                }
                else
                {
                    entry.cts?.Cancel();
                    entry.cts = new CancellationTokenSource();
                    _ = ExecuteRemoteOnServer(entry, entry.cts.Token);
                }
            }
            GUI.enabled = true;

            if (entry.isExecuting)
            {
                EditorGUILayout.Space(5);
                Rect progressRect = EditorGUILayout.GetControlRect(false, 18);
                EditorGUI.ProgressBar(progressRect, entry.progress / 100f, $"{entry.progress:F0}%");
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);
        }

        // 底部按钮：Ping服务器
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Ping服务器", GUILayout.Height(30), GUILayout.ExpandWidth(true)))
        {
            PingAllServers();
        }
        if (GUILayout.Button("增加服务器", GUILayout.Height(30), GUILayout.ExpandWidth(true)))
        {
            var newEntry = new ServerEntry
            {
                ip = "127.0.0.1",
                port = 8888,
                remote = $"Remote{_serverEntries.Count + 1}",
                name = $"新服务器 {_serverEntries.Count + 1}",
                verifyCode = ""
            };
            newEntry.id = GenerateNewId();
            _serverEntries.Add(newEntry);
            SaveRemoteSettings();
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        EditorGUILayout.LabelField("客户端响应日志", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("清理日志", GUILayout.Width(80)))
        {
            lock (_clientLogLock) { _clientLog.Clear(); }
            _clientScrollPos = Vector2.zero;
            Repaint();
        }
        EditorGUILayout.EndHorizontal();

        _clientScrollPos = EditorGUILayout.BeginScrollView(_clientScrollPos, GUILayout.ExpandHeight(true));
        DrawColoredLog(_clientLog);
        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndScrollView();
    }

    // ============================================================
    // 日志绘制
    // ============================================================
    private void DrawColoredLog(List<LogEntry> logList)
    {
        if (logList == null || logList.Count == 0) return;
        foreach (var entry in logList)
        {
            if (string.IsNullOrEmpty(entry.message)) continue;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("📋", GUILayout.Width(30), GUILayout.Height(18)))
                GUIUtility.systemCopyBuffer = entry.message;
            GUIStyle style = new GUIStyle(EditorStyles.label);
            if (entry.isError)
                style.normal.textColor = Color.red;
            GUILayout.Label(entry.message, style);
            EditorGUILayout.EndHorizontal();
        }
    }

    // ============================================================
    // 参数合并工具函数
    // ============================================================
    private List<RPCParamItem> MergeParamLists(List<RPCParamItem> highPriority, List<RPCParamItem> lowPriority)
    {
        var mergedDict = new Dictionary<string, RPCParamItem>();
        if (lowPriority != null)
        {
            foreach (var item in lowPriority)
            {
                if (item != null && item.enabled && !string.IsNullOrEmpty(item.key))
                    mergedDict[item.key] = item;
            }
        }
        if (highPriority != null)
        {
            foreach (var item in highPriority)
            {
                if (item != null && item.enabled && !string.IsNullOrEmpty(item.key))
                    mergedDict[item.key] = item;
            }
        }
        return mergedDict.Values.ToList();
    }

    // ============================================================
    // 服务端核心
    // ============================================================
    private void StartServer()
    {
        if (_isRunning) return;
        try
        {
            Debug.Log("[RPC] StartServer");
            AddServerLog($"启动服务器，监听端口 {_serverPort}", false);
            _listener = new TcpListener(IPAddress.Any, _serverPort);
            _listener.Start();
            _isRunning = true;
            EditorPrefs.SetBool("RPCServer_ShouldRun", true);
            AddServerLog($"服务器启动，监听端口 {_serverPort}", false);
            _listener.BeginAcceptTcpClient(OnClientConnected, null);
            SaveRemoteSettings();
        }
        catch (Exception e) { AddServerLog($"启动失败: {e.Message}", true); Debug.LogError("StartServer 异常: " + e.ToString()); }
        Repaint();
    }

    private void StopServer(bool clearShouldRun = false)
    {
        if (!_isRunning) return;
        _isRunning = false;
        _listener?.Stop();
        _listener = null;
        if (clearShouldRun) EditorPrefs.SetBool("RPCServer_ShouldRun", false);
        AddServerLog("服务器已停止", false);
        Repaint();
    }

    private void OnClientConnected(IAsyncResult ar)
    {
        if (!_isRunning) return;
        try
        {
            TcpClient client = _listener.EndAcceptTcpClient(ar);
            AddServerLog($"客户端已连接: {client.Client.RemoteEndPoint}", false);
            _listener.BeginAcceptTcpClient(OnClientConnected, null);
            _ = HandleClientAsync(client);
        }
        catch (ObjectDisposedException) { }
        catch (Exception e) { AddServerLog($"接受客户端失败: {e.Message}", true); }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            using (client)
            using (var stream = client.GetStream())
            {
                byte[] buffer = new byte[4096];
                StringBuilder sb = new StringBuilder();
                while (client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    string chunk = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    sb.Append(chunk);
                    int idx;
                    while ((idx = sb.ToString().IndexOf('\n')) >= 0)
                    {
                        string line = sb.ToString().Substring(0, idx).Trim();
                        sb.Remove(0, idx + 1);
                        if (!string.IsNullOrEmpty(line))
                        {
                            AddServerLog($"收到指令: {line}", false);
                            await ExecuteCommand(stream, client, line);
                        }
                    }
                }
            }
        }
        catch (Exception e) { AddServerLog($"客户端异常: {e.Message}", true); }
        finally { AddServerLog("客户端已断开", false); }
    }

    // ============================================================
    // 命令路由
    // ============================================================
    private async Task ExecuteCommand(NetworkStream stream, TcpClient client, string jsonLine)
    {
        try
        {
            var cmdObj = JsonUtility.FromJson<RPCCommand>(jsonLine);
            if (cmdObj == null)
            {
                await SendResult(stream, 1, "无效的命令格式");
                return;
            }
            if (string.IsNullOrEmpty(cmdObj.cmd))
            {
                await SendResult(stream, 1, "缺少命令类型");
                return;
            }
            if (string.IsNullOrEmpty(cmdObj.verifyCode))
            {
                await SendResult(stream, 1, "缺少校验码");
                return;
            }
            if (!VerifyCode(cmdObj.verifyCode, out string errMsg))
            {
                await SendResult(stream, 1, errMsg);
                return;
            }

            switch (cmdObj.cmd.ToLower())
            {
                case "remote":
                    await ExecuteRemote(stream, client, cmdObj);
                    break;
                case "resume":
                    await ExecuteResume(stream, client, cmdObj);
                    break;
                case "list":
                    await ExecuteList(stream, client, cmdObj);
                    break;
                default:
                    await SendResult(stream, 1, $"未知命令: {cmdObj.cmd}");
                    break;
            }
        }
        catch (Exception e)
        {
            await SendResult(stream, 1, $"执行异常: {e.Message}");
        }
    }

    // ============================================================
    // 校验辅助
    // ============================================================
    private bool VerifyCode(string clientCode, out string errorMsg)
    {
        if (string.IsNullOrEmpty(clientCode))
        {
            errorMsg = "缺少校验码";
            return false;
        }
        if (clientCode != _serverCode)
        {
            errorMsg = "校验码错误";
            return false;
        }
        errorMsg = null;
        return true;
    }

    // ============================================================
    // 执行 Remote
    // ============================================================
    private async Task ExecuteRemote(NetworkStream stream, TcpClient client, RPCCommand cmd)
    {
        string remote = cmd.remote;
        string methodName = cmd.methodName;

        if (string.IsNullOrEmpty(remote))
        {
            await SendResult(stream, 1, "缺少 Remote 名称");
            return;
        }
        if (!_rpcMethodsByRemote.TryGetValue(remote, out var methods))
        {
            await SendResult(stream, 1, $"Remote 未注册: {remote}");
            return;
        }

        var finalParamList = MergeParamLists(cmd.paramItems, _serverParams);

        if (!string.IsNullOrEmpty(methodName))
        {
            var targetMethod = methods.FirstOrDefault(m => (m.DeclaringType?.FullName ?? "Unknown") + "." + m.Name == methodName);
            if (targetMethod == null)
            {
                await SendResult(stream, 1, $"方法 {methodName} 在 Remote {remote} 中未找到");
                return;
            }
            await ExecuteSingleMethod(stream, client, remote, targetMethod, finalParamList);
            return;
        }

        _resumeRemote = null;
        _resumeMethodIndex = 0;
        _resumeParam = "";
        EnqueueOnMainThread(() =>
        {
            EditorPrefs.DeleteKey("RPC_ResumeRemote");
            EditorPrefs.DeleteKey("RPC_ResumeIndex");
            EditorPrefs.DeleteKey("RPC_ResumeParam");
        });

        await ExecuteMethods(stream, client, remote, methods, finalParamList, 0);
    }

    // ============================================================
    // 执行单个方法（内部）
    // ============================================================
    private async Task ExecuteSingleMethod(NetworkStream stream, TcpClient client, string remote, MethodInfo method, List<RPCParamItem> paramList)
    {
        AddServerLog($"开始执行单个方法 {method.Name} (Remote: {remote})", false);
        await SendProgress(stream, $"执行方法 {method.Name}", 0f);

        try
        {
            var pars = method.GetParameters();
            if (pars.Length > 1)
            {
                await SendResult(stream, 1, $"方法 {method.Name} 参数过多，只支持无参或单个 RPCArgs");
                return;
            }

            var cmder = new RPCArgs(paramList);
            var tcs = new TaskCompletionSource<(bool, object)>();
            Exception invokeEx = null;

            EnqueueOnMainThread(() =>
            {
                try
                {
                    object[] args = null;
                    if (pars.Length == 1)
                    {
                        if (pars[0].ParameterType != typeof(RPCArgs))
                        {
                            invokeEx = new Exception($"参数类型必须是 RPCArgs");
                            tcs.SetResult((false, null));
                            return;
                        }
                        args = new object[] { cmder };
                    }
                    object result = method.Invoke(null, args);
                    tcs.SetResult((true, result));
                }
                catch (Exception e) { invokeEx = e; tcs.SetResult((false, null)); }
            });

            var (ok, resultObj) = await tcs.Task;
            if (!ok && invokeEx != null)
            {
                string err = $"方法 {method.Name} 执行异常: {invokeEx.ToString()}";
                AddServerLog(err, true);
                await SendProgress(stream, err);
                await SendResult(stream, 1, err);
                return;
            }

            if (resultObj is string strResult && !string.IsNullOrEmpty(strResult))
            {
                AddServerLog($"方法 {method.Name} 返回: {strResult}", false);
                await SendProgress(stream, $"返回值: {strResult}");
            }

            AddServerLog($"方法执行成功: {method.Name}", false);
            await SendProgress(stream, $"成功: {method.Name}", 100f);
            await SendResult(stream, 0, $"方法 {method.Name} 执行成功");
        }
        catch (Exception ex)
        {
            AddServerLog($"执行方法 {method.Name} 异常: {ex.Message}", true);
            await SendResult(stream, 1, ex.Message);
        }
    }

    // ============================================================
    // 续传执行
    // ============================================================
    private async Task ExecuteResume(NetworkStream stream, TcpClient client, RPCCommand cmd)
    {
        string remote = EditorPrefs.GetString("RPC_ResumeRemote", "");
        int methodIndex = EditorPrefs.GetInt("RPC_ResumeIndex", 0);
        string paramJson = EditorPrefs.GetString("RPC_ResumeParam", "");
        if (string.IsNullOrEmpty(remote))
        {
            await SendResult(stream, 1, "无续传上下文");
            return;
        }

        EnqueueOnMainThread(() =>
        {
            EditorPrefs.DeleteKey("RPC_ResumeRemote");
            EditorPrefs.DeleteKey("RPC_ResumeIndex");
            EditorPrefs.DeleteKey("RPC_ResumeParam");
        });

        if (!_rpcMethodsByRemote.TryGetValue(remote, out var methods))
        {
            await SendResult(stream, 1, "续传 Remote 已失效");
            return;
        }

        List<RPCParamItem> paramList = null;
        if (!string.IsNullOrEmpty(paramJson))
        {
            try
            {
                var wrapper = JsonUtility.FromJson<SerializableList<RPCParamItem>>(paramJson);
                paramList = wrapper?.list;
            }
            catch { }
        }
        if (paramList == null)
            paramList = new List<RPCParamItem>();

        _resumeRemote = remote;
        _resumeMethodIndex = methodIndex;
        _resumeParam = paramJson;
        await ExecuteMethods(stream, client, remote, methods, paramList, methodIndex);
    }

    // ============================================================
    // 核心执行循环（全部方法）
    // ============================================================
    private async Task ExecuteMethods(NetworkStream stream, TcpClient client, string remote, List<MethodInfo> methods, List<RPCParamItem> paramList, int startIndex)
    {
        AddServerLog($"开始执行 Remote [{remote}]，从方法 {startIndex + 1}/{methods.Count} 开始", false);
        await SendProgress(stream, $"开始执行 Remote [{remote}] (共 {methods.Count} 个方法)", (startIndex / (float)methods.Count) * 100f);

        _resumeRemote = remote;
        _resumeMethodIndex = startIndex;
        var wrapper = new SerializableList<RPCParamItem> { list = paramList };
        _resumeParam = JsonUtility.ToJson(wrapper);
        _isCompiling = false;

        var cmder = new RPCArgs(paramList);

        int successCount = 0;
        for (int i = startIndex; i < methods.Count; i++)
        {
            var method = methods[i];
            string methodName = $"{method.DeclaringType?.FullName}.{method.Name}";

            _resumeMethodIndex = i;

            var pars = method.GetParameters();
            if (pars.Length > 1)
            {
                await SendResult(stream, 1, $"方法 {methodName} 参数过多，只支持无参或单个 RPCArgs");
                try { client.Close(); } catch { }
                return;
            }

            var tcs = new TaskCompletionSource<(bool, object)>();
            Exception invokeEx = null;

            EnqueueOnMainThread(() =>
            {
                try
                {
                    object[] args = null;
                    if (pars.Length == 1)
                    {
                        if (pars[0].ParameterType != typeof(RPCArgs))
                        {
                            invokeEx = new Exception($"参数类型必须是 RPCArgs");
                            tcs.SetResult((false, null));
                            return;
                        }
                        args = new object[] { cmder };
                    }
                    object result = method.Invoke(null, args);
                    tcs.SetResult((true, result));
                }
                catch (Exception e) { invokeEx = e; tcs.SetResult((false, null)); }
            });

            var (ok, resultObj) = await tcs.Task;
            if (!ok && invokeEx != null)
            {
                string err = $"方法 {methodName} 执行异常: {invokeEx.ToString()}";
                AddServerLog(err, true);
                await SendProgress(stream, err);
                await SendResult(stream, 1, err);
                try { client.Close(); } catch { }
                return;
            }

            if (resultObj is string strResult && !string.IsNullOrEmpty(strResult))
            {
                AddServerLog($"方法 {methodName} 返回: {strResult}", false);
                await SendProgress(stream, $"返回值: {strResult}");
            }

            successCount++;
            float progress = ((i + 1) / (float)methods.Count) * 100f;
            AddServerLog($"方法执行成功: {methodName}", false);
            await SendProgress(stream, $"成功: {methodName}", progress);

            if (_isCompiling)
            {
                AddServerLog($"检测到编译，断开连接，续传状态已保存", false);
                try { client.Close(); } catch { }
                return;
            }
        }

        await SendResult(stream, 0, $"Remote [{remote}] 执行完成，共 {successCount}/{methods.Count} 个方法成功");
        _resumeRemote = null;
        _resumeMethodIndex = 0;
        _resumeParam = "";
        EnqueueOnMainThread(() =>
        {
            EditorPrefs.DeleteKey("RPC_ResumeRemote");
            EditorPrefs.DeleteKey("RPC_ResumeIndex");
            EditorPrefs.DeleteKey("RPC_ResumeParam");
        });
        try { client.Close(); } catch { }
    }

    // ============================================================
    // 执行 List 命令
    // ============================================================
    private async Task ExecuteList(NetworkStream stream, TcpClient client, RPCCommand cmd)
    {
        string remote = cmd.remote;
        if (string.IsNullOrEmpty(remote))
        {
            await SendResult(stream, 1, "缺少 Remote 名称");
            return;
        }

        if (!_rpcMethodsByRemote.TryGetValue(remote, out var methods))
        {
            await SendResult(stream, 1, $"Remote 未注册: {remote}");
            return;
        }

        var methodNames = methods.Select(m => (m.DeclaringType?.FullName ?? "Unknown") + "." + m.Name).ToList();
        string message = string.Join(",", methodNames);

        var methodsMsg = new RPCMessage { type = "methods", code = 0, message = message };
        string json = JsonUtility.ToJson(methodsMsg);
        byte[] data = Encoding.UTF8.GetBytes(json + "\n");
        await stream.WriteAsync(data, 0, data.Length);

        await SendResult(stream, 0, $"获取到 {methodNames.Count} 个方法");
    }

    // ============================================================
    // 客户端执行全部方法
    // ============================================================
    private async Task ExecuteRemoteOnServer(ServerEntry entry, CancellationToken cancellationToken)
    {
        if (entry.isExecuting) return;
        entry.isExecuting = true;
        entry.progress = 0f;
        var token = cancellationToken;

        string displayName = string.IsNullOrEmpty(entry.name) ? entry.remote : entry.name;
        string serverLabel = string.IsNullOrEmpty(displayName) ? $"{entry.ip}:{entry.port}" : displayName;

        try
        {
            AppendClientLog($"========== 执行 Remote [{entry.remote}] 到 [{serverLabel}] ==========");

            var mergedParams = MergeParamLists(entry.paramItems, _clientGlobalParams);

            var cmdObj = new RPCCommand
            {
                cmd = "remote",
                remote = entry.remote,
                paramItems = mergedParams,
                methodName = "",
                verifyCode = entry.verifyCode
            };
            string jsonCmd = JsonUtility.ToJson(cmdObj);
            AppendClientLog($"发送命令: {jsonCmd}");
            var result = await SendCommandAndWaitForReconnect(entry, jsonCmd, token);
            if (token.IsCancellationRequested) return;

            int retryCount = 0;
            const int maxRetries = 30;
            while (result.needResume && !token.IsCancellationRequested && retryCount < maxRetries)
            {
                retryCount++;
                AppendClientLog($"检测到编译，等待服务端重启后续传... (第{retryCount}次)");
                bool reconnected = await WaitForServerRestart(entry.ip, entry.port, 30, 2000, token);
                if (!reconnected)
                {
                    AppendClientLog($"错误: 服务器 [{serverLabel}] 重启超时", true);
                    break;
                }
                AppendClientLog("服务端已重启，发送 resume...");
                var resumeCmd = new RPCCommand
                {
                    cmd = "resume",
                    verifyCode = entry.verifyCode
                };
                jsonCmd = JsonUtility.ToJson(resumeCmd);
                result = await SendCommandAndWaitForReconnect(entry, jsonCmd, token);
            }

            if (retryCount >= maxRetries && result.needResume)
                AppendClientLog($"警告: 达到最大续传尝试次数 ({maxRetries})，可能未完全执行", true);

            AppendClientLog($"========== [{serverLabel}] 执行完成 ==========");
        }
        catch (OperationCanceledException)
        {
            AppendClientLog($"操作已被取消 [{serverLabel}]");
        }
        catch (Exception ex)
        {
            AppendClientLog($"执行异常 [{serverLabel}]: {ex.ToString()}", true);
        }
        finally
        {
            entry.isExecuting = false;
            entry.progress = 100f;
            entry.cts?.Dispose();
            entry.cts = null;
            Repaint();
        }
    }

    // ============================================================
    // 客户端执行单个方法
    // ============================================================
    private async Task ExecuteSingleMethodOnServer(ServerEntry entry, string methodName, CancellationToken cancellationToken)
    {
        if (entry.isExecuting) return;
        entry.isExecuting = true;
        entry.progress = 0f;
        var token = cancellationToken;

        string displayName = string.IsNullOrEmpty(entry.name) ? entry.remote : entry.name;
        string serverLabel = string.IsNullOrEmpty(displayName) ? $"{entry.ip}:{entry.port}" : displayName;

        try
        {
            AppendClientLog($"========== 执行方法 [{methodName}] 到 [{serverLabel}] ==========");

            var mergedParams = MergeParamLists(entry.paramItems, _clientGlobalParams);

            var cmdObj = new RPCCommand
            {
                cmd = "remote",
                remote = entry.remote,
                paramItems = mergedParams,
                methodName = methodName,
                verifyCode = entry.verifyCode
            };
            string jsonCmd = JsonUtility.ToJson(cmdObj);
            AppendClientLog($"发送命令: {jsonCmd}");
            var result = await SendCommandAndWaitForReconnect(entry, jsonCmd, token);
            if (token.IsCancellationRequested) return;

            AppendClientLog($"========== 方法 [{methodName}] 执行完成 ==========");
        }
        catch (OperationCanceledException)
        {
            AppendClientLog($"操作已被取消 [{serverLabel}]");
        }
        catch (Exception ex)
        {
            AppendClientLog($"执行异常 [{serverLabel}]: {ex.ToString()}", true);
        }
        finally
        {
            entry.isExecuting = false;
            entry.progress = 100f;
            entry.cts?.Dispose();
            entry.cts = null;
            Repaint();
        }
    }

    // ============================================================
    // 刷新所有服务器的远程方法列表（已废弃，改用单个刷新）
    // ============================================================
    private async void RefreshRemoteMethodsForEntry(ServerEntry entry)
    {
        if (string.IsNullOrEmpty(entry.remote))
        {
            AppendClientLog($"跳过 {entry.ip}:{entry.port}（Remote 为空）");
            return;
        }
        if (string.IsNullOrEmpty(entry.verifyCode))
        {
            AppendClientLog($"跳过 {entry.remote}（校验码为空）");
            return;
        }

        AppendClientLog($"刷新 {entry.remote} 方法列表...");
        try
        {
            var cmdObj = new RPCCommand
            {
                cmd = "list",
                remote = entry.remote,
                verifyCode = entry.verifyCode
            };
            string jsonCmd = JsonUtility.ToJson(cmdObj);
            using (var cts = new CancellationTokenSource())
            {
                var result = await SendCommandAndWaitForReconnect(entry, jsonCmd, cts.Token);
                if (result.needResume)
                {
                    AppendClientLog($"刷新 {entry.remote} 失败: 服务端未响应或校验失败", true);
                }
                else
                {
                    AppendClientLog($"刷新 {entry.remote} 成功，获取到 {entry.remoteMethods?.Count ?? 0} 个方法");
                }
            }
        }
        catch (Exception ex)
        {
            AppendClientLog($"刷新 {entry.remote} 异常: {ex.Message}", true);
        }
        Repaint();
    }

    // ============================================================
    // Ping 所有服务器
    // ============================================================
    private async void PingAllServers()
    {
        AppendClientLog("========== 开始Ping所有服务器 ==========");
        var tasks = _serverEntries.Select(entry => PingServerAsync(entry)).ToArray();
        await Task.WhenAll(tasks);
        AppendClientLog("========== Ping完成 ==========");
        Repaint();
    }

    private async Task PingServerAsync(ServerEntry entry)
    {
        try
        {
            using (var client = new TcpClient())
            {
                var connectTask = client.ConnectAsync(entry.ip, entry.port);
                if (await Task.WhenAny(connectTask, Task.Delay(2000)) == connectTask)
                {
                    // 连接成功
                    await connectTask;
                    entry.isOnline = true;
                    entry.hasOnlineState = true;
                    AppendClientLog($"[{entry.name}] 在线");
                }
                else
                {
                    entry.isOnline = false;
                    entry.hasOnlineState = true;
                    AppendClientLog($"[{entry.name}] 离线 (超时)");
                }
            }
        }
        catch
        {
            entry.isOnline = false;
            entry.hasOnlineState = true;
            AppendClientLog($"[{entry.name}] 离线 (异常)");
        }
        Repaint();
    }

    // ============================================================
    // 发送命令并等待响应（含进度更新）
    // ============================================================
    private async Task<(bool needReconnect, bool needResume)> SendCommandAndWaitForReconnect(ServerEntry entry, string jsonCommand, CancellationToken token)
    {
        string displayName = string.IsNullOrEmpty(entry.name) ? entry.remote : entry.name;
        string serverLabel = string.IsNullOrEmpty(displayName) ? $"{entry.ip}:{entry.port}" : displayName;

        AppendClientLog($"发送命令: {jsonCommand}");
        bool needReconnect = true;
        bool needResume = false;
        bool receivedResult = false;
        bool isSuccess = false;
        bool shouldExit = false;

        try
        {
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(entry.ip, entry.port);
                using (var stream = client.GetStream())
                {
                    byte[] send = Encoding.UTF8.GetBytes(jsonCommand + "\n");
                    await stream.WriteAsync(send, 0, send.Length, token);

                    byte[] buffer = new byte[4096];
                    StringBuilder sb = new StringBuilder();
                    while (!shouldExit)
                    {
                        token.ThrowIfCancellationRequested();
                        int bytes = await stream.ReadAsync(buffer, 0, buffer.Length, token);
                        if (bytes == 0) break;
                        string chunk = Encoding.UTF8.GetString(buffer, 0, bytes);
                        sb.Append(chunk);
                        int idx;
                        while ((idx = sb.ToString().IndexOf('\n')) >= 0)
                        {
                            string line = sb.ToString().Substring(0, idx).Trim();
                            sb.Remove(0, idx + 1);
                            if (string.IsNullOrEmpty(line)) continue;

                            try
                            {
                                var msg = JsonUtility.FromJson<RPCMessage>(line);
                                if (msg == null) continue;

                                if (msg.type == "progress")
                                {
                                    if (msg.progress >= 0f)
                                    {
                                        entry.progress = msg.progress;
                                        Repaint();
                                    }
                                    AppendClientLog($"[{serverLabel}][进度] {msg.message}");
                                }
                                else if (msg.type == "methods")
                                {
                                    if (!string.IsNullOrEmpty(msg.message))
                                    {
                                        var methodNames = msg.message.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                        entry.remoteMethods = methodNames;
                                        AppendClientLog($"[{serverLabel}] 获取到 {methodNames.Count} 个方法");
                                    }
                                    else
                                    {
                                        entry.remoteMethods = new List<string>();
                                    }
                                }
                                else if (msg.type == "result")
                                {
                                    if (msg.code == 0)
                                    {
                                        AppendClientLog($"[{serverLabel}][结果] 成功: {msg.message}");
                                        isSuccess = true;
                                        needReconnect = false;
                                    }
                                    else
                                    {
                                        AppendClientLog($"[{serverLabel}][结果] 错误码 {msg.code}: {msg.message}", true);
                                        needReconnect = false;
                                        needResume = false;
                                    }
                                    receivedResult = true;
                                    shouldExit = true;
                                }
                                else
                                {
                                    AppendClientLog($"[{serverLabel}][信息] {line}");
                                }
                            }
                            catch
                            {
                                AppendClientLog($"[{serverLabel}][信息] {line}");
                            }
                        }
                    }
                }
            }
            if (!receivedResult || !isSuccess)
            {
                needReconnect = true;
                needResume = false;
            }
        }
        catch (OperationCanceledException) { throw; }
        catch (Exception e)
        {
            AppendClientLog($"[{serverLabel}] 连接异常: {e.Message}", true);
            needReconnect = true;
            try
            {
                var cmdObj = JsonUtility.FromJson<RPCCommand>(jsonCommand);
                if (cmdObj != null && cmdObj.cmd == "remote" && !receivedResult)
                    needResume = true;
            }
            catch { }
        }
        return (needReconnect, needResume);
    }

    // ============================================================
    // 重连等待辅助
    // ============================================================
    private async Task<bool> WaitForServerRestart(string ip, int port, int maxRetries, int delayMs, CancellationToken token)
    {
        AppendClientLog($"等待服务端重启... (最多尝试 {maxRetries} 次)");
        for (int i = 0; i < maxRetries; i++)
        {
            token.ThrowIfCancellationRequested();
            await Task.Delay(delayMs, token);
            try
            {
                using (var c = new TcpClient())
                {
                    await c.ConnectAsync(ip, port);
                    AppendClientLog($"服务端已重启，连接成功 (尝试 {i + 1}/{maxRetries})");
                    return true;
                }
            }
            catch
            {
                if ((i + 1) % 5 == 0) AppendClientLog($"等待中... ({i + 1}/{maxRetries})");
            }
        }
        AppendClientLog("服务端重启超时", true);
        return false;
    }

    // ============================================================
    // 日志辅助
    // ============================================================
    private void AppendClientLog(string msg, bool isError = false)
    {
        if (string.IsNullOrEmpty(msg)) return;
        lock (_clientLogLock)
        {
            _clientLog.Add(new LogEntry(msg, isError));
        }
        _clientScrollPos.y = float.MaxValue;
        Repaint();
    }

    private void AddServerLog(string msg, bool isError = false)
    {
        if (string.IsNullOrEmpty(msg)) return;
        lock (_serverLogLock)
        {
            _serverLog.Add(new LogEntry(msg, isError));
        }
        EnqueueOnMainThread(() => Repaint());
    }

    // ============================================================
    // 发送辅助（结构化消息，支持进度）
    // ============================================================
    private async Task SendProgress(NetworkStream stream, string msg, float progress = -1f)
    {
        var rpcMsg = new RPCMessage { type = "progress", code = 0, message = msg, progress = progress };
        string json = JsonUtility.ToJson(rpcMsg);
        byte[] data = Encoding.UTF8.GetBytes(json + "\n");
        await stream.WriteAsync(data, 0, data.Length);
    }

    private async Task SendResult(NetworkStream stream, int code, string msg)
    {
        var rpcMsg = new RPCMessage { type = "result", code = code, message = msg };
        string json = JsonUtility.ToJson(rpcMsg);
        byte[] data = Encoding.UTF8.GetBytes(json + "\n");
        await stream.WriteAsync(data, 0, data.Length);
    }
}