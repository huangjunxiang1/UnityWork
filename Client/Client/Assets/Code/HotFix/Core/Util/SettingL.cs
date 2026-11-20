using Animancer;
using Event;
using FairyGUI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

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

    static SystemLanguage _languageType = SystemLanguage.Chinese;
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

    static void loadLocationText()
    {
        DBuffer buff = new(new MemoryStream(Pkg.LoadRaw($"raw_Lan_{SettingL.LanguageType}")));

        if (buff.ReadHeaderInfo())
        {
            LanguageUtil.Load((int)SettingL.LanguageType, buff, SSetting.CoreSetting.Debug);
            if (SettingL.LanguageType != SystemLanguage.Chinese)
            {
                var strings = new Dictionary<string, Dictionary<string, string>>();
                int len = buff.Readint();
                for (int idx = 0; idx < len; idx++)
                {
                    var key = buff.Readstring();
                    var text = buff.Readstring();
                    int i = key.IndexOf("-");
                    if (i == -1)
                        continue;

                    string key2 = key.Substring(0, i);
                    string key3 = key.Substring(i + 1);
                    Dictionary<string, string> col;
                    if (!strings.TryGetValue(key2, out col))
                    {
                        col = new Dictionary<string, string>();
                        strings[key2] = col;
                    }
                    col[key3] = text;
                }

                TranslationHelper.strings = strings;
            }
        }

        Game.Event.RunEvent(new EC_LanguageChange());
    }
}
