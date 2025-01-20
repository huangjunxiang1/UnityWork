using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[Serializable]
public class DirectoryItem
{
    public string self;
    public string target;
}
[CreateAssetMenu(menuName = "Shit/" + nameof(ShitSettings), fileName = nameof(ShitSettings), order = 1)]
public class ShitSettings : ScriptableObject
{
    [SerializeField]
    internal string _MainPath = "/Code/Main/";
    [SerializeField]
    internal string _HotPath = "/Code/HotFix/";
    [SerializeField]
    internal List<DirectoryItem> SyncDirs = new();

    static ShitSettings _inst;
    public static ShitSettings Inst => get();
    internal SerializedObject target;

    
    static ShitSettings get()
    {
        if (!_inst)
        {
            _inst = InternalEditorUtility.LoadSerializedFileAndForget("ProjectSettings/" + nameof(ShitSettings) + ".asset").FirstOrDefault() as ShitSettings;
            if (!_inst)
            {
                _inst = CreateInstance<ShitSettings>();
                InternalEditorUtility.SaveToSerializedFileAndForget(new UnityEngine.Object[] { _inst }, "ProjectSettings/" + nameof(ShitSettings) + ".asset", true);
            }
            _inst.target = new SerializedObject(_inst);
        }
        return _inst;
    }

    public string MainPath => Application.dataPath + MainPath;
    public string HotPath => Application.dataPath + _HotPath;

}
class ShitProvider : SettingsProvider
{
    public ShitProvider() : base($"Project/Shit", SettingsScope.Project)
    {
        
    }
    public override void OnGUI(string searchContext)
    {
        ShitSettings.Inst._MainPath = EditorGUILayout.TextField(nameof(ShitSettings.Inst.MainPath), ShitSettings.Inst._MainPath);
        ShitSettings.Inst._HotPath = EditorGUILayout.TextField(nameof(ShitSettings.Inst.HotPath), ShitSettings.Inst._HotPath);
        EditorGUILayout.PropertyField(ShitSettings.Inst.target.FindProperty(nameof(ShitSettings.Inst.SyncDirs)));
        if (EditorGUI.EndChangeCheck())
        {
            ShitSettings.Inst.target.ApplyModifiedProperties();
            InternalEditorUtility.SaveToSerializedFileAndForget(new UnityEngine.Object[] { ShitSettings.Inst }, "ProjectSettings/" + nameof(ShitSettings) + ".asset", true);
        }
    }

    static ShitProvider provider;
    [SettingsProvider]
    static SettingsProvider CreateMyCustomSettingsProvider()
    {
        if (provider == null)
            provider = new ShitProvider();
        return provider;
    }
}

