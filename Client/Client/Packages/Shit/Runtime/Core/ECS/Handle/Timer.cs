using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//begin#1
//public class Timer<?[T]+, ?> : __Timer ?where [T] : SComponent+ ?
//{
//    Action<Timer<?[T]+, ?>> action;
//    static List<TimerItem> timers;
//    internal override bool Disposed => this.dispposed || ?this.[t].Disposed+ || ?;
//    public Timer(?[T] [t]+, ?) { ?this.[t] = [t];+ ? }
//?    public [T] [t] { get; }+\r?
//
//    internal static void SetTimerList(List<TimerItem> lst) => timers = lst;
//    internal static void TryCreateHandle(SObject o)
//    {
//        for (int i = 0; i < timers.Count; i++)
//        {
//            var ti = timers[i];
//            if (ti.attribute.delay < 0 || ti.attribute.count == 0 || ti.attribute.count < -1)
//                Loger.Error($"Timer 参数错误 {ti.action.Method.ReflectedType.FullName} {ti.action.Method.Name}");
//
//?            if (!o.TryGetComponent<[T]>(out var [c])) continue;+\r?
//            var v = new Timer<?[T]+, ?>(?[c]+, ?);
//            v.timer = ti.attribute;
//            v.action = (Action<Timer<?[T]+, ?>>)ti.action;
//            o.World.System.timerHandles.Enqueue(v);
//#if UNITY_EDITOR
//            o._timers.Add(v);
//#endif
//        }
//    }
//    internal override void Update()
//    {
//        if (?![t].Enable+ || ?) return;
//        this.delay += t.World.DeltaTime;
//        if (this.delay >= timer.delay)
//        {
//            this.delay -= timer.delay;
//            this.count++;
//            action.Invoke(this);
//            if (timer.count > 0 && timer.count <= this.count)
//                this.Dispose();
//        }
//    }
//}
//end
