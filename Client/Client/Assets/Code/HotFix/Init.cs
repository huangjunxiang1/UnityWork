using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using Main;
using FairyGUI;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Runtime.CompilerServices;

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
        Application.targetFrameRate = -1;
        FairyGUI.UIConfig.defaultFont = "Impact";
        DG.Tweening.DOTween.Init();
        System.Threading.SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

        GameM.Init();
        GameL.Init();
        await GameL.UI.Init();

        GameL.Setting.Languege = SystemLanguage.Chinese;
        GameL.Setting.UIModel = UIModel.FGUI;

        await LoadConfig();
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
        ECSSingle.GetSingle<TabM_ST>().Dispose();
    }
    static async TaskAwaiter LoadConfig()
    {
        DBuffer buffM = new(new MemoryStream((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabM.bytes")).bytes));
        buffM.Compress = false;
        if (buffM.Readint() != 20220702)
            Loger.Error("不是TabM数据");
        else
        {
            buffM.Compress = buffM.Readbool();
            TabM.Init(buffM, ConstDefM.Debug);
        }

        DBuffer buffM_ST = new(new MemoryStream((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabM_ST.bytes")).bytes));
        buffM_ST.Compress = false;
        if (buffM_ST.Readint() != 20220702)
            Loger.Error("不是TabM数据");
        else
        {
            buffM_ST.Compress = buffM_ST.Readbool();
            var st = ECSSingle.GetSingle<TabM_ST>();
            st.Init(buffM_ST);
            ECSSingle.SetSingle(st);
        }

        DBuffer buffL = new(new MemoryStream((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabL.bytes")).bytes));
        buffL.Compress = false;
        if (buffL.Readint() != 20220702)
            Loger.Error("不是TabL数据");
        else
        {
            buffL.Compress = buffL.Readbool();
            TabL.Init(buffL, ConstDefM.Debug);
        }

        DBuffer buff_cn = new(new MemoryStream((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/Language_cn.bytes")).bytes));
        buff_cn.Compress = false;
        if (buff_cn.Readint() != 20220702)
            Loger.Error("不是Language_cn数据");
        else
        {
            buff_cn.Compress = buff_cn.Readbool();
            LanguageS.Load((int)SystemLanguage.Chinese, buff_cn, ConstDefM.Debug);
        }

        DBuffer buff_en = new(new MemoryStream((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/Language_en.bytes")).bytes));
        buff_en.Compress = false;
        if (buff_en.Readint() != 20220702)
            Loger.Error("不是Language_en数据");
        else
        {
            buff_en.Compress = buff_en.Readbool();
            LanguageS.Load((int)SystemLanguage.English, buff_en, ConstDefM.Debug);
        }
    }
}
