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
            //�����̲���debugģʽ loger�ᱻ���� �����ȸ�debugģʽ����loger����
            if (!ConstDefM.Debug && ConstDefL.Debug)
                Loger.Error("�����̲���debugģʽ �ȸ���debugģʽ");
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
            Loger.Error("����TabM����");
        else
        {
            buffM.Compress = buffM.Readbool();
            TabM.Init(buffM, ConstDefM.Debug);
        }

        DBuffer buffM_ST = new(new MemoryStream((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabM_ST.bytes")).bytes));
        buffM_ST.Compress = false;
        if (buffM_ST.Readint() != 20220702)
            Loger.Error("����TabM����");
        else
        {
            buffM_ST.Compress = buffM_ST.Readbool();
            Game.ECSSingle.LoadTabs(buffM_ST);
        }

        DBuffer buffL = new(new MemoryStream((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabL.bytes")).bytes));
        buffL.Compress = false;
        if (buffL.Readint() != 20220702)
            Loger.Error("����TabL����");
        else
        {
            buffL.Compress = buffL.Readbool();
            TabL.Init(buffL, ConstDefM.Debug);
        }
    }
}
