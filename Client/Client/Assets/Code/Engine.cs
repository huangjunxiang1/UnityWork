using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Main;
using Game;
using System.Threading.Tasks;
using System.Reflection;

public class Engine : MonoBehaviour
{

    public CodeRuntime Runtime;
    public bool AssetBundle;
    public bool Debug;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        AppSetting.Runtime = Runtime;
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
            Loger.Error("��ǰRuntime�궨�岻��ȷ");
#endif
        }
        else if (AppSetting.Runtime == CodeRuntime.Assembly)
        {
#if !Assembly
            Loger.Error("��ǰRuntime�궨�岻��ȷ");
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
            Loger.Error("��ǰRuntime�궨�岻��ȷ");
#endif
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
        SysEvent.ExcuteEvent((int)EIDM.QuitGame);
    }
}