using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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