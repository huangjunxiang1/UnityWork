using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WObjectL : TreeL<WObjectL>
{
    public WObjectL(WObjectM t) : base()
    {
        ObjectM = t;
    }
    public WObjectL(WObjectM t, long cid) : base(cid)
    {
        ObjectM = t;
    }
    public WObjectM ObjectM { get; private set; }

    public void Replace(WObjectM t)
    {
        ObjectM?.Dispose();
        ObjectM = t;
    }

    public override void Dispose()
    {
        ObjectM?.Dispose();
        base.Dispose();
    }
}
class WObjectL<T> : WObjectL where T : WObjectM
{
    public WObjectL(T t) : base(t)
    {

    }
    public WObjectL(T t, long cid) : base(t, cid)
    {

    }
    public new T ObjectM => base.ObjectM as T;
}