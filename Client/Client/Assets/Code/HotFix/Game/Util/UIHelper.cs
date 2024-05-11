using FairyGUI;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

static class UIHelper
{
    static UnityEngine.U2D.SpriteAtlas Items = SAsset.Load<UnityEngine.U2D.SpriteAtlas>("UI/UUI/Atlas/Items.spriteatlasv2");
    static UnityEngine.U2D.SpriteAtlas UIAtlas = SAsset.Load<UnityEngine.U2D.SpriteAtlas>("UI/UUI/Atlas/UIAtlas.spriteatlasv2");
    public static Sprite ToUUIItemUrl(this string name) => Items.GetSprite(name);
    public static Sprite ToUUIResUrl(this string name) => UIAtlas.GetSprite(name);

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

    public static string ToFUIItemUrl(this string name)
    {
        PackageItem pi = UIPkg.Items.GetItemByName(name);
        if (pi == null)
            return null;
        return $"{UIPackage.URL_PREFIX}{UIPkg.Items.id}{pi.id}";
    }
    public static string ToFUIResUrl(this string name)
    {
        PackageItem pi = UIPkg.ResPkg.GetItemByName(name);
        if (pi == null)
            return null;
        return $"{UIPackage.URL_PREFIX}{UIPkg.ResPkg.id}{pi.id}";
    }
}