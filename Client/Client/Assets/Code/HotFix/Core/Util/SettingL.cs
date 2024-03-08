using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Event;
using FairyGUI;
using Game;
using Main;
using UnityEngine;

static partial class SettingL
{
    static UIModel _uiModel = UIModel.FGUI;
    public static UIModel UIModel
    {
        get { return _uiModel; }
        set
        {
            if (_uiModel != value)
            {
                _uiModel = value;

                GameL.UI.UGUIRoot.gameObject.SetActive(value == UIModel.UGUI);
                GameL.UI.UGUICamera.gameObject.SetActive(value == UIModel.UGUI);

                GRoot.inst.visible = value == UIModel.FGUI;
                StageCamera.main.gameObject.SetActive(value == UIModel.FGUI);
            }
        }
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
            LanguageUtil.Load((int)LanguageUtil.LanguageType, buff, ConstDefM.Debug);
        GameM.Event.RunEvent(new EC_LanguageChange());
    }
}
