using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

class TextPropertyBinding : UIPropertyBinding<Text, string>
{
    public TextPropertyBinding(Text u) : base(u)
    {
        record = u.text;
        format = record != null && record.Contains("{0}");
    }
    string record;
    bool format;

    protected override void View(string v)
    {
        v ??= string.Empty;
        ui.text = format ? string.Format(record, v) : v;
    }
}
class TextMeshProUGUIPropertyBinding : UIPropertyBinding<TMPro.TextMeshProUGUI, string>
{
    public TextMeshProUGUIPropertyBinding(TMPro.TextMeshProUGUI u) : base(u)
    {
        record = u.text;
        format = record != null && record.Contains("{0}");
    }
    string record;
    bool format;

    protected override void View(string v)
    {
        v ??= string.Empty;
        ui.text = format ? string.Format(record, v) : v;
    }
}
class ImagePropertyBinding : UIPropertyBinding<Image, string>
{
    public ImagePropertyBinding(Image u) : base(u)
    {
    }

    protected override void View(string v)
    {
        ui.sprite = v.ToUUIResUrl();
    }
}
class RawImagePropertyBinding : UIPropertyBinding<RawImage, string>
{
    public RawImagePropertyBinding(RawImage u) : base(u)
    {
    }

    protected override void View(string v)
    {
        _ = ui.SetTexture(v);
    }
}
class TogglePropertyBinding : UIPropertyBinding<Toggle, bool>
{
    public TogglePropertyBinding(Toggle u) : base(u)
    {
    }

    protected override void View(bool v)
    {
        this.ui.SetIsOnWithoutNotify(v);
    }
}
