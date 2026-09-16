using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
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
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RPCQueryAttribute : Attribute
{
    public string Remote { get; set; }
    public RPCQueryAttribute(string remote)
    {
        Remote = remote;
    }
}


[Serializable]
public class RPCParamItem
{
    public bool enabled = true;
    public string key = "";
    public string value = "";
}

public class RPCArgs
{
    public Dictionary<string, string> map = new Dictionary<string, string>();
    public RPCArgs(Dictionary<string, string> items)
    {
        map = items;
    }
    public string this[string key] => map.TryGetValue(key, out string v) ? v : null;
    public string GetValueOrDefault(string key, string def = "") => map.TryGetValue(key, out string v) ? v : def;
}

internal partial class RPCWindow : EditorWindow
{
    private string _projectId = null;
    string GetCombineKey(string key)
    {
        if (_projectId == null)
        {
            string projectName = PlayerSettings.productName;
            string dataPath = Application.dataPath;
            _projectId = $"{projectName}_{dataPath.GetHashCode()}";
        }
        return $"{_projectId}_{key}";
    }

    [MenuItem("Shit/RPC Window")]
    public static void ShowWindow() => GetWindow<RPCWindow>("RPC 窗口");

    Mode _currentMode = Mode.Client;
    RPCWindow_Client client;
    RPCWindow_Server server;
    List<LogEntry> _allLogs = new List<LogEntry>();
    ConcurrentQueue<Action> actions = new();


    public void WaitRepaint() => actions.Enqueue(Repaint);
    public Task WaitMainThread(Action action)
    {
        var tcs = new TaskCompletionSource<bool>();
        actions.Enqueue(() =>
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception e)
            { }
            tcs.TrySetResult(true);
        });
        return tcs.Task;
    }

    private void OnEnable()
    {
        _currentMode = (Mode)EditorPrefs.GetInt(GetCombineKey("RPCWindow_Mode"), 0);
        switch (_currentMode)
        {
            case Mode.Client:
                client = new(this);
                break;
            case Mode.Server:
                server = new(this);
                break;
            default:
                break;
        }
    }
    private void OnDisable()
    {
        client?.Dispose();
        server?.Dispose();
    }
    private void Update()
    {
        client?.Update();
        server?.Update();
        while (actions.TryDequeue(out var act))
            act.Invoke();
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        Mode newMode = (Mode)EditorGUILayout.EnumPopup("模式", _currentMode);
        if (newMode != _currentMode)
        {
            _currentMode = newMode;
            EditorPrefs.SetInt(GetCombineKey("RPCWindow_Mode"), (int)_currentMode);
            if (_currentMode == Mode.Client)
            {
                client = new(this);
                server?.Dispose();
                server = null;
            }
            else if (_currentMode == Mode.Server)
            {
                server = new(this);
                client?.Dispose();
                client = null;
            }
        }
        EditorGUILayout.EndHorizontal();

        client?.DrawUI();
        server?.DrawUI();
    }
    void DrawColoredLog(List<LogEntry> logList, string search)
    {
        if (logList == null || logList.Count == 0) return;

        float availableWidth = EditorGUIUtility.currentViewWidth - 35f;
        bool hasSearch = !string.IsNullOrEmpty(search);

        int len = logList.Count;
        for (int i = 0; i < len; i++)
        {
            var entry = logList[i];
            if (hasSearch && entry.message.IndexOf(search, StringComparison.OrdinalIgnoreCase) < 0)
                continue;

            string displayText = string.IsNullOrEmpty(entry.message) ? " " : entry.message;
            var style = new GUIStyle(EditorStyles.label);
            style.wordWrap = true;
            style.margin = new RectOffset(0, 0, 0, 0);
            style.padding = new RectOffset(2, 2, 0, 0);
            if (entry.type == LogType.Error)
            {
                style.normal.textColor = Color.red;
                style.hover.textColor = Color.red;
                style.active.textColor = Color.red;
                style.focused.textColor = Color.red;
            }
            float height = style.CalcHeight(new GUIContent(entry.message), availableWidth);
            EditorGUILayout.SelectableLabel(displayText, style,
                GUILayout.ExpandWidth(true),
                GUILayout.Height(height));
        }
    }
    void DrawParamContent(List<RPCParamItem> paramList, Action click)
    {
        if (paramList == null) return;
        for (int p = 0; p < paramList.Count; p++)
        {
            var param = paramList[p];
            EditorGUILayout.BeginHorizontal();
            var enable = EditorGUILayout.Toggle(param.enabled, GUILayout.Width(20));
            if (param.enabled != enable)
            {
                param.enabled = enable;
                click();
            }
            var key = EditorGUILayout.TextField(param.key, GUILayout.Width(100));
            if (param.key != key)
            {
                param.key = key;
                click();
            }
            var value = EditorGUILayout.TextField(param.value, GUILayout.ExpandWidth(true));
            if (param.value != value)
            {
                param.value = value;
                click();
            }

            GUI.enabled = p > 0;
            if (GUILayout.Button("↑", GUILayout.Width(25)))
            {
                var temp = paramList[p];
                paramList[p] = paramList[p - 1];
                paramList[p - 1] = temp;
                click();
            }
            GUI.enabled = true;
            GUI.enabled = p < paramList.Count - 1;
            if (GUILayout.Button("↓", GUILayout.Width(25)))
            {
                var temp = paramList[p];
                paramList[p] = paramList[p + 1];
                paramList[p + 1] = temp;
                click();
            }
            GUI.enabled = true;
            if (GUILayout.Button("✕", GUILayout.Width(25)))
            {
                paramList.RemoveAt(p);
                p--;
                click();
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private enum Mode { Client, Server }
    private enum LogFilter { All, Normal, Error }
    private class LogEntry
    {
        public string message;
        public LogType type;
    }
    private enum RPCCmdId
    {
        GetStatus,
        ExcuteRemote,
    }
    private class RPCCommand
    {
        public int cmd;
        public string requestId;
        public string remote;
        public Dictionary<string, string> paramItems = new();
        public string methodName;
        public string verifyCode;
    }
    private class RPCResult
    {
        public int cmd;
        public string requestId;
        public int code = -1;
        public string message;
        public List<string> methods = new();
        public Dictionary<string, string> envVar = new();
        public RemoteExcuteInfo currentExcuteInfo = new();
    }
    class RemoteExcuteInfo
    {
        public bool isExecuting;
        public string remote;
        public List<string> methods = new();
        public Dictionary<string, string> args = new Dictionary<string, string>();
        public int currentMethodIndex;
        public int totalMethodCount;
    }
}
