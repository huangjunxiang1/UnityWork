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
        DisplayObject o = FairyGUI.Stage.inst.HitTest(position, true);
        GObject g = GRoot.inst.DisplayObjectToGObject(o);
        while (g != null)
        {
            if (g == GRoot.inst) return true;
            g = g.parent;
        }
        return false;
    }
    public static async void SetTexture(this RawImage ri, string texPath)
    {
        if (!ri) return;
        Texture tex = await AssetLoad.LoadAsync<Texture>(texPath);
        AssetLoad.AddTextureRef(ri.gameObject, tex);
    }
    public static string GetFGUIItemUrl(string name)
    {
        return UIPackage.GetItemURL("ResPkg", name);
    }
}
