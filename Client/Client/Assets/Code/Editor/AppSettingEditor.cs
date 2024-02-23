using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build;
using Game;
using System.IO;

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

            if (setting.Runtime == CodeRuntime.Assembly)
            {
                ao.includePlatforms = new List<string>();
                ao.includePlatforms.Add("Editor");
            }
            else
                ao.includePlatforms = new List<string>();

            ao.Save(path);
        }

        NamedBuildTarget[] groups = new NamedBuildTarget[]
        {
            NamedBuildTarget.Standalone,
            NamedBuildTarget.Android,
            NamedBuildTarget.iOS,
        };
        for (int i = 0; i < groups.Length; i++)
        {
            List<string> defs = PlayerSettings.GetScriptingDefineSymbols(groups[i]).Split(';').ToList();

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

            PlayerSettings.SetScriptingDefineSymbols(groups[i], str1.ToString());
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/MyTool/热重载配置表")]
    static void ReloadConfig()
    {
        if (!Application.isPlaying) return;
        TabM.Init(new DBuffer(new MemoryStream(File.ReadAllBytes(Application.dataPath + $"/Res/Config/Tabs/{nameof(TabM)}.bytes"))), ConstDefM.Debug);
        TabL.Init(new DBuffer(new MemoryStream(File.ReadAllBytes(Application.dataPath + $"/Res/Config/Tabs/{nameof(TabL)}.bytes"))), ConstDefM.Debug);
        EditorUtility.DisplayDialog("完成", "重载完成", "确定");
    }
}
