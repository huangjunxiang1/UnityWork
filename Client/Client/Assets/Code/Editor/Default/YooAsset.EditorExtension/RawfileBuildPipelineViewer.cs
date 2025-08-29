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
    static void cull_res()
    {
        var dirs = Directory.GetDirectories($"{Application.dataPath}/../Bundles/");
        foreach (var dir in dirs)
        {
            foreach (var dir2 in Directory.GetDirectories(dir))
            {
                var ds = Directory.GetDirectories(dir2).Select(t => new DirectoryInfo(t)).Where(t => t.Name.Contains('.')).ToList();
                ds.Sort((x, y) =>
                {
                    Version v1 = null;
                    Version v2 = null;
                    try { v1 = new(x.Name); }
                    catch (Exception) { }
                    try { v2 = new(y.Name); }
                    catch (Exception) { }
                    if (v1 < v2) return -1;
                    if (v1 > v2) return 1;
                    return 0;
                });
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
        EditorUtility.DisplayDialog("提示", "剔除完毕", "确定", "取消");
    }

    [BuildPipelineAttribute(nameof(EBuildPipeline.RawFileBuildPipeline))]
    internal class R1 : RawfileBuildpipelineViewer
    {
        protected override string GetDefaultPackageVersion() => getVer(this.BuildTarget,this.PackageName);
        public override void CreateView(VisualElement parent)
        {
            base.CreateView(parent);

            Button myButton = new Button();
            myButton.style.marginTop = 10;
            myButton.text = "剔除重复资源";
            myButton.style.height = 50;
            myButton.AddToClassList("my-button-style");
            myButton.clicked += cull_res;
            Root.Add(myButton);
        }
    }
    [BuildPipelineAttribute(nameof(EBuildPipeline.ScriptableBuildPipeline))]
    internal class S1 : ScriptableBuildPipelineViewer
    {
        protected override string GetDefaultPackageVersion() => getVer(this.BuildTarget, this.PackageName);
        public override void CreateView(VisualElement parent)
        {
            base.CreateView(parent);

            Button myButton = new Button();
            myButton.style.marginTop = 10;
            myButton.text = "剔除重复资源";
            myButton.style.height = 50;
            myButton.AddToClassList("my-button-style");
            myButton.clicked += cull_res;
            Root.Add(myButton);
        }
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