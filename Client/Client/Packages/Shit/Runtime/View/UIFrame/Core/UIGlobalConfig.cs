using System;
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
    public static Action<bool> LoadingViewHandle;
    public static int LoadingViewDelay1TimeMs = 1000;
    public static int LoadingViewDelay2TimeMs = 10000;
    static HashSet<object> loadingTokens = new();

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
                UIGlobalConfig.LoadingViewHandle?.Invoke(true);
        }
        else
        {
            if (!loadingTokens.Contains(token))
                return;
            UIGlobalConfig.loadingTokens.Remove(token);
            if (UIGlobalConfig.loadingTokens.Count == 0)
                UIGlobalConfig.LoadingViewHandle?.Invoke(false);
        }
    }

#if FairyGUI
    public static Func<FUIBase, string, FairyGUI.GComponent> CreateUI;
    public static Func<FUIBase, string, STask<FairyGUI.GComponent>> CreateUIAsync;
#endif
}
