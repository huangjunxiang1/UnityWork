using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
#if FairyGUI
using FairyGUI;
#endif

[DebuggerNonUserCode]
public static class AsyncUtil
{
    public static STask AsTask(this AsyncOperation op)
    {
        if (op.isDone)
            return STask.Completed;

        STask task = new();
        op.completed += e => task.TrySetResult();
        return task;
    }

    public static STask AsTask(this IEnumerator ie)
    {
        if(ie.MoveNext())
            return STask.Completed;

        STask task = new();
        void update()
        {
            if (ie.MoveNext())
            {
                STimer.Remove(update);
                task.TrySetResult();
            }
        }
        STimer.Add(0.1f, -1, update);
        return task;
    }
#if FairyGUI
    public static STask AsTask(this GTweener tween)
    {
        if (tween.completed)
            return STask.Completed;

        STask task = new();
        tween.OnComplete(() => task.TrySetResult());
        return task;
    }
    public static STask PlayAsTask(this Transition t)
    {
        STask task = new();
        t.Play(() => task.TrySetResult());
        return task;
    }
    public static STask PlayReverseAsTask(this Transition t)
    {
        STask task = new();
        t.PlayReverse(() => task.TrySetResult());
        return task;
    }
    public static STask AsTask(this EventListener eventListener)
    {
        STask task = new();

        //只触发一次 然后移除掉
        void trigger()
        {
            eventListener.Remove(trigger);
            task.TrySetResult();
        }
        eventListener.Add(trigger);

        return task;
    }
#endif
    public static STask AsTask(this Tween tween)
    {
        if (tween.IsComplete())
            return STask.Completed;

        STask task = new();
        tween.OnComplete(() => task.TrySetResult());
        return task;
    }
    public static STask AsTask(this UnityEvent unityEvent)
    {
        STask task = new();

        //只触发一次 然后移除掉
        void trigger()
        {
            unityEvent.RemoveListener(trigger);
            task.TrySetResult();
        }
        unityEvent.AddListener(trigger);

        return task;
    }
    public static STask AsTask(this SEventListener eventer)
    {
        STask task = new();

        //只触发一次 然后移除掉
        void trigger()
        {
            eventer.Remove(trigger);
            task.TrySetResult();
        }
        eventer.Add(trigger);

        return task;
    }
}