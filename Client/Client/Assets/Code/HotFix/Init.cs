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
        Application.targetFrameRate = 60;
        FairyGUI.UIConfig.defaultFont = "Impact";
        DG.Tweening.DOTween.Init();

        System.Threading.SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);
        SysEvent.RigisterAllStaticListener();

        GameSetting.Languege = SystemLanguage.Chinese;
        GameSetting.UIModel = UIModel.FGUI;

        TabM.Init(new DBuffer((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabM.bytes")).bytes));
        TabL.Init(new DBuffer((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabL.bytes")).bytes));
        LanguageS.Load((int)SystemLanguage.Chinese, new DBuffer((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/Language_cn.bytes")).bytes));
        LanguageS.Load((int)SystemLanguage.English, new DBuffer((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/Language_en.bytes")).bytes));

        WRoot.Inst.Init();
        SceneMgr.Inst.Init();

        //≥ı ºªØUI≈‰÷√
        await UIS.Init();
        await UIS.OpenAsync<FUIGlobal>();

        await SceneMgr.Inst.InLoginScene();
    }
}
