using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;
using FairyGUI;
using Game;

static partial class UIHelper
{
    static UnityEngine.EventSystems.EventSystem eventSysCurrent;
    static int EnableCounter = 0;

    public static void EnableUIInput(bool enable)
    {
        if (!enable) EnableCounter++;
        else EnableCounter--;

        if (SettingL.UIModel == UIModel.UGUI)
        {
            if (!eventSysCurrent)
                eventSysCurrent = UnityEngine.EventSystems.EventSystem.current;
            if (eventSysCurrent)
                eventSysCurrent.enabled = EnableCounter <= 0;
        }
        else
            GRoot.inst.touchable = EnableCounter <= 0;
    }
}
