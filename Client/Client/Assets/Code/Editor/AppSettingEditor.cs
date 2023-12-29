using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Compilation;

[CustomEditor(typeof(GameStart))]
public class AppSettingEditor : Editor
{
    GameStart setting;
    void OnEnable()
    {
        setting = (GameStart)target;
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
        {
            string path = Application.dataPath + "/Code/HotFix/HotFix.asmdef";
            AssmblyOpter ao = AssmblyOpter.Load(path);

            if (setting.Runtime == CodeRuntime.Assembly
                || setting.Runtime == CodeRuntime.ILRuntime)
            {
                ao.includePlatforms = new List<string>();
                ao.includePlatforms.Add("Editor");
            }
            else
                ao.includePlatforms = new List<string>();

            ao.Save(path);
        }

        {
            string path = Application.dataPath + "/../Packages/ILRuntimeBinding/ILRuntimeBinding.asmdef";
            AssmblyOpter ao = AssmblyOpter.Load(path);

            if (setting.Runtime == CodeRuntime.Assembly
                || setting.Runtime == CodeRuntime.ILRuntime)
            {
                ao.includePlatforms = new List<string>();
                ao.includePlatforms.Add("Editor");
            }
            else
                ao.includePlatforms = new List<string>();

            ao.Save(path);
        }


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
