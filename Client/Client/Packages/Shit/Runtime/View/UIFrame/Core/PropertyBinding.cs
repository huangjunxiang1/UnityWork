using Core;
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
        Game.Event.RigisteEvent(act);
        act(Game.Data.Get<K>());
    }
    protected virtual void View(V v) { }
    public virtual void Dispose()
    {
        if (getter != null)
            Game.Event.RemoveEvent(getter);
    }
}