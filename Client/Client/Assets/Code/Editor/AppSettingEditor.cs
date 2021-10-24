using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(Engine))]
public class AppSettingEditor : Editor
{
    Engine setting;
    void OnEnable()
    {
        setting = (Engine)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(20);
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("刷新程序集以及宏定义"))
        {
            refAsmdefAndDefine();
        }
        EditorGUILayout.EndVertical();
    }

    void refAsmdefAndDefine()
    {
        string all = File.ReadAllText(Application.dataPath + "/Code/HotFix/HotFix.asmdef");

        int idx1 = all.IndexOf("includePlatforms");
        int idx11 = all.IndexOf("[", idx1 + 1);
        int idx22 = all.IndexOf("]", idx1);
        StringBuilder str = new StringBuilder();
        if (setting.Runtime == CodeRuntime.Assembly
            || setting.Runtime == CodeRuntime.ILRuntime)
        {
            str.Append(all.Substring(0, idx11 + 1));
            str.Append("\"Editor\"");
            str.Append(all.Substring(idx22, all.Length - idx22));
        }
        else
            str.Append(all.Remove(idx11 + 1, idx22 - (idx11 + 1)));

        File.WriteAllText(Application.dataPath + "/Code/HotFix/HotFix.asmdef", str.ToString());

        BuildTargetGroup[] groups = new BuildTargetGroup[]
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.Android,
            BuildTargetGroup.iOS,
        };
        for (int i = 0; i < groups.Length; i++)
        {
            List<string> defs = PlayerSettings.GetScriptingDefineSymbolsForGroup(groups[i]).Split(';').ToList();

            if (setting.Runtime == CodeRuntime.ILRuntime)
            {
                if (!defs.Contains("ILRuntime"))
                    defs.Add("ILRuntime");
            }
            else
                defs.RemoveAll(t => t == "ILRuntime");

            if (setting.Runtime == CodeRuntime.Assembly)
            {
                if (!defs.Contains("Assembly"))
                    defs.Add("Assembly");
            }
            else
                defs.RemoveAll(t => t == "Assembly");

            if (setting.Debug)
            {
                if (!defs.Contains("DebugEnable"))
                    defs.Add("DebugEnable");
            }
            else
                defs.RemoveAll(t => t == "DebugEnable");

            StringBuilder str1 = new StringBuilder();
            for (int j = 0; j < defs.Count; j++)
            {
                if (j < defs.Count - 1)
                    str1.Append(defs[j] + ";");
                else
                    str1.Append(defs[j]);
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(groups[i], str1.ToString());
        }

        AssetDatabase.Refresh();
    }
}
