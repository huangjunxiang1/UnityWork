using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Main;
using FairyGUI;

public class Init
{
    public async static void Main()
    {
        DG.Tweening.DOTween.Init();

        System.Threading.SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
        SysEvent.RigisterAllStaticListener();

        GameSetting.Languege = SystemLanguage.Chinese;
        GameSetting.UIModel = UIModel.FGUI;
        
        TabM.Init((await AssetLoad.TextAssetLoader.LoadAsync("Config/TabM.bytes")).bytes);
        TabL.Init((await AssetLoad.TextAssetLoader.LoadAsync("Config/TabL.bytes")).bytes);
        LanguageS.Init((await AssetLoad.TextAssetLoader.LoadAsync("Config/Language.bytes")).bytes);

        WRoot.Inst.Init();
        SceneMgr.Inst.Init();
        WObjectManager.Inst.Init();

        //≥ı ºªØUI≈‰÷√
        await UIS.Init();

        await SceneMgr.Inst.InLoginScene();
    }
}
