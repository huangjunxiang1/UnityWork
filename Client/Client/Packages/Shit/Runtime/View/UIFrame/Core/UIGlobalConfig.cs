using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UIGlobalConfig
{
    /// <summary>
    /// Loading...显示
    /// </summary>
    public static Action<bool> LoadingViewHandle;
    public static int LoadingViewDelay1TimeMs = 1000;
    public static int LoadingViewDelay2TimeMs = 10000;
    static HashSet<object> loadingTokens = new();

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
    public static Func<FUIBase, string, GComponent> CreateUI;
    public static Func<FUIBase, string, STask<GComponent>> CreateUIAsync;
#endif
}
