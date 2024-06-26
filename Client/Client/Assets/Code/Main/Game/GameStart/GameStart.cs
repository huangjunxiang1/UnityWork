using UnityEngine;
using Game;
using System.Threading;
using Core;
using System;
using System.Reflection;

public class GameStart : MonoBehaviour
{
    public CodeRuntime Runtime;
    public bool Debug;

    // Start is called before the first frame update
    void Start()
    {
        var tsc = ThreadSynchronizationContext.GetOrCreate(System.Environment.CurrentManagedThreadId);
        ThreadSynchronizationContext.SetMainThread(tsc);

        DontDestroyOnLoad(this.gameObject);

        AppSetting.Debug = Debug;
        AppSetting.Runtime = Runtime;
        GameObject.FindFirstObjectByType<IngameDebugConsole.DebugLogManager>()?.gameObject.SetActive(Debug && !Application.isEditor);

        EnterGame();
    }
    void EnterGame()
    {
        Assembly assembly = null;
        if (AppSetting.Runtime == CodeRuntime.Native)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.GetName().Name == "Game.HotFix")
                {
                    assembly = asm;
                    break;
                }
            }
            if (assembly == null)
            {
                Loger.Error("not find Assembly Game.HotFix");
                return;
            }
        }
        else if (AppSetting.Runtime == CodeRuntime.Assembly)
        {
            //load dll from Res Hotfix in runtime
            if (AppSetting.Debug)
            {
                byte[] dll = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/Game.HotFix.dll");
                byte[] pdb = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/Game.HotFix.pdb");
                assembly = System.Reflection.Assembly.Load(dll, pdb);
            }
            else
            {
                byte[] dll = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/Game.HotFix.dll");
                assembly = System.Reflection.Assembly.Load(dll);
            }
        }
        else
        {
            Loger.Error("Unknown Runtime=" + AppSetting.Runtime);
            return;
        }
        assembly
                .GetType("Program")
                .GetMethod("Main", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Invoke(null, null);
    }
}