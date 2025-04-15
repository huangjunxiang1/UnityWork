using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//internal partial class EventWatcher<E, ?[T]+, ?> : __SystemHandle ?where [T] : SComponent+ ?
//{
//    internal List<Action<E, ?[T]+, ?>> sys = new(1);
//
//    public override void Add(Delegate d) => sys.Add((Action<E, ?[T]+, ?>)d);
//    public override void _invoke_eventWatcher(object o, SObject obj)
//    {
//?        if (!obj.TryGetComponent<[T]>(out var [c]) || ![c].Enable) return;+\r?
//#if UNITY_EDITOR
//        obj._EventWatcher.Add(this);
//#endif
//        for (int i = 0; i < sys.Count; i++)
//        {
//            try { sys[i].Invoke((E)o, ?[c]+, ?); }
//            catch (Exception e) { Loger.Error(e); }
//        }
//    }
//    public override Type _get_firstType() => typeof(T);
//    internal override object GetActions() => sys;
//}
//end