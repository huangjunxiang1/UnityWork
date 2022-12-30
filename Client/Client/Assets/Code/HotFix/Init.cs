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
        Application.targetFrameRate = -1;
        FairyGUI.UIConfig.defaultFont = "Impact";
        DG.Tweening.DOTween.Init();
        System.Threading.SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

        GameM.Init();
        GameL.Init();
        await GameL.UI.Init();

        GameL.Setting.Languege = SystemLanguage.Chinese;
        GameL.Setting.UIModel = UIModel.FGUI;

        DBytesBuffer buffM = new((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabM.bytes")).bytes);
        buffM.Compress = false;
        if (buffM.Readint() != 20220702)
            Loger.Error("����TabM����");
        else
        {
            buffM.Compress = buffM.Readbool();
            TabM.Init(buffM);
        }

        DBytesBuffer buffL = new((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabL.bytes")).bytes);
        buffL.Compress = false;
        if (buffL.Readint() != 20220702)
            Loger.Error("����TabL����");
        else
        {
            buffL.Compress = buffL.Readbool();
            TabL.Init(buffL);
        }

        DBytesBuffer buff_cn = new((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/Language_cn.bytes")).bytes);
        buff_cn.Compress = false;
        if (buff_cn.Readint() != 20220702)
            Loger.Error("����Language_cn����");
        else
        {
            buff_cn.Compress = buff_cn.Readbool();
            LanguageS.Load((int)SystemLanguage.Chinese, buff_cn);
        }

        DBytesBuffer buff_en = new((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/Language_en.bytes")).bytes);
        buff_en.Compress = false;
        if (buff_en.Readint() != 20220702)
            Loger.Error("����Language_en����");
        else
        {
            buff_en.Compress = buff_en.Readbool();
            LanguageS.Load((int)SystemLanguage.English, buff_en);
        }

        //��ʼ������ϵͳ
        GameM.Event.RunEvent((int)EventIDM.Init);

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
