using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        Timer.Add(0, -1, update);

        return task;
    }
}