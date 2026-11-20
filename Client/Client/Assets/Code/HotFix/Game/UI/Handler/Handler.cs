using Event;
using FairyGUI;
using System.IO;
using UnityEngine;

static class Handler
{
    [Event(-100, Queue = true)]
    static async STask Init(EC_GameStart e)
    {
        await Resources.UnloadUnusedAssets().AsTask();

        DG.Tweening.DOTween.Init();
        SettingL.LanguageType = SystemLanguage.Chinese;
        Application.targetFrameRate = -1;
        SSetting.ViewSetting.LoadingUrl = G_Connecting.URL;
        GRoot.inst.SetContentScaleFactor(1334, 750);

        //FairyGUI.UIConfig.defaultFont = "UIFont";
        //TMPFont font = new() { };
        //font.name = "UIFont";
        //font.fontAsset = await Game.Loader.LoadAsync<TMPro.TMP_FontAsset>("UI_UIFont");
        //FontManager.RegisterFont(font);

        TabL.Init(new cfg.Tables(s => new Luban.ByteBuf(Pkg.LoadRaw($"raw_bin_{s}"))));
        TabM.Init(new Native.Tables(s => new Luban.ByteBuf(Pkg.LoadRaw($"raw_Native_{s}"))));
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
                    Box.Op_YesOrNo("退出游戏".ToLanx(), "是否退出游戏?".ToLanx(), "确定".ToLanx(), "取消".ToLanx(), () =>
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
        try { Game.Close(); } catch (System.Exception ex) { throw ex; }
        TabM.Dispose();
    }
    [Event]
    static void EC_AcceptedMessage(EC_AcceptedMessage e)
    {
        if (!string.IsNullOrEmpty(e.message.error))
            Box.Tips(e.message.error);
    }
}
