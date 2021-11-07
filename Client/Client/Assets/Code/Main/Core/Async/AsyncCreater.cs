using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[DebuggerNonUserCode]
public static class AsyncCreater
{
    public static TaskAwaiter GetAwaiter(this AsyncOperation op)
    {
        TaskAwaiter task = new TaskAwaiter();

        op.completed += e => task.TrySetResult();

        return task;
    }

    public static TaskAwaiter GetAwaiter(this IEnumerator ie)
    {
        TaskAwaiter task = new TaskAwaiter();

        void update()
        {
            if (ie.MoveNext())
            {
                Timer.Remove(update);
                task.TrySetResult();
            }
        }
        Timer.Add(-1, 0, update);

        return task;
    }
}