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
        var g = new G_Tips(UIPkg.ComPkg.CreateObject("Tips").asCom);
        GRoot.inst.AddChild(g.ui);
        g.ui.Center();
        g.ui.sortingOrder = int.MaxValue;
        g.ui.GetChild("title").text = s;
        g.ui.GetTransition("my").Play(g.ui.Dispose);
    }
    public static void Op_YesOrNo(string title, string text, string yes, string no, EventCallback0 onYes = null, EventCallback0 onNo = null)
    {
        var g = new G_Box_YesOrNo(UIPkg.ComPkg.CreateObject("Box_YesOrNo").asCom);
        GRoot.inst.AddChild(g.ui);
        g.ui.Center();
        g.ui.sortingOrder = int.MaxValue - 1;
        g.ui.fairyBatching = true;
        g._title.text = title;
        g._text.text = text;

        g._yes.title = yes;
        g._yes.onClick.Add(() =>
        {
            g.ui.Dispose();
            onYes?.Invoke();
        });

        g._no.title = no;
        g._no.onClick.Add(() =>
        {
            g.ui.Dispose();
            onNo?.Invoke();
        });
    }
}
