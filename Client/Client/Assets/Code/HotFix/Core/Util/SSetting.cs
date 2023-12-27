using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using Game;
using Main;
using UnityEngine;

namespace Game
{
    static partial class SSetting
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

                    SGameL.UI.UGUIRoot.gameObject.SetActive(value == UIModel.UGUI);
                    SGameL.UI.UGUICamera.gameObject.SetActive(value == UIModel.UGUI);

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
            DBuffer buff = new(new MemoryStream((await SAsset.LoadAsync<TextAsset>($"Config/Tabs/SLanguage_{LanguageUtil.LanguageType}.bytes")).bytes));
            if (LanguageUtil.LanguageType != t)
            {
                buff.Dispose();
                return;
            }

            if (buff.ReadHeaderInfo())
                LanguageUtil.Load((int)LanguageUtil.LanguageType, buff, SConstDefM.Debug);
            SGameM.Event.RunEvent(new EC_LanguageChange());
        }
    }
}
