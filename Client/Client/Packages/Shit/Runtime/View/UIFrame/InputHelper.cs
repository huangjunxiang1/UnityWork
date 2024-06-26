using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class InputHelper
{
    static UnityEngine.EventSystems.EventSystem eventSysCurrent;
    static int EnableCounter = 0;

    public static void EnableUIInput(bool enable)
    {
        if (!enable) EnableCounter++;
        else EnableCounter--;

#if UGUI
        if (!eventSysCurrent)
            eventSysCurrent = UnityEngine.EventSystems.EventSystem.current;
        if (eventSysCurrent)
            eventSysCurrent.enabled = EnableCounter <= 0;
#endif
#if FairyGUI
        FairyGUI.GRoot.inst.touchable = EnableCounter <= 0;
#endif
    }
}
