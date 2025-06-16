using HybridCLR.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

static class Other
{

    [MenuItem("Shit/热重载配置表")]
    static void ReloadConfig()
    {
        if (!Application.isPlaying) return;

        {
            var buff = new DBuffer(new MemoryStream(File.ReadAllBytes(Application.dataPath + $"/Res/Config/raw/Tabs/{nameof(TabM)}.bytes")));
            if (!buff.ReadHeaderInfo())
                throw new System.Exception("数据错误");
            TabM.Init(buff, ConstDefCore.Debug);
        }
        {
            var buff = new DBuffer(new MemoryStream(File.ReadAllBytes(Application.dataPath + $"/Res/Config/raw/Tabs/{nameof(TabL)}.bytes")));
            if (!buff.ReadHeaderInfo())
                throw new System.Exception("数据错误");
            TabL.Init(buff, ConstDefCore.Debug);
        }
        {
            var buff = new DBuffer(new MemoryStream(File.ReadAllBytes(Application.dataPath + $"/Res/Config/raw/Tabs/Language_{LanguageUtil.LanguageType}.bytes")));
            if (!buff.ReadHeaderInfo())
                throw new System.Exception("数据错误");
            LanguageUtil.Load((int)LanguageUtil.LanguageType, buff, ConstDefCore.Debug);
        }
        EditorUtility.DisplayDialog("完成", "重载完成", "确定");
    }


    [MenuItem("Shit/AOT_Copy")]
    static void copyAotDll()
    {
        string srcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(EditorUserBuildSettings.activeBuildTarget);
        var dstDir = $"{Application.dataPath}/Resources/AOT";
        Directory.Delete(dstDir, true);
        if (!Directory.Exists(dstDir))
            Directory.CreateDirectory(dstDir);
        for (int i = 0; i < AOTGenericReferences.PatchedAOTAssemblyList.Count; i++)
        {
            File.Copy($"{srcDir}/{AOTGenericReferences.PatchedAOTAssemblyList[i]}", $"{dstDir}/{AOTGenericReferences.PatchedAOTAssemblyList[i]}.bytes");
        }
        AssetDatabase.Refresh();
    }
}
