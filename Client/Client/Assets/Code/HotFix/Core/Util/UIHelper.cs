using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;
using FairyGUI;

static partial class UIHelper
{
    static UnityEngine.EventSystems.EventSystem eventSysCurrent;
    static int EnableCounter = 0;

    public static void EnableUIInput(bool enable)
    {
        if (!enable) EnableCounter++;
        else EnableCounter--;

        if (GameL.Setting.UIModel == UIModel.UGUI)
        {
            if (!eventSysCurrent)
                eventSysCurrent = UnityEngine.EventSystems.EventSystem.current;
            if (eventSysCurrent)
                eventSysCurrent.enabled = EnableCounter <= 0;
        }
        else
            GRoot.inst.touchable = EnableCounter <= 0;
    }

    public static bool IsOnTouchFUI()
    {
        GObject g = GRoot.inst.touchTarget;
        while (g != null)
        {
            if (g == GRoot.inst) return true;
            g = g.parent;
        }
        return false;
    }
    public static bool IsOnTouchFUI(Vector2 position)
    {
        position.y = Screen.height - position.y;
        DisplayObject o = Stage.inst.HitTest(position, true);
        GObject g = GRoot.inst.DisplayObjectToGObject(o);
        while (g != null)
        {
            if (g == GRoot.inst) return true;
            g = g.parent;
        }
        return false;
    }

    public static string GetFGUIItemUrl(string name)
    {
        PackageItem pi = UIPkg.ResPkg.GetItem(name);
        if (pi == null)
            return null;
        return $"{UIPackage.URL_PREFIX}{UIPkg.ResPkg.id}{pi.id}";
    }
}
