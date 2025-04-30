using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

//begin#1
//internal partial class SystemHandler<?[T]+, ?> : __SystemHandle ?where [T] : SComponent+ ?
//{
//    internal List<Action<?[T]+, ?>> sys = new(1);
//    internal Queue<ComponentFilter<?[T]+, ?>> cfq = ObjectPool.Get<Queue<ComponentFilter<?[T]+, ?>>>();
//
//    public override ComponentFilter Filter(SObject o, bool addToQueue = false)
//    {
//?        if (!o.TryGetComponent<[T]>(out var [c])) return null;+\r?
//        var v = new ComponentFilter<?[T]+, ?>() { system = this, world = o.World, ?[t] = [c]+, ? };
//?        if (![c].Enable) v.EnableCounter %= 1;+\r?
//        if (addToQueue) cfq.Enqueue(v);
//        return v;
//    }
//
//    public override void Add(Delegate d) => sys.Add((Action<?[T]+, ?>)d);
//    public override void _invoke_update()
//    {
//        var tmp = cfq;
//        cfq = ObjectPool.Get<Queue<ComponentFilter<?[T]+, ?>>>();
//        while (tmp.TryDequeue(out var cf))
//        {
//            if (cf.EnableCounter == 0)
//            {
//                for (int i = 0; i < sys.Count; i++)
//                {
//                    if (cf.Disposed) continue;
//                    try { sys[i].Invoke(?cf.[t]+, ?); }
//                    catch (Exception e) { Loger.Error(e); }
//                }
//            }
//            cfq.Enqueue(cf);
//        }
//        ObjectPool.Return(tmp);
//    }
//    public override void Invoke(ComponentFilter filter)
//    {
//        var cf = (ComponentFilter<?[T]+, ?>)filter;
//        for (int i = 0; i < sys.Count; i++)
//        {
//            if (cf.Disposed) continue;
//            try { sys[i].Invoke(?cf.[t]+, ?); }
//            catch (Exception e) { Loger.Error(e); }
//        }
//    }
//    public override Type _get_firstType() => typeof(T);
//    internal override object GetActions() => sys;
//}
//end

internal partial class SystemHandler<T>
{
    public override void _handle_AwakeOrDispose(SComponent c)
    {
        T t = (T)c;
        for (int i = 0; i < sys.Count; i++)
        {
            try { sys[i].Invoke(t); }
            catch (Exception e) { Loger.Error(e); }
            if (t.Disposed) break;
        }
    }
}