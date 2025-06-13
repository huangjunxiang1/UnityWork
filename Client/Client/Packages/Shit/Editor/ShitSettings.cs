using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[Serializable]
public class DirectoryItem
{
    public bool rootIsAppDataPath;
    public string self;
    public string target;
}
[CreateAssetMenu(menuName = "Shit/" + nameof(ShitSettings), fileName = nameof(ShitSettings), order = 1)]
public class ShitSettings : ScriptableObject
{
    public string src;
    public float PathNodeDrawLineRadius = 0.5f;

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

}
class ShitProvider : SettingsProvider
{
    public ShitProvider() : base($"Project/Shit", SettingsScope.Project)
    {
        
    }
    public override void OnGUI(string searchContext)
    {
        ShitSettings.Inst.src = EditorGUILayout.TextField(nameof(ShitSettings.Inst.src), ShitSettings.Inst.src);
        ShitSettings.Inst.PathNodeDrawLineRadius = EditorGUILayout.FloatField(nameof(ShitSettings.Inst.PathNodeDrawLineRadius), ShitSettings.Inst.PathNodeDrawLineRadius);
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

