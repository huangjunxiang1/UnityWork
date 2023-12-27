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
    static void EC_InScene(EC_InScene e)
    {
        if (e.sceneId > 10000)
        {
            BaseCamera.SetCamera(new FreedomCamera(Camera.main.GetComponent<CinemachineBrain>()));
            GameObject cm = new("CMTarget");
            cm.transform.position = new Vector3(0, 0, 0);
            BaseCamera.Current.Init(cm);
            BaseCamera.Current.EnableCamera();
        }
    }
    [Event(1, Queue = true)]
    static async STask EC_HotFixInit(EC_HotFixInit e)
    {
        SSetting.Languege = SystemLanguage.Chinese;
        SSetting.UIModel = UIModel.FGUI;
        Game.ECSSingle.Init();
        Application.targetFrameRate = -1;

        DBuffer buffM_ST = new(new MemoryStream((SAsset.Load<TextAsset>("Config/Tabs/STabM_ST.bytes")).bytes));
        if (buffM_ST.ReadHeaderInfo())
            STabM_ST.Init(buffM_ST);

        DBuffer buffM = new(new MemoryStream((await SAsset.LoadAsync<TextAsset>("Config/Tabs/STabM.bytes")).bytes));
        if (buffM.ReadHeaderInfo())
            STabM.Init(buffM, SConstDefM.Debug);

        DBuffer buffL = new(new MemoryStream((await SAsset.LoadAsync<TextAsset>("Config/Tabs/STabL.bytes")).bytes));
        if (buffL.ReadHeaderInfo())
            STabL.Init(buffL, SConstDefM.Debug);
    }
    [Event]
    static void EC_QuitGame(EC_QuitGame e)
    {
        Game.ECSSingle.Dispose();
    }

}
