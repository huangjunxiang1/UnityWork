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
        await Resources.UnloadUnusedAssets().AsTask();

        DG.Tweening.DOTween.Init();
        SettingL.Languege = SystemLanguage.Chinese;
        Application.targetFrameRate = -1;

        UIGlobalConfig.LoadingUrl = G_Connecting.URL;
        /*FairyGUI.UIConfig.defaultFont = "UIFont";
        TMPFont font = new() { };
        font.name = "UIFont";
        font.fontAsset = await SAsset.LoadAsync<TMPro.TMP_FontAsset>("UI_UIFont");
        FontManager.RegisterFont(font);*/

        DBuffer buffM_ST = new(new MemoryStream(Pkg.LoadRaw($"raw_{nameof(TabM_ST)}")));
        if (buffM_ST.ReadHeaderInfo())
            TabM_ST.Init(buffM_ST);

        DBuffer buffM = new(new MemoryStream(Pkg.LoadRaw($"raw_{nameof(TabM)}")));
        if (buffM.ReadHeaderInfo())
            TabM.Init(buffM, ConstDefCore.Debug);

        DBuffer buffL = new(new MemoryStream(Pkg.LoadRaw($"raw_{nameof(TabL)}")));
        if (buffL.ReadHeaderInfo())
            TabL.Init(buffL, ConstDefCore.Debug);
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
