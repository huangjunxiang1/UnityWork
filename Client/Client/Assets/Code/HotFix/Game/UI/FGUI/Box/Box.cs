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
    public static void Op_YesOrNo(string title, string text, string yes, string no, EventCallback0 onYes, EventCallback0 onNo = null)
    {
        var g = UIPackage.CreateObject("ComPkg", "Box_YesOrNo").asCom;
        GRoot.inst.AddChild(g);
        g.xy = new UnityEngine.Vector2(GRoot.inst.width - g.width, GRoot.inst.height - g.height) / 2f;
        g.sortingOrder = int.MaxValue - 1;
        g.fairyBatching = true;
        g.GetChild("title").text = title;
        g.GetChild("text").text = text;

        GButton btnYes = g.GetChild("_yes").asButton;
        btnYes.title = yes;
        btnYes.onClick.Add(onYes);

        GButton btnNo = g.GetChild("_no").asButton;
        btnNo.title = no;
        btnNo.onClick.Add(() =>
        {
            g.Dispose();
            onNo?.Invoke();
        });
    }
}
