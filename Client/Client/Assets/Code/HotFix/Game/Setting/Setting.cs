using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using Game;
using Main;
using UnityEngine;

namespace Game
{
    partial class SettingL
    {
        UIModel _uiModel = UIModel.FGUI;
        public UIModel UIModel
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

        public SystemLanguage Languege
        {
            get { return LanguageS.LanguageType; }
            set { LanguageS.LanguageType = value; }
        }
    }
}
