using System.IO;
using Event;
using Game;
using UnityEngine;

static partial class SettingL
{
    static bool isFirst = true;
    public static void loadLocationText()
    {
        DBuffer buff = new(new MemoryStream(Pkg.LoadRaw($"raw_Language_{SSetting.ViewSetting.LanguageType}")));

        if (buff.ReadHeaderInfo())
            LanguageUtil.Load((int)SSetting.ViewSetting.LanguageType, buff, SSetting.CoreSetting.Debug);

        if (!isFirst || SSetting.ViewSetting.LanguageType != SystemLanguage.Chinese)
        {
            var txt = Pkg.LoadRawText($"raw_Language_UIText_{SSetting.ViewSetting.LanguageType}");
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
