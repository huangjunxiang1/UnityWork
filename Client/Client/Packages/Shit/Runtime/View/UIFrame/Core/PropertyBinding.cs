using Core;
using Game;
using System;

public abstract class UIPropertyBinding<T, V>
{
    public UIPropertyBinding(T u)
    {
        this.ui = u;
    }

    Delegate getter;
    public T ui { get; }

    public void Binding<K>(Func<K, V> getter) where K : class
    {
        if (this.getter != null)
        {
            Loger.Error("getter Repeat");
            return;
        }
        void Event(K k) => this.View(getter(k));
        var act = new Action<K>(Event);
        this.getter = act;
        Client.World.Event.RigisteEvent(act);
        act(Client.Data.Get<K>());
    }
    protected virtual void View(V v) { }
    public virtual void Dispose()
    {
        if (getter != null)
            Client.World.Event.RemoveEvent(getter);
    }
}