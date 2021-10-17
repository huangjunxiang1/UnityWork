using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Main;

public class Init
{
    public static void Main()
    {
        System.Threading.SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
        SysEvent.RigisterAllStaticListener();
        WObjectManager.Inst.init();

        TabM.Init(AssetLoad.LoadConfigBytes("TabM.bytes"));
        TabL.Init(AssetLoad.LoadConfigBytes("TabL.bytes"));
        LanguageS.Init(AssetLoad.LoadConfigBytes("Language.bytes"));
        LanguageS.LanguageType = SystemLanguage.Chinese;

        var ui = UIManager.Inst.Open<UILoding>(1);
        ui.OnDispose.Add(inLogin);
    }
    static void inLogin()
    {
        SceneManager.Inst.init();
        UIManager.Inst.Open<UILogin>();
    }
}
