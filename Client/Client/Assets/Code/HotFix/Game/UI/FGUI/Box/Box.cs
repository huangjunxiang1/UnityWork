using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
static class Box
{
    public static void Tips(string s)
    {
        var g = UIPackage.CreateObject("ComPkg", "Tips").asCom;
        GRoot.inst.AddChild(g);
        g.xy = new UnityEngine.Vector2(GRoot.inst.width - g.width, GRoot.inst.height - g.height) / 2f;
        g.sortingOrder = int.MaxValue;
        g.GetChild("title").text = s;
        g.GetTransition("my").Play(g.Dispose);
    }
}
