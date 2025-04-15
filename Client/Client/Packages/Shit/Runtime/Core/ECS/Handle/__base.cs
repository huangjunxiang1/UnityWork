using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal abstract class __SystemHandle
{
    public virtual void _handle_AwakeOrDispose(SComponent c) { }
    public virtual ComponentFilter Filter(SObject o, bool addToQueue = false) => null;
    public virtual void Add(Delegate o) { }
    public virtual void _invoke_update() { }
    public virtual void Invoke(ComponentFilter filter) { }
    public virtual void _invoke_eventWatcher(object o, SObject obj) { }
    public virtual Type _get_firstType() => null;
    internal virtual object GetActions() => null;
}
internal abstract class __KVWatcher : __SystemHandle
{
    public abstract void Add(int key, Delegate d);
    public abstract void _invoke_kvWatcher(int key,ComponentFilter cf);
}

internal abstract class ComponentFilter
{
    internal int EnableCounter;
    internal bool Disposed;
    internal __SystemHandle system;
    internal bool dirty;
    internal SystemType type = SystemType.None;

    internal KVComponent kv;
    internal World world;

    public void Invoke() => system.Invoke(this);
    public void KvInvoke(int k) => ((__KVWatcher)system)._invoke_kvWatcher(k, this);
    public abstract void _addTo_HandlesList();
    public abstract void _addTo_kvHandlesList();
    public abstract void _handle_waitRemove(ICollection<SComponent> hash);
    public abstract SComponent GetFirstComponent();
}