using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class ViewComponent : SComponent
{
    public sealed override bool Enable
    {
        get => base.Enable && this.Entity.View; 
        set
        {
            base.Enable = value;
            this.View(this.Enable);
        }
    }
    protected abstract void View(bool view);
    public override void SetChange()
    {
        base.SetChange();
        this.View(this.Enable);
    }
    public override void SetChangeFlag()
    {
        base.SetChangeFlag();
        this.View(this.Enable);
    }
}
