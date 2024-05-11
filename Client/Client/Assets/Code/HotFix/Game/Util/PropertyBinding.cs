using FairyGUI;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

public class GTextFieldPropertyBinding : UIPropertyBinding<GTextField, string>
{
    public GTextFieldPropertyBinding(GTextField u) : base(u)
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
public class GLoaderPropertyBinding : UIPropertyBinding<GLoader, string>
{
    public GLoaderPropertyBinding(GLoader u) : base(u)
    {
    }

    protected override void View(string v)
    {
        this.ui.url = v.ToFUIResUrl();
    }
}
public class GLoader3DPropertyBinding : UIPropertyBinding<GLoader3D, string>
{
    public GLoader3DPropertyBinding(GLoader3D u) : base(u)
    {
    }

    protected override async void View(string v)
    {
        var go = await SAsset.LoadGameObjectAsync(v, ReleaseMode.Destroy);
        if (this.ui.wrapTarget)
            SAsset.Release(this.ui.wrapTarget);
        this.ui.SetWrapTarget(go, false, 0, 0);
    }
    public override void Dispose()
    {
        base.Dispose();
        if (this.ui.wrapTarget)
            SAsset.Release(this.ui.wrapTarget);
    }
}
public class GButtonPropertyBinding : UIPropertyBinding<GButton, bool>
{
    public GButtonPropertyBinding(GButton u) : base(u)
    {
    }

    protected override void View(bool v)
    {
        this.ui.selected = v;
    }
}


public class TextPropertyBinding : UIPropertyBinding<Text, string>
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
public class TextMeshProUGUIPropertyBinding : UIPropertyBinding<TMPro.TextMeshProUGUI, string>
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
public class ImagePropertyBinding : UIPropertyBinding<UnityEngine.UI.Image, string>
{
    public ImagePropertyBinding(UnityEngine.UI.Image u) : base(u)
    {
    }

    protected override void View(string v)
    {
        ui.sprite = v.ToUUIResUrl();
    }
}
public class RawImagePropertyBinding : UIPropertyBinding<RawImage, string>
{
    public RawImagePropertyBinding(RawImage u) : base(u)
    {
    }

    protected override void View(string v)
    {
        _ = ui.SetTexture(v);
    }
}
public class TogglePropertyBinding : UIPropertyBinding<Toggle, bool>
{
    public TogglePropertyBinding(Toggle u) : base(u)
    {
    }

    protected override void View(bool v)
    {
        this.ui.SetIsOnWithoutNotify(v);
    }
}