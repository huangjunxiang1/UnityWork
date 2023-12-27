using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Main;
using Game;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

public class Engine : MonoBehaviour
{
    public CodeRuntime Runtime;
    public bool Debug;

    // Start is called before the first frame update
    void Start()
    {
        _ = ThreadSynchronizationContext.Instance;
        DontDestroyOnLoad(this.gameObject);

        SAppSetting.Runtime = Runtime;
        SAppSetting.Debug = Debug;

        EnterGame();
    }

    // Update is called once per frame
    void Update()
    {
        ThreadSynchronizationContext.Instance.Update();
        STimer.Update();
    }

    void EnterGame()
    {
        Type[] a1 = typeof(Types).Assembly.GetTypes();
        Type[] a2 = typeof(STabM).Assembly.GetTypes();
        Type[] all = new Type[a1.Length + a2.Length];
        a1.CopyTo(all, 0);
        a2.CopyTo(all, a1.Length);

        if (SAppSetting.Runtime == CodeRuntime.Native)
        {
#if !ILRuntime && !Assembly
            Type[] a3 = typeof(Init).Assembly.GetTypes();

            Types.InitTypes(all, a3);

            Init.Main();
#else
            Loger.Error("当前Runtime宏定义不正确");
#endif
        }
        else if (SAppSetting.Runtime == CodeRuntime.Assembly)
        {
#if !Assembly
            Loger.Error("当前Runtime宏定义不正确");
#endif
#if ENABLE_IL2CPP
            Loger.Error("IL2CPP模式无法运行");
#endif
            Assembly asm;
            if (SAppSetting.Debug)
            {
                byte[] dll = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll");
                byte[] pdb = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.pdb");
                asm = Assembly.Load(dll, pdb);
            }
            else
            {
                byte[] dll = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll");
                asm = Assembly.Load(dll);
            }

            Type[] a3 = asm.GetTypes();

            Types.InitTypes(all, a3);

            asm.GetType("Init").GetMethod("Main").Invoke(null, null);
        }
        else if (SAppSetting.Runtime == CodeRuntime.ILRuntime)
        {
#if !ILRuntime
            Loger.Error("当前Runtime宏定义不正确");
#endif
            ILRuntime.Runtime.Enviorment.AppDomain app = new();
            if (SAppSetting.Debug)
            {
                System.IO.MemoryStream dll = new(System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll"));
                System.IO.MemoryStream pdb = new(System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.pdb"));
                app.LoadAssembly(dll, pdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
            }
            else
            {
                System.IO.MemoryStream dll = new(System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll"));
                app.LoadAssembly(dll);
            }
            ILRuntimeBinding.Binding(app);

            Type[] a3 = app.LoadedTypes.Values.Select(t => t.ReflectionType).ToArray();

            Types.InitTypes(all, a3);

            app.Invoke("Init", "Main", null, null);
        }

    }
    private void OnApplicationQuit()
    {
        SGameM.Event?.RunEvent(new EC_QuitGame());
    }
}