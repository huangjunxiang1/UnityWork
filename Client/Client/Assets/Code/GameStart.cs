using UnityEngine;
using Game;
using System.Threading;
using Core;

public class GameStart : MonoBehaviour
{
    public CodeRuntime Runtime;
    public bool Debug;

    // Start is called before the first frame update
    void Start()
    {
        var tsc = ThreadSynchronizationContext.GetOrCreate(Thread.CurrentThread.ManagedThreadId);
        ThreadSynchronizationContext.SetMainThread(tsc);
        Loger.__get__log += o => ThreadSynchronizationContext.MainThread.Post(() => UnityEngine.Debug.Log(o));
        Loger.__get__warning += o => ThreadSynchronizationContext.MainThread.Post(() => UnityEngine.Debug.LogWarning(o));
        Loger.__get__error += o => ThreadSynchronizationContext.MainThread.Post(() => UnityEngine.Debug.LogError(o));

        DontDestroyOnLoad(this.gameObject);

        AppSetting.Debug = Debug;
        AppSetting.Runtime = Runtime;
        GameObject.FindFirstObjectByType<IngameDebugConsole.DebugLogManager>()?.gameObject.SetActive(Debug && !Application.isEditor);

        EnterGame();
    }
    void EnterGame()
    {
        if (AppSetting.Runtime == CodeRuntime.Native)
        {
#if Assembly
            Loger.Error("当前Runtime宏定义不正确");
#else
            Program.Main();
#endif
        }
        else if (AppSetting.Runtime == CodeRuntime.Assembly)
        {
#if !Assembly
            Loger.Error("当前Runtime宏定义不正确");
#else
            System.Reflection.Assembly asm;
            if (AppSetting.Debug)
            {
                byte[] dll = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/Game.HotFix.dll");
                byte[] pdb = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/Game.HotFix.pdb");
                asm = System.Reflection.Assembly.Load(dll, pdb);
            }
            else
            {
                byte[] dll = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/Game.HotFix.dll");
                asm = System.Reflection.Assembly.Load(dll);
            }
            asm.GetType("Program").GetMethod("Main").Invoke(null, null);
#endif
        }
    }
}