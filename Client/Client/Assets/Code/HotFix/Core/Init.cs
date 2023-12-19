using UnityEngine;
using Main;
using System.IO;

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

        await LoadConfig();
        await GameL.UI.Init();

        GameL.Setting.Languege = SystemLanguage.Chinese;
        GameL.Setting.UIModel = UIModel.FGUI;

        await GameL.UI.OpenAsync<FUIGlobal>();
        await GameL.Scene.InLoginScene();
    }

    static async TaskAwaiter LoadConfig()
    {
        DBuffer buffM_ST = new(new MemoryStream((AssetLoad.Load<TextAsset>("Config/Tabs/TabM_ST.bytes")).bytes));
        if (buffM_ST.ReadHeaderInfo())
            TabM_ST.Init(buffM_ST);

        DBuffer buffM = new(new MemoryStream((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabM.bytes")).bytes));
        if(buffM.ReadHeaderInfo())
            TabM.Init(buffM, ConstDefM.Debug);

        DBuffer buffL = new(new MemoryStream((await AssetLoad.LoadAsync<TextAsset>("Config/Tabs/TabL.bytes")).bytes));
        if (buffL.ReadHeaderInfo())
            TabL.Init(buffL, ConstDefM.Debug);
    }
}
