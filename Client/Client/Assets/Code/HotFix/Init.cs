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
            //�����̲���debugģʽ loger�ᱻ���� �����ȸ�debugģʽ����loger����
            if (!ConstDefM.Debug && ConstDefL.Debug)
                Loger.Error("�����̲���debugģʽ �ȸ���debugģʽ");
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

        //��ʼ������ϵͳ
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
