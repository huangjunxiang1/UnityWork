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

public class GameStart : MonoBehaviour
{
    public CodeRuntime Runtime;
    public bool Debug;

    // Start is called before the first frame update
    void Start()
    {
        _ = ThreadSynchronizationContext.Instance;
        DontDestroyOnLoad(this.gameObject);

        AppSetting.Runtime = Runtime;
        AppSetting.Debug = Debug;

        EnterGame();
    }
    void EnterGame()
    {
        Type[] a1 = typeof(Types).Assembly.GetTypes();
        Type[] a2 = typeof(TabM).Assembly.GetTypes();
        Type[] all = new Type[a1.Length + a2.Length];
        a1.CopyTo(all, 0);
        a2.CopyTo(all, a1.Length);

        if (AppSetting.Runtime == CodeRuntime.Native)
        {
#if Assembly
            Loger.Error("当前Runtime宏定义不正确");
#else
            Type[] a3 = typeof(Init).Assembly.GetTypes();
            Types.InitTypes(all, a3);
            Init.Main();
#endif
        }
        else if (AppSetting.Runtime == CodeRuntime.Assembly)
        {
#if !Assembly
            Loger.Error("当前Runtime宏定义不正确");
#else
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

            Type[] a3 = asm.GetTypes();

            Types.InitTypes(all, a3);

            asm.GetType("Init").GetMethod("Main").Invoke(null, null);
#endif
        }
    }
    private void OnApplicationQuit()
    {
        GameM.Event?.RunEvent(new EC_QuitGame());
    }
}