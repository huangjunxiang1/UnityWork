using UnityEngine;
using Main;
using System.IO;
using Game;

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

        DG.Tweening.DOTween.Init();
        Timer.RigisterStaticTimer();
        GameM.Init();
        GameL.Init();
        GameM.Event.RunEvent(new EC_HotFixInit());
        await GameL.UI.Init();

        GameL.Setting.Languege = SystemLanguage.Chinese;
        GameL.Setting.UIModel = UIModel.FGUI;

        await LoadConfig();
        await GameL.UI.OpenAsync<FUIGlobal>();
        await GameL.Scene.InLoginScene();
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
            Game.ECSSingle.LoadTabs(buffM_ST);
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
    }
}
