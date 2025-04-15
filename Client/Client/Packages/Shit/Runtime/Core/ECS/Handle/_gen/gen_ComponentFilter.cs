using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

internal class ComponentFilter<T> : ComponentFilter where T : SComponent
{
    public T t;
    public override void _addTo_HandlesList()
    {
        t._Handles.Add(this);
    }
    public override void _addTo_kvHandlesList() 
    {
        if (!kv._Handles.Contains(this))
            kv._Handles.Add(this);
    }
    public override void _handle_waitRemove(ICollection<SComponent> hash)
    {
        hash.Add(t);
    }
    public override SComponent GetFirstComponent() => t;
}
internal class ComponentFilter<T, T2> : ComponentFilter where T : SComponent where T2 : SComponent
{
    public T t;
    public T2 t2;
    public override void _addTo_HandlesList()
    {
        t._Handles.Add(this);
        t2._Handles.Add(this);
    }
    public override void _addTo_kvHandlesList() 
    {
        if (!kv._Handles.Contains(this))
            kv._Handles.Add(this);
    }
    public override void _handle_waitRemove(ICollection<SComponent> hash)
    {
        hash.Add(t);
        hash.Add(t2);
    }
    public override SComponent GetFirstComponent() => t;
}
internal class ComponentFilter<T, T2, T3> : ComponentFilter where T : SComponent where T2 : SComponent where T3 : SComponent
{
    public T t;
    public T2 t2;
    public T3 t3;
    public override void _addTo_HandlesList()
    {
        t._Handles.Add(this);
        t2._Handles.Add(this);
        t3._Handles.Add(this);
    }
    public override void _addTo_kvHandlesList() 
    {
        if (!kv._Handles.Contains(this))
            kv._Handles.Add(this);
    }
    public override void _handle_waitRemove(ICollection<SComponent> hash)
    {
        hash.Add(t);
        hash.Add(t2);
        hash.Add(t3);
    }
    public override SComponent GetFirstComponent() => t;
}
internal class ComponentFilter<T, T2, T3, T4> : ComponentFilter where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent
{
    public T t;
    public T2 t2;
    public T3 t3;
    public T4 t4;
    public override void _addTo_HandlesList()
    {
        t._Handles.Add(this);
        t2._Handles.Add(this);
        t3._Handles.Add(this);
        t4._Handles.Add(this);
    }
    public override void _addTo_kvHandlesList() 
    {
        if (!kv._Handles.Contains(this))
            kv._Handles.Add(this);
    }
    public override void _handle_waitRemove(ICollection<SComponent> hash)
    {
        hash.Add(t);
        hash.Add(t2);
        hash.Add(t3);
        hash.Add(t4);
    }
    public override SComponent GetFirstComponent() => t;
}
internal class ComponentFilter<T, T2, T3, T4, T5> : ComponentFilter where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent
{
    public T t;
    public T2 t2;
    public T3 t3;
    public T4 t4;
    public T5 t5;
    public override void _addTo_HandlesList()
    {
        t._Handles.Add(this);
        t2._Handles.Add(this);
        t3._Handles.Add(this);
        t4._Handles.Add(this);
        t5._Handles.Add(this);
    }
    public override void _addTo_kvHandlesList() 
    {
        if (!kv._Handles.Contains(this))
            kv._Handles.Add(this);
    }
    public override void _handle_waitRemove(ICollection<SComponent> hash)
    {
        hash.Add(t);
        hash.Add(t2);
        hash.Add(t3);
        hash.Add(t4);
        hash.Add(t5);
    }
    public override SComponent GetFirstComponent() => t;
}
internal class ComponentFilter<T, T2, T3, T4, T5, T6> : ComponentFilter where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent
{
    public T t;
    public T2 t2;
    public T3 t3;
    public T4 t4;
    public T5 t5;
    public T6 t6;
    public override void _addTo_HandlesList()
    {
        t._Handles.Add(this);
        t2._Handles.Add(this);
        t3._Handles.Add(this);
        t4._Handles.Add(this);
        t5._Handles.Add(this);
        t6._Handles.Add(this);
    }
    public override void _addTo_kvHandlesList() 
    {
        if (!kv._Handles.Contains(this))
            kv._Handles.Add(this);
    }
    public override void _handle_waitRemove(ICollection<SComponent> hash)
    {
        hash.Add(t);
        hash.Add(t2);
        hash.Add(t3);
        hash.Add(t4);
        hash.Add(t5);
        hash.Add(t6);
    }
    public override SComponent GetFirstComponent() => t;
}
internal class ComponentFilter<T, T2, T3, T4, T5, T6, T7> : ComponentFilter where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent
{
    public T t;
    public T2 t2;
    public T3 t3;
    public T4 t4;
    public T5 t5;
    public T6 t6;
    public T7 t7;
    public override void _addTo_HandlesList()
    {
        t._Handles.Add(this);
        t2._Handles.Add(this);
        t3._Handles.Add(this);
        t4._Handles.Add(this);
        t5._Handles.Add(this);
        t6._Handles.Add(this);
        t7._Handles.Add(this);
    }
    public override void _addTo_kvHandlesList() 
    {
        if (!kv._Handles.Contains(this))
            kv._Handles.Add(this);
    }
    public override void _handle_waitRemove(ICollection<SComponent> hash)
    {
        hash.Add(t);
        hash.Add(t2);
        hash.Add(t3);
        hash.Add(t4);
        hash.Add(t5);
        hash.Add(t6);
        hash.Add(t7);
    }
    public override SComponent GetFirstComponent() => t;
}
internal class ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8> : ComponentFilter where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent
{
    public T t;
    public T2 t2;
    public T3 t3;
    public T4 t4;
    public T5 t5;
    public T6 t6;
    public T7 t7;
    public T8 t8;
    public override void _addTo_HandlesList()
    {
        t._Handles.Add(this);
        t2._Handles.Add(this);
        t3._Handles.Add(this);
        t4._Handles.Add(this);
        t5._Handles.Add(this);
        t6._Handles.Add(this);
        t7._Handles.Add(this);
        t8._Handles.Add(this);
    }
    public override void _addTo_kvHandlesList() 
    {
        if (!kv._Handles.Contains(this))
            kv._Handles.Add(this);
    }
    public override void _handle_waitRemove(ICollection<SComponent> hash)
    {
        hash.Add(t);
        hash.Add(t2);
        hash.Add(t3);
        hash.Add(t4);
        hash.Add(t5);
        hash.Add(t6);
        hash.Add(t7);
        hash.Add(t8);
    }
    public override SComponent GetFirstComponent() => t;
}
internal class ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9> : ComponentFilter where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent
{
    public T t;
    public T2 t2;
    public T3 t3;
    public T4 t4;
    public T5 t5;
    public T6 t6;
    public T7 t7;
    public T8 t8;
    public T9 t9;
    public override void _addTo_HandlesList()
    {
        t._Handles.Add(this);
        t2._Handles.Add(this);
        t3._Handles.Add(this);
        t4._Handles.Add(this);
        t5._Handles.Add(this);
        t6._Handles.Add(this);
        t7._Handles.Add(this);
        t8._Handles.Add(this);
        t9._Handles.Add(this);
    }
    public override void _addTo_kvHandlesList() 
    {
        if (!kv._Handles.Contains(this))
            kv._Handles.Add(this);
    }
    public override void _handle_waitRemove(ICollection<SComponent> hash)
    {
        hash.Add(t);
        hash.Add(t2);
        hash.Add(t3);
        hash.Add(t4);
        hash.Add(t5);
        hash.Add(t6);
        hash.Add(t7);
        hash.Add(t8);
        hash.Add(t9);
    }
    public override SComponent GetFirstComponent() => t;
}
internal class ComponentFilter<T, T2, T3, T4, T5, T6, T7, T8, T9, T10> : ComponentFilter where T : SComponent where T2 : SComponent where T3 : SComponent where T4 : SComponent where T5 : SComponent where T6 : SComponent where T7 : SComponent where T8 : SComponent where T9 : SComponent where T10 : SComponent
{
    public T t;
    public T2 t2;
    public T3 t3;
    public T4 t4;
    public T5 t5;
    public T6 t6;
    public T7 t7;
    public T8 t8;
    public T9 t9;
    public T10 t10;
    public override void _addTo_HandlesList()
    {
        t._Handles.Add(this);
        t2._Handles.Add(this);
        t3._Handles.Add(this);
        t4._Handles.Add(this);
        t5._Handles.Add(this);
        t6._Handles.Add(this);
        t7._Handles.Add(this);
        t8._Handles.Add(this);
        t9._Handles.Add(this);
        t10._Handles.Add(this);
    }
    public override void _addTo_kvHandlesList() 
    {
        if (!kv._Handles.Contains(this))
            kv._Handles.Add(this);
    }
    public override void _handle_waitRemove(ICollection<SComponent> hash)
    {
        hash.Add(t);
        hash.Add(t2);
        hash.Add(t3);
        hash.Add(t4);
        hash.Add(t5);
        hash.Add(t6);
        hash.Add(t7);
        hash.Add(t8);
        hash.Add(t9);
        hash.Add(t10);
    }
    public override SComponent GetFirstComponent() => t;
}
