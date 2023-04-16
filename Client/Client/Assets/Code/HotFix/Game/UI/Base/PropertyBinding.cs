using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class PropertyBinding<U, V>
{
    public PropertyBinding(U u)
    {
        this.ui = u;
    }

    V _v;
    Action _event;

    public U ui { get; }
    public V value
    {
        get => _v;
        set
        {
            if (value.Equals(_v))
                return;

            _v = value;
            _event?.Invoke();
        }
    }

    public void Binding(Action action)
    {
        _event += action;
    }
    public void CallEvent()
    {
        _event?.Invoke();
    }
}
