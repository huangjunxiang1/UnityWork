using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using FairyGUI;

[DebuggerNonUserCode]
public static class AsyncUtil
{
    public static TaskAwaiter AsTask(this AsyncOperation op)
    {
        if (op.isDone)
            return TaskAwaiter.Completed;

        TaskAwaiter task = new();
        op.completed += e => task.TrySetResult();
        return task;
    }

    public static TaskAwaiter AsTask(this IEnumerator ie)
    {
        if(ie.MoveNext())
            return TaskAwaiter.Completed;

        TaskAwaiter task = new();
        void update()
        {
            if (ie.MoveNext())
            {
                Timer.Remove(update);
                task.TrySetResult();
            }
        }
        Timer.Add(0.1f, -1, update);
        return task;
    }
    public static TaskAwaiter AsTask(this GTweener tween)
    {
        if (tween.completed)
            return TaskAwaiter.Completed;

        TaskAwaiter task = new();
        tween.OnComplete(() => task.TrySetResult());
        return task;
    }
    public static TaskAwaiter AsTask(this Tween tween)
    {
        if (tween.IsComplete())
            return TaskAwaiter.Completed;

        TaskAwaiter task = new();
        tween.OnComplete(() => task.TrySetResult());
        return task;
    }

    public static TaskAwaiter AsTask(this EventListener eventListener)
    {
        TaskAwaiter task = new();

        //只触发一次 然后移除掉
        void trigger()
        {
            eventListener.Remove(trigger);
            task.TrySetResult();
        }
        eventListener.Add(trigger);

        return task;
    }
    public static TaskAwaiter AsTask(this UnityEvent unityEvent)
    {
        TaskAwaiter task = new();

        //只触发一次 然后移除掉
        void trigger()
        {
            unityEvent.RemoveListener(trigger);
            task.TrySetResult();
        }
        unityEvent.AddListener(trigger);

        return task;
    }
    public static TaskAwaiter AsTask(this Eventer eventer)
    {
        TaskAwaiter task = new();

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