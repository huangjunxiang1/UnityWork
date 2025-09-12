using System.IO;
using Event;
using Game;
using UnityEngine;

static partial class SettingL
{
    public static SystemLanguage Languege
    {
        get { return LanguageUtil.LanguageType; }
        set
        {
            if (LanguageUtil.LanguageType == value)
                return;
            LanguageUtil.LanguageType = value;
            loadLocationText();
        }
    }

    static void loadLocationText()
    {
        var t = LanguageUtil.LanguageType;
        DBuffer buff = new(new MemoryStream(Pkg.LoadRaw($"raw_Language_{LanguageUtil.LanguageType}")));
        if (LanguageUtil.LanguageType != t)
        {
            buff.Dispose();
            return;
        }

        if (buff.ReadHeaderInfo())
            LanguageUtil.Load((int)LanguageUtil.LanguageType, buff, ConstDefCore.Debug);
        Client.World.Event.RunEvent(new EC_LanguageChange());
    }
}
