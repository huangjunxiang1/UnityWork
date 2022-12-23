using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Main;
using FairyGUI;

static class UIHelper
{
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

    public static Vector2 WorldToFUI(Vector3 world)
    {
        Vector3 sp = Camera.main.WorldToScreenPoint(world);
        //原点位置转换
        sp.y = Screen.height - sp.y;
        return sp;
    }
    public static Vector2 WorldToGObject(Vector3 world, GObject g)
    {
        Vector2 p = WorldToFUI(world);
        p.x -= g.width / 2;
        p.y -= g.height / 2;
        return p;
    }

    public static async void SetTexture(this RawImage ri, string url)
    {
        if (!ri) return;
        Texture tex = await AssetLoad.LoadAsync<Texture>(url);
        AssetLoad.AddTextureRef(ri.gameObject, tex);
    }
    public static string GetFGUIItemUrl(string name)
    {
        PackageItem pi = UIPkg.ResPkg.GetItem(name);
        if (pi == null)
            return null;
        return $"{UIPackage.URL_PREFIX}{UIPkg.ResPkg.id}{pi.id}";
    }
}
