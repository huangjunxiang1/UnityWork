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
    internal bool Disposed { get; private set; }
    internal bool Dispose() => this.Disposed = true;
    internal abstract void Invoke(CoreWorld world);
}
public abstract class __UpdateHandle : __SystemHandle
{
    internal abstract bool IsValid();
    internal abstract void Invoke(CoreWorld world);
}