using System.IO;
using Event;
using Game;
using UnityEngine;

static partial class SettingL
{
    public static bool Debug
    {
        get
        {
#if DebugEnable
            return true;
#else 
            return false;
#endif
        }
    }

    static SystemLanguage _languageType = SystemLanguage.Unknown;
    public static SystemLanguage LanguageType
    {
        get => _languageType;
        set
        {
            if (_languageType != value)
            {
                _languageType = value;
                if (Application.isPlaying)
                    loadLocationText();
            }
        }
    }

    static bool isFirst = true;
    static void loadLocationText()
    {
        DBuffer buff = new(new MemoryStream(Pkg.LoadRaw($"raw_Language_{SettingL.LanguageType}")));

        if (buff.ReadHeaderInfo())
            LanguageUtil.Load((int)SettingL.LanguageType, buff, SSetting.CoreSetting.Debug);

        if (!isFirst || SettingL.LanguageType != SystemLanguage.Chinese)
        {
            var txt = Pkg.LoadRawText($"raw_Language_UIText_{SettingL.LanguageType}");
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
