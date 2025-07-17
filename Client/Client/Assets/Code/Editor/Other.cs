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

    [MenuItem("Tools/热重载配置表")]
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


    [MenuItem("Tools/AOT_Copy")]
    static void copyAotDll()
    {
        string srcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(EditorUserBuildSettings.activeBuildTarget);
        var dstDir = $"{Application.dataPath}/Resources/AOT";
        if (!Directory.Exists(dstDir))
            Directory.CreateDirectory(dstDir);
        else
        {
            foreach (var item in Directory.GetFiles(dstDir))
                File.Delete(item);
        }
        for (int i = 0; i < AOTGenericReferences.PatchedAOTAssemblyList.Count; i++)
        {
            File.Copy($"{srcDir}/{AOTGenericReferences.PatchedAOTAssemblyList[i]}", $"{dstDir}/{AOTGenericReferences.PatchedAOTAssemblyList[i]}.bytes");
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/Code_Copy")]
    static void copyCode()
    {
        var dstDir = $"{Application.dataPath}/Res/Config/raw/Code";
        if (!Directory.Exists(dstDir))
            Directory.CreateDirectory(dstDir);
        var bs = File.ReadAllBytes($"{Application.dataPath}/../Library/ScriptAssemblies/Game.HotFix.dll");
        File.WriteAllBytes($"{dstDir}/code.bytes", bs);
        AssetDatabase.Refresh();
    }

    [MenuItem("Tools/剔除重复资源")]
    static void cull_res()
    {
        var dirs = Directory.GetDirectories($"{Application.dataPath}/../Bundles/");
        foreach (var dir in dirs)
        {
            foreach (var dir2 in Directory.GetDirectories(dir))
            {
                var ds = Directory.GetDirectories(dir2).Select(t => new DirectoryInfo(t)).Where(t => t.Name.Contains('-')).ToList();
                var src = ds.LastOrDefault();
                var cull = src.GetFiles().Select(t => t.Name);
                //File.Delete($"{src}/PackageManifest_{AssetBundleBuilderSettingData.Setting.BuildPackage}.version");
                HashSet<string> olds = new(100000);
                for (int i = 0; i < ds.Count - 1; i++)
                {
                    var target = ds[i];
                    foreach (var item in target.GetFiles())
                        olds.Add(item.Name);
                }
                foreach (var item in src.GetFiles())
                {
                    if (item.Name.EndsWith(".version"))
                        continue;
                    if (item.Name.EndsWith(".json") || item.Name.EndsWith(".xml") || item.Name.EndsWith(".report"))
                        item.Delete();
                    if (olds.Contains(item.Name))
                        item.Delete();
                }
            }
        }
        Debug.Log("剔除完毕");
    }
}