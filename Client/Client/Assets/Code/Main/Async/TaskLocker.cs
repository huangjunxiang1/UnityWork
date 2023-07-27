using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// TaskAwaiter 有自动取消机制 所以这里使用要注意 单次锁 如果异步被中途取消 会走到超时逻辑
/// </summary>
public class TaskLocker : IDisposable
{
    static Dictionary<object, TaskLocker> locks = new();
    static Dictionary<long, TaskLocker> locks2 = new();

    object key;
    long key2;
    TaskAwaiter next;
    CancellationTokenSource cts = new();

    public bool IsDisposed { get; private set; }

    public static TaskAwaiter Lock(ref TaskLocker locker, int timeout = 5000)
    {
        if (locker == null || locker.IsDisposed)
        {
            locker = new TaskLocker();
            return TaskAwaiter.Completed;
        }
        TaskAwaiter task = locker.next = new TaskAwaiter();
        locker = new TaskLocker();
        locker.timeout(timeout);
        return task;
    }
    public static async TaskAwaiter<TaskLocker> Lock(object key, int timeout = 5000)
    {
        if (!locks.TryGetValue(key, out var locker) || locker.IsDisposed)
        {
            locks[key] = locker = new TaskLocker() { key = key };
            locker.timeout(timeout);
            return locker;
        }
        else
        {
            TaskAwaiter task = locker.next = new TaskAwaiter();
            locker = new TaskLocker() { key = key };
            locks[key] = locker;
            locker.timeout(timeout);
            await task;
            return locker;
        }
    }
    public static async TaskAwaiter<TaskLocker> Lock(long key, int timeout = 5000)
    {
        if (key == 0)
        {
            Loger.Error("key不能为0");
            return null;
        }
        if (!locks2.TryGetValue(key, out var locker) || locker.IsDisposed)
        {
            locks2[key] = locker = new TaskLocker() { key2 = key };
            locker.timeout(timeout);
            return locker;
        }
        else
        {
            TaskAwaiter task = locker.next = new TaskAwaiter();
            locker = new TaskLocker() { key2 = key };
            locks2[key] = locker;
            locker.timeout(timeout);
            await task;
            return locker;
        }
    }

    public void Dispose()
    {
        IsDisposed = true;
        cts.Cancel();
        if (key != null && locks.TryGetValue(key, out var v) && v == this)
            locks.Remove(key);
        if (key2 != 0 && locks2.TryGetValue(key2, out var v2) && v2 == this)
            locks2.Remove(key2);
        next?.TrySetResult();
    }

    async void timeout(int time)
    {
        cts = new();
        //取消异步会报错 所以这里加try 不打印错误
        try
        {
            await Task.Delay(time, cts.Token);
        }
        catch (Exception)
        {
        }
        this.Dispose();
    }
}
