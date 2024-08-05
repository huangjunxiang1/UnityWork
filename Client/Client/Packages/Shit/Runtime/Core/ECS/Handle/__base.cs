using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class __SystemHandle { }
public abstract class __InHandle : __SystemHandle { }
public abstract class __OutHandle : __SystemHandle
{
    internal abstract void Invoke(SObject o);
}
public abstract class __ChangeHandle : __SystemHandle
{
    internal abstract bool Disposed { get; }
    internal bool setInvokeWaiting = false;
    internal abstract void Dispose();
    internal abstract void Invoke();
}
public abstract class __UpdateHandle : __SystemHandle
{
    internal abstract bool Disposed { get; }
    internal abstract void Invoke();
}
public abstract class __EventWatcher : __SystemHandle { }
public abstract class __KVWatcher : __SystemHandle
{
    public long Old { get; internal set; }
    public long New { get; internal set; }
    internal bool Disposed { get; private set; }
    internal virtual void Dispose() => this.Disposed = true;
    internal abstract void Invoke(int type);
}
public abstract class __Timer : __SystemHandle
{
    protected SObject obj;
    internal virtual bool Disposed { get; }
}