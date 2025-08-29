﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UIGlobalConfig
{
    public static Func<bool> isTouchUI;
    /// <summary>
    /// Loading...显示
    /// </summary>
    public static string LoadingUrl;
    public static int LoadingViewDelayBeginTimeMs = 1000;
    public static int LoadingViewTimeOutTimeMs = 10000;
    static HashSet<object> loadingTokens = new();
#if FairyGUI
    static FairyGUI.GComponent LoadingUI;
#endif

    static UnityEngine.EventSystems.EventSystem eventSysCurrent;
    static int EnableCounter;
    public static void EnableUIInput(bool enable)
    {
        if (!enable) EnableCounter++;
        else EnableCounter--;

#if UGUI
        if (!eventSysCurrent)
            eventSysCurrent = UnityEngine.EventSystems.EventSystem.current;
        if (eventSysCurrent)
            eventSysCurrent.enabled = EnableCounter <= 0;
#endif
#if FairyGUI
        FairyGUI.GRoot.inst.touchable = EnableCounter <= 0;
#endif
    }

    public static void LoadingView(object token, bool view)
    {
        if (token == null)
        {
            Loger.Error("key is null");
            return;
        }
        if (view)
        {
            if (loadingTokens.Contains(token))
                return;
            loadingTokens.Add(token);
            if (loadingTokens.Count == 1)
            {
                if (!string.IsNullOrEmpty(LoadingUrl))
                {
#if FairyGUI
                    LoadingUI = FairyGUI.UIPackage.CreateObjectFromURL(LoadingUrl).asCom;
                    LoadingUI.sortingOrder = int.MaxValue - 2;
                    FairyGUI.GRoot.inst.AddChild(LoadingUI);
                    LoadingUI.MakeFullScreen();
                    LoadingUI.AddRelation(FairyGUI.GRoot.inst, FairyGUI.RelationType.Size);
#endif
                }
            }
        }
        else
        {
            if (!loadingTokens.Contains(token))
                return;
            UIGlobalConfig.loadingTokens.Remove(token);
            if (UIGlobalConfig.loadingTokens.Count == 0)
            {
#if FairyGUI
                LoadingUI?.Dispose();
                LoadingUI = null;
#endif
            }
        }
    }
}
