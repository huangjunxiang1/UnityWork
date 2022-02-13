#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;

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

            //Crossbind Adapter is needed to generate the correct binding code
            ILRuntimeInit.Init(domain);
            ILRuntime.Runtime.CLRBinding.BindingCodeGenerator.GenerateBindingCode(domain, Application.dataPath + "/Code/Main/ILRuntimeHelper/Generated");
        }

        AssetDatabase.Refresh();
    }
}
#endif
