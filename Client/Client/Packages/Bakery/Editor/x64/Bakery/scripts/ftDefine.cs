#if UNITY_EDITOR

// Disable 'obsolete' warnings
#pragma warning disable 0618

using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Build;

[InitializeOnLoad]
#if UNITY_2017_4_OR_NEWER
public class ftDefine : IActiveBuildTargetChanged
#else
public class ftDefine
#endif
{
    static void AddDefine()
    {
        var platform = EditorUserBuildSettings.selectedBuildTargetGroup;
        var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(platform);
        if (!defines.Contains("BAKERY_INCLUDED"))
        {
            if (defines.Length > 0) defines += ";";
            defines += "BAKERY_INCLUDED";
            if (!defines.Contains("BAKERY_NOREIMPORT"))
            {
                defines += ";BAKERY_NOREIMPORT";
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(platform, defines);
        }
    }

    static ftDefine()
    {
        AddDefine();
    }

#if UNITY_2017_4_OR_NEWER
    public int callbackOrder { get { return 0; } }
    public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
    {
        AddDefine();
    }
#endif
}

#endif
