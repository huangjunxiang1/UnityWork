using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Event;
using FairyGUI;
using Game;
using UnityEngine;

static partial class SettingL
{
    public static string Account
    {
        get => PlayerPrefs.GetString(nameof(Account));
        set => PlayerPrefs.SetString(nameof(Account), value);
    }
    public static string Password
    {
        get => PlayerPrefs.GetString(nameof(Password));
        set => PlayerPrefs.SetString(nameof(Password), value);
    }
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

    static async void loadLocationText()
    {
        var t = LanguageUtil.LanguageType;
        DBuffer buff = new(new MemoryStream((await SAsset.LoadAsync<TextAsset>($"Config/Tabs/Language_{LanguageUtil.LanguageType}.bytes")).bytes));
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
