using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using Game;
using Main;
using UnityEngine;

static class GameSetting
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

                UIS.UGUIRoot.gameObject.SetActive(value == UIModel.UGUI);
                UIS.UGUICamera.gameObject.SetActive(value == UIModel.UGUI);

                GRoot.inst.visible = value == UIModel.FGUI;
                StageCamera.main.gameObject.SetActive(value == UIModel.FGUI);
            }
        }
    }

    public static SystemLanguage Languege
    {
        get { return LanguageS.LanguageType; }
        set { LanguageS.LanguageType = value; }
    }
}
