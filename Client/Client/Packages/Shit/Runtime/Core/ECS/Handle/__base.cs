using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class __SystemHandle { }
public abstract class __AwakeHandle : __SystemHandle { }
public abstract class __DisposeHandle : __SystemHandle { }
public abstract class __EnableHandle : __SystemHandle { }
public abstract class __ChangeHandle : __SystemHandle
{
    internal bool Disposed;
    internal abstract void AddToRemoveWait();
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
    internal bool Disposed { get; private set; }
    internal virtual void Dispose() => this.Disposed = true;
    internal abstract void Invoke(int param);
}
public abstract class __Timer : __SystemHandle
{
    protected int count;
    protected float delay;
    protected TimerAttribute timer;
    protected bool dispposed = false;
    internal virtual bool Disposed { get; }

    internal abstract void Update();
    public virtual void Dispose() => this.dispposed = true;
}