using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// STask 有自动取消机制 所以这里使用要注意 单次锁 如果异步被中途取消 会走到超时逻辑
/// </summary>
public class STaskLocker : IDispose
{
    static ConcurrentDictionary<object, STaskLocker> lockObj = new();
    static ConcurrentDictionary<long, STaskLocker> lockLong = new();

    object key;
    long key2;
    STask next;

    public bool Disposed { get; private set; }

    public static STask Lock(ref STaskLocker locker, int timeout = 5000)
    {
        if (locker == null || locker.Disposed)
        {
            locker = new STaskLocker();
            if (timeout > 0)
                locker.timeout(timeout);
            return STask.Completed;
        }
        STask task = locker.next = new STask();
        var tmp = locker = new STaskLocker();
        task.AddEvent(() =>
        {
            if (timeout > 0)
                tmp.timeout(timeout);
        });
        return task;
    }
    public static async STask<STaskLocker> Lock(object key, int timeout = 5000)
    {
        STask task;
        STaskLocker locker;

        lock (lockObj)
        {
            if (!lockObj.TryGetValue(key, out locker) || locker.Disposed)
            {
                lockObj[key] = locker = new STaskLocker() { key = key };
                if (timeout > 0)
                    locker.timeout(timeout);
                return locker;
            }
            else
            {
                task = locker.next = new STask();
                locker = new STaskLocker() { key = key };
                lockObj[key] = locker;
            }
        }

        await task;
        if (timeout > 0 && !locker.Disposed)
            locker.timeout(timeout);
        return locker;
    }
    public static async STask<STaskLocker> Lock(long key, int timeout = 5000)
    {
        if (key == 0)
        {
            Loger.Error("key cannot is 0");
            return null;
        }
        STask task;
        STaskLocker locker;

        lock (lockLong)
        {
            if (!lockLong.TryGetValue(key, out locker) || locker.Disposed)
            {
                lockLong[key] = locker = new STaskLocker() { key2 = key };
                if (timeout > 0)
                    locker.timeout(timeout);
                return locker;
            }
            else
            {
                task = locker.next = new STask();
                locker = new STaskLocker() { key2 = key };
                lockLong[key] = locker;
            }
        }

        await task;
        if (timeout > 0 && !locker.Disposed)
            locker.timeout(timeout);
        return locker;
    }

    public void Dispose()
    {
        Disposed = true;
        if (key != null && lockObj.TryGetValue(key, out var v) && v == this)
            lockObj.TryRemove(key, out _);
        if (key2 != 0 && lockLong.TryGetValue(key2, out var v2) && v2 == this)
            lockLong.TryRemove(key2, out _);
        next?.TrySetResult();
    }

    async void timeout(int time)
    {
        await SValueTask.Delay(time);
        if (this.Disposed) return;
        this.Dispose();
    }
}
