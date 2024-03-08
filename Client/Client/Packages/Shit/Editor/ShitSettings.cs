using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(menuName = "Shit/" + nameof(ShitSettings), fileName = nameof(ShitSettings), order = 1)]
public class ShitSettings : ScriptableObject
{
    [SerializeField]
    internal string _MainPath = "/Code/Main/";
    [SerializeField]
    internal string _HotPath = "/Code/HotFix/";

    static ShitSettings _inst;
    public static ShitSettings Inst => get();

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
        }
        return _inst;
    }

    public string MainPath => Application.dataPath + MainPath;
    public string HotPath => Application.dataPath + _HotPath;

    [SettingsProvider]
    static SettingsProvider CreateMyCustomSettingsProvider()
    {
        var provider = new SettingsProvider($"Project/Shit", SettingsScope.Project)
        {
            label = "Shit",
            guiHandler = (searchContext) =>
            {
                EditorGUILayout.TextField(nameof(ShitSettings.Inst.MainPath), ShitSettings.Inst._MainPath);
                EditorGUILayout.TextField(nameof(ShitSettings.Inst.HotPath), ShitSettings.Inst._HotPath);
            }
        };
        return provider;
    }
}
