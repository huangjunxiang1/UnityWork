using UnityEngine;
using Game;
using System.Threading;
using Main;

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
                byte[] dll = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll");
                byte[] pdb = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.pdb");
                asm = System.Reflection.Assembly.Load(dll, pdb);
            }
            else
            {
                byte[] dll = System.IO.File.ReadAllBytes(Application.dataPath + "/../Library/ScriptAssemblies/HotFix.dll");
                asm = System.Reflection.Assembly.Load(dll);
            }
            asm.GetType("Program").GetMethod("Main").Invoke(null, null);
#endif
        }
    }
}