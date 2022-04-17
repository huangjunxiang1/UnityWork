using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Main;
using Game;
using System.Threading.Tasks;
using System.Reflection;
using Unity.Jobs;
using Unity.Collections;

public class Engine : MonoBehaviour
{

    public CodeRuntime Runtime;
    public bool Debug;
    /* public ServiceType ServiceType;*/

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        AppSetting.Runtime = Runtime;
        AppSetting.Debug = Debug;
        /*AppSetting.ServiceType = ServiceType;*/

        EnterGame();
    }

    // Update is called once per frame
    void Update()
    {
        ThreadSynchronizationContext.Instance.Update();
        SysNet.Update();
        Timer.Update();
    }

    void EnterGame()
    {
        Type[] a1 = typeof(TypesCache).Assembly.GetTypes();

        if (AppSetting.Runtime == CodeRuntime.Native)
        {
#if !ILRuntime && !Assembly
            Type[] a2 = typeof(Init).Assembly.GetTypes();

            Type[] ts = new Type[a1.Length + a2.Length];
            a1.CopyTo(ts, 0);
            a2.CopyTo(ts, a1.Length);
            TypesCache.InitTypes(ts);

            Init.Main();
#else
            Loger.Error("当前Runtime宏定义不正确");
#endif
        }
        else if (AppSetting.Runtime == CodeRuntime.Assembly)
        {
#if !Assembly
            Loger.Error("当前Runtime宏定义不正确");
#endif
#if UNITY_IL2CPP
           Loger.Error("IL2CPP模式无法运行");
#endif
            Assembly asm;
            if (AppSetting.Debug)
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

            Type[] a2 = asm.GetTypes();

            Type[] ts = new Type[a1.Length + a2.Length];
            a1.CopyTo(ts, 0);
            a2.CopyTo(ts, a1.Length);
            TypesCache.InitTypes(ts);

            asm.GetType("Init").GetMethod("Main").Invoke(null, null);
        }
        else if (AppSetting.Runtime == CodeRuntime.ILRuntime)
        {
#if !ILRuntime
            Loger.Error("当前Runtime宏定义不正确");
#endif
            ILRuntime.Runtime.Enviorment.AppDomain app = new();
            if (AppSetting.Debug)
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

            Type[] a2 = app.LoadedTypes.Values.Select(t => t.ReflectionType).ToArray();

            Type[] ts = new Type[a1.Length + a2.Length];
            a1.CopyTo(ts, 0);
            a2.CopyTo(ts, a1.Length);
            TypesCache.InitTypes(ts);

            app.Invoke("Init", "Main", null, null);
        }

    }
    private void OnApplicationQuit()
    {
        SysEvent.ExecuteEvent((int)EIDM.QuitGame);
    }
}