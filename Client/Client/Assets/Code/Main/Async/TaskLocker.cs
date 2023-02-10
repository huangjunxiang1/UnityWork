using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TaskLocker : IDisposable
{
    public bool IsDisposed { get; private set; }

    TaskAwaiter next;

    public static TaskAwaiter Lock(ref TaskLocker locker)
    {
        if (locker == null || locker.IsDisposed)
        {
            locker = new TaskLocker();
            return TaskAwaiter.Completed;
        }
        TaskAwaiter task = locker.next = new TaskAwaiter();
        locker = new TaskLocker();
        return task;
    }

    public void Dispose()
    {
        IsDisposed = true;
        next?.TrySetResult();
    }
}
