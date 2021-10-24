using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Main;

public class Init
{
    public static void Main()
    {
        DG.Tweening.DOTween.Init();

        System.Threading.SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
        SysEvent.RigisterAllStaticListener();

        TabM.Init(AssetLoad.LoadConfigBytes("TabM.bytes"));
        TabL.Init(AssetLoad.LoadConfigBytes("TabL.bytes"));
        LanguageS.Init(AssetLoad.LoadConfigBytes("Language.bytes"));

        GameSetting.Languege = SystemLanguage.Chinese;
        GameSetting.UIModel = UIModel.FGUI;

        WRoot.Inst.Init();
        SceneMgr.Inst.Init();
        WObjectManager.Inst.Init();

        _ = SceneMgr.Inst.InLoginScene();
    }
}
