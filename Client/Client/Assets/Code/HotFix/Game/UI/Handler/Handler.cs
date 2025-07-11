using Core;
using Event;
using FairyGUI;
using Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using YooAsset;

static class Handler
{
    [Event(-100, Queue = true)]
    static async STask Init(EC_GameStart e)
    {
        YooAssets.Initialize();
        var loader = (YooassetLoader)SAsset.Loader;
        YooPkg.res = YooAssets.TryGetPackage("Res") ?? YooAssets.CreatePackage("Res");
        YooPkg.raw = YooAssets.TryGetPackage("Raw") ?? YooAssets.CreatePackage("Raw");
        loader.SetDefaultPackage(YooPkg.res);

        await YooPkg.LoadAsync(APPConfig.Inst.EPlayMode);

        await Resources.UnloadUnusedAssets().AsTask();

        DG.Tweening.DOTween.Init();
        SettingL.Languege = SystemLanguage.Chinese;
        Application.targetFrameRate = -1;

        G_Connecting g_Connecting = null;
        UIGlobalConfig.LoadingViewHandle += view =>
        {
            if (view)
            {
                g_Connecting = G_Connecting.Create();
                g_Connecting.sortingOrder = int.MaxValue - 2;
                GRoot.inst.AddChild(g_Connecting);
                g_Connecting.MakeFullScreen();
                g_Connecting.AddRelation(GRoot.inst, RelationType.Size);
            }
            else
            {
                g_Connecting.Dispose();
                g_Connecting = null;
            }
        };

        DBuffer buffM_ST = new(new MemoryStream(YooPkg.LoadRaw($"raw_{nameof(TabM_ST)}")));
        if (buffM_ST.ReadHeaderInfo())
            TabM_ST.Init(buffM_ST);

        DBuffer buffM = new(new MemoryStream(YooPkg.LoadRaw($"raw_{nameof(TabM)}")));
        if (buffM.ReadHeaderInfo())
            TabM.Init(buffM, ConstDefCore.Debug);

        DBuffer buffL = new(new MemoryStream(YooPkg.LoadRaw($"raw_{nameof(TabL)}")));
        if (buffL.ReadHeaderInfo())
            TabL.Init(buffL, ConstDefCore.Debug);
        await YooPkg.raw.UnloadUnusedAssetsAsync().AsTask();
    }

    [Event]
    static void EC_GameStart(EC_GameStart e)
    {
        {
            bool showExit = false;
            var input = new ESCInput();
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
        if (e.sceneId == 1)
            Client.Data.Clear();
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
        try { Server.Close(); } catch (System.Exception ex) { throw ex; }
        try { Client.Close(); } catch (System.Exception ex) { throw ex; }
        TabM_ST.Tab.Data.Dispose();
        Game.ShareData.Dispose();
    }
    [Event]
    static void EC_AcceptedMessage(EC_AcceptedMessage e)
    {
        if (!string.IsNullOrEmpty(e.message.error))
            Box.Tips(e.message.error);
    }
}
