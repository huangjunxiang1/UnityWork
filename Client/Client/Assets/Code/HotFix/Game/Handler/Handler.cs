using Cinemachine;
using Game;
using Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class Handler
{
    [Event]
    static void EC_OutScene(EC_OutScene e)
    {
        BaseCamera.Current?.Dispose();
    }
    [Event]
    static async void EC_InScene(EC_InScene e)
    {
        if (e.sceneId > 10000)
        {
            BaseCamera.SetCamera(new FreedomCamera(Camera.main.GetComponent<CinemachineBrain>()));
            GameObject cm = new("CMTarget");
            cm.transform.position = new Vector3(0, 0, 0);
            BaseCamera.Current.Init(cm);
            BaseCamera.Current.EnableCamera();
        }
        if (e.sceneId == 1)
            await GameL.UI.OpenAsync<FUILogin>();
    }
    [Event(1, Queue = true)]
    static async STask EC_GameInit(EC_GameInit e)
    {
        DG.Tweening.DOTween.Init();
        Setting.Languege = SystemLanguage.Chinese;
        Setting.UIModel = UIModel.FGUI;
        Game.ShareData.Init();
        Application.targetFrameRate = -1;

        DBuffer buffM_ST = new(new MemoryStream((SAsset.Load<TextAsset>($"Config/Tabs/{nameof(TabM_ST)}.bytes")).bytes));
        if (buffM_ST.ReadHeaderInfo())
            TabM_ST.Init(buffM_ST);

        DBuffer buffM = new(new MemoryStream((await SAsset.LoadAsync<TextAsset>($"Config/Tabs/{nameof(TabM)}.bytes")).bytes));
        if (buffM.ReadHeaderInfo())
            TabM.Init(buffM, ConstDefM.Debug);

        DBuffer buffL = new(new MemoryStream((await SAsset.LoadAsync<TextAsset>($"Config/Tabs/{nameof(TabL)}.bytes")).bytes));
        if (buffL.ReadHeaderInfo())
            TabL.Init(buffL, ConstDefM.Debug);
    }
    [Event]
    static void EC_QuitGame(EC_QuitGame e)
    {
        Game.ShareData.Dispose();
    }

}
