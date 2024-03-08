using FairyGUI;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class GTextFieldPropertyBinding : UIPropertyBinding<GTextField, string>
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
class GLoaderPropertyBinding : UIPropertyBinding<GLoader, string>
{
    public GLoaderPropertyBinding(GLoader u) : base(u)
    {
    }

    protected override void View(string v)
    {
        _ = this.ui.SetTexture(v);
    }
}
class GLoader3DPropertyBinding : UIPropertyBinding<GLoader3D, string>
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
class GButtonPropertyBinding : UIPropertyBinding<GButton, bool>
{
    public GButtonPropertyBinding(GButton u) : base(u)
    {
    }

    protected override void View(bool v)
    {
        this.ui.selected = v;
    }
}