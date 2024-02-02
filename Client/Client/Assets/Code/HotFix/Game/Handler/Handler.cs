using Cinemachine;
using FairyGUI;
using Game;
using Main;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

static class Handler
{
    [Event(-2, Queue = true)]
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
    static void EC_GameStart(EC_GameStart e)
    {
        if (!Application.isEditor && AppSetting.Debug)
        {
            G_LogReporter log = G_LogReporter.Create();
            log.ui.size = new Vector2(100, 120);
            GRoot.inst.AddChild(log.ui);
            log.ui.xy = new Vector2(0, (GRoot.inst.size.y - log.ui.size.y) / 2);
            log.ui.sortingOrder = int.MaxValue - 10;
            log.ui.onClick.Add(() => AppSetting.ShowReporter = !AppSetting.ShowReporter);
        }
        {
            bool showExit = false;
            var input = new ESCInput();
            input.esc.Enable();
            input.esconEsc.started += e =>
            {
                if (showExit)
                    return;
                if (e.ReadValueAsButton())
                {
                    showExit = true;
                    Box.Op_YesOrNo("退出游戏", "是否退出游戏?", "确定", "取消", () =>
                    {
                        UnityEngine.Application.Quit();
                        showExit = false;
                    },
                    () => showExit = false);
                }
            };
        }
    }
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
    [Event]
    static void EC_QuitGame(EC_QuitGame e)
    {
        Game.ShareData.Dispose();
    }
}
