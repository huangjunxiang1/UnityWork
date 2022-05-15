using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;

class ManagerL<T> : EntityL where T : ManagerL<T>, new()
{
    static T _inst;
    public static T Inst
    {
        get { return _inst ??= new(); }
    }

    public virtual bool DisposeOnChangeScene => true;

    public virtual void Init()
    {

    }
    public override void Dispose()
    {
        base.Dispose();
        _inst = null;
    }

    [Event((int)EIDM.OutScene)]
    void exitScene()
    {
        if (!DisposeOnChangeScene)
            return;
        this.Dispose();
    }
}