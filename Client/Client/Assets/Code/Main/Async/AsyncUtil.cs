using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using FairyGUI;
using UnityEngine.UI;
using UnityEngine.Events;

[DebuggerNonUserCode]
public static class AsyncUtil
{
    public static TaskAwaiter GetAwaiter(this AsyncOperation op)
    {
        TaskAwaiter task = new();
        op.completed += e => task.TrySetResult();
        return task;
    }

    public static TaskAwaiter GetAwaiter(this IEnumerator ie)
    {
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
    public static TaskAwaiter GetAwaiter(this GTweener tween)
    {
        TaskAwaiter task = new();
        tween.OnComplete(task.TrySetResult);
        return task;
    }
    public static TaskAwaiter GetAwaiter(this Tween tween)
    {
        TaskAwaiter task = new();
        tween.OnComplete(task.TrySetResult);
        return task;
    }

    public static TaskAwaiter GetAwaiter(this EventListener eventListener)
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
    public static TaskAwaiter GetAwaiter(this UnityEvent unityEvent)
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
}