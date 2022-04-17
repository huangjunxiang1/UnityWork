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

    public static TaskAwaiter GetAwaiter(this GObject obj)
    {
        TaskAwaiter task = new();
        obj.onClick.Add(task.TrySetResult);
        return task;
    }
    public static TaskAwaiter GetAwaiter(this Button obj)
    {
        TaskAwaiter task = new();
        obj.onClick.AddListener(task.TrySetResult);
        return task;
    }
}