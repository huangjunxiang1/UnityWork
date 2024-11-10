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
        var g = G_Tips.Create();
        GRoot.inst.AddChild(g);
        g.Center();
        g.sortingOrder = int.MaxValue;
        g.GetChild("title").text = s;
        g.GetTransition("my").Play(g.Dispose);
    }
    public static void Op_YesOrNo(string title, string text, string yes, string no, EventCallback0 onYes = null, EventCallback0 onNo = null)
    {
        var g = G_Box_YesOrNo.Create();
        GRoot.inst.AddChild(g);
        g.Center();
        g.sortingOrder = int.MaxValue - 1;
        g.fairyBatching = true;
        g._title.text = title;
        g._text.text = text;

        g._yes.title = yes;
        g._yes.onClick.Add(() =>
        {
            g.Dispose();
            onYes?.Invoke();
        });

        g._no.title = no;
        g._no.onClick.Add(() =>
        {
            g.Dispose();
            onNo?.Invoke();
        });
    }
}
