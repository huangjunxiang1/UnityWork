using System.IO;
using Event;
using Game;
using UnityEngine;

static partial class SettingL
{
    static bool isFirst = true;
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
        DBuffer buff = new(new MemoryStream(Pkg.LoadRaw($"raw_Language_{LanguageUtil.LanguageType}")));

        if (buff.ReadHeaderInfo())
            LanguageUtil.Load((int)LanguageUtil.LanguageType, buff, ConstDefCore.Debug);

        if (!isFirst || LanguageUtil.LanguageType != SystemLanguage.Chinese)
        {
            var txt = Pkg.LoadRawText($"raw_Language_UIText_{LanguageUtil.LanguageType}");
            if (!string.IsNullOrEmpty(txt))
            {
                FairyGUI.Utils.XML xml = new FairyGUI.Utils.XML(txt);
                FairyGUI.UIPackage.SetStringsSource(xml);
            }
        }
        isFirst = false;

        Client.World.Event.RunEvent(new EC_LanguageChange());
    }
}
