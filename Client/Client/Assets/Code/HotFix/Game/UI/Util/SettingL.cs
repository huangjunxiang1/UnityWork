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
}
