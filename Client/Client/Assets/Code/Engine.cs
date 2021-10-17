using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Main;

public class Engine : MonoBehaviour
{

    public bool Ilruntime;
    public bool Debug;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        AppSetting.ILRunTime = Ilruntime;
        AppSetting.Debug = Debug;

        EnterGame();
    }

    // Update is called once per frame
    void Update()
    {
        ThreadSynchronizationContext.Instance.Update();
        SysNet.Update();
        Timer.Update();
    }

    public static void EnterGame()
    {
        var a1 = typeof(TypesCache).Assembly.GetTypes();

#if ILRuntime
        ILRuntime.Runtime.Enviorment.AppDomain app = new ILRuntime.Runtime.Enviorment.AppDomain();
        if (AppSetting.Debug)
        {
            System.IO.MemoryStream dll = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll"));
            System.IO.MemoryStream pdb = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.pdb"));
            app.LoadAssembly(dll, pdb, new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
        }
        else
        {
            System.IO.MemoryStream dll = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll"));
            app.LoadAssembly(dll);
        }
        ILRuntimeInit.Init(app);

        var a2 = app.LoadedTypes.Values.Select(t => t.ReflectionType).ToArray();
#else
        var a2 = typeof(Init).Assembly.GetTypes();
#endif

        Type[] ts = new Type[a1.Length + a2.Length];
        a1.CopyTo(ts, 0);
        a2.CopyTo(ts, a1.Length);
        TypesCache.InitTypes(ts);

#if ILRuntime
        app.Invoke("Init", "Main", null, null);
#else
        Init.Main();
#endif
    }
    private void OnApplicationQuit()
    {
        SysEvent.ExcuteEvent((int)EventIDM.QuitGame);
    }
}
