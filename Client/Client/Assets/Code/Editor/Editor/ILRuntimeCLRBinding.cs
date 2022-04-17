#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEditorInternal;
using System.IO;
using System.Linq;

[System.Reflection.Obfuscation(Exclude = true)]
public class ILRuntimeCLRBinding
{
    [MenuItem("ILRuntime/通过自动分析热更DLL生成CLR绑定")]
    static void GenerateCLRBindingByAnalysis()
    {
        //用新的分析热更dll调用引用来生成绑定代码
        ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        using (System.IO.FileStream fs = new System.IO.FileStream(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll", System.IO.FileMode.Open, System.IO.FileAccess.Read))
        {
            domain.LoadAssembly(fs);

            ILRuntimeBinding.Binding(domain);
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, Application.dataPath + "/../Packages/ILRuntimeBinding/MethodBinding");
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("ILRuntime/合并相同DelegateBinding")]
    static void MergeSameDelegateBinding()
    {
        string mainPath = Application.dataPath + "/../Packages/ILRuntimeBinding/DelegateBinding/DelegateBinding.cs";
        foreach (var item in Directory.GetFiles(Application.dataPath + "/../Packages/ILRuntimeBinding/DelegateBinding/", "GenerateDelegateBinding_*.cs"))
        {
            List<string> main = File.ReadAllLines(mainPath).ToList();
            int m1 = main.FindIndex(t => t.Contains("xx1Start"));

            List<string> fs = File.ReadAllLines(item).ToList();
            {
                int idx1 = fs.FindIndex(t => t.Contains("xx1Start"));
                int idx2 = fs.FindIndex(t => t.Contains("xx1End"));
                for (int i = idx1 + 1; i < idx2; i++)
                {
                    if (main.Contains(fs[i]))
                        continue;
                    main.Insert(m1 + 1, fs[i]);
                }
                fs.RemoveRange(idx1 + 1, idx2 - idx1 - 1);
            }
            int m2 = main.FindIndex(t => t.Contains("xx2Start"));
            {
                int idx1 = fs.FindIndex(t => t.Contains("xx2Start"));
                int idx2 = fs.FindIndex(t => t.Contains("xx2End"));
                for (int i = idx1 + 1; i < idx2; i += 7)
                {
                    if (main.Contains(fs[i + 0]))
                        continue;
                    main.Insert(m2 + 1, fs[i + 6]);
                    main.Insert(m2 + 1, fs[i + 5]);
                    main.Insert(m2 + 1, fs[i + 4]);
                    main.Insert(m2 + 1, fs[i + 3]);
                    main.Insert(m2 + 1, fs[i + 2]);
                    main.Insert(m2 + 1, fs[i + 1]);
                    main.Insert(m2 + 1, fs[i + 0]);
                }
                fs.RemoveRange(idx1 + 1, idx2 - idx1 - 1);
            }
            File.WriteAllLines(item, fs);
            File.WriteAllLines(mainPath, main);
        }
        AssetDatabase.Refresh();
    }
}
#endif
