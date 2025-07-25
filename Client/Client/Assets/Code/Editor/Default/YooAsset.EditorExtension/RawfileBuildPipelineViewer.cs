using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using YooAsset.Editor;

class Ex
{
    [MenuItem("Tools/剔除重复资源")]
    static void cull_res()
    {
        var dirs = Directory.GetDirectories($"{Application.dataPath}/../Bundles/");
        foreach (var dir in dirs)
        {
            foreach (var dir2 in Directory.GetDirectories(dir))
            {
                var ds = Directory.GetDirectories(dir2).Select(t => new DirectoryInfo(t)).Where(t => t.Name.Contains('.')).ToList();
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

    [BuildPipelineAttribute(nameof(EBuildPipeline.RawFileBuildPipeline))]
    internal class R1 : RawfileBuildpipelineViewer
    {
        protected override string GetDefaultPackageVersion() => getVer(this.BuildTarget,this.PackageName);
    }
    [BuildPipelineAttribute(nameof(EBuildPipeline.ScriptableBuildPipeline))]
    internal class S1 : ScriptableBuildPipelineViewer
    {
        protected override string GetDefaultPackageVersion() => getVer(this.BuildTarget, this.PackageName);
    }

    static string getVer(BuildTarget BuildTarget,string PackageName)
    {
        var output = $"{AssetBundleBuilderHelper.GetDefaultBuildOutputRoot()}/{BuildTarget}/{PackageName}";
        var min = new Version("1.0.0");
        if (!Directory.Exists(output))
            return min.ToString();
        var dirs = Directory.GetDirectories(output);
        var max = min;
        for (int i = 0; i < dirs.Length; i++)
        {
            try
            {
                var v = new Version(new DirectoryInfo(dirs[i]).Name);
                if (max < v)
                    max = v;
            }
            catch (Exception)
            {
                continue;
            }
        }
        return $"{max.Major}.{max.Minor}.{max.Build + 1}";
    }
}