using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Main;
using FairyGUI;
using System.Threading.Tasks;
using System;

public class Init
{
    public async static void Main()
    {
        if (ConstDefM.isILRuntime)
        {
            //主工程不是debug模式 loger会被剪裁 导致热更debug模式访问loger报错
            if (!ConstDefM.Debug && ConstDefL.Debug)
                Loger.Error("主工程不是debug模式 热更是debug模式");
        }
        Application.targetFrameRate = 60;
        FairyGUI.UIConfig.defaultFont = "Impact";
        DG.Tweening.DOTween.Init();
        System.Threading.SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

        GameM.Init();
        GameL.Init();
        await GameL.UI.Init();

        GameL.Setting.Languege = SystemLanguage.Chinese;
        GameL.Setting.UIModel = UIModel.FGUI;

        TabM.Init(new DBuffer((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabM.bytes")).bytes));
        TabL.Init(new DBuffer((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabL.bytes")).bytes));
        LanguageS.Load((int)SystemLanguage.Chinese, new DBuffer((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/Language_cn.bytes")).bytes));
        LanguageS.Load((int)SystemLanguage.English, new DBuffer((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/Language_en.bytes")).bytes));

        //初始化各个系统
        GameM.Event.ExecuteEvent((int)EventIDM.Init);

        await GameL.UI.OpenAsync<FUIGlobal>();
        await GameL.Scene.InLoginScene();
    }

    [Event((int)EventIDM.QuitGame)]
    static void Quit()
    {
        if (Application.isEditor)
        {
            GameL.Close();
            GameM.Close();
        }
    }
}
