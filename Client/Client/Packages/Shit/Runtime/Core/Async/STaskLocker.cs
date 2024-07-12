using System;
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
    static Dictionary<object, STaskLocker> locks = new();
    static Dictionary<long, STaskLocker> locks2 = new();

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
        if (!locks.TryGetValue(key, out var locker) || locker.Disposed)
        {
            locks[key] = locker = new STaskLocker() { key = key };
            if (timeout > 0)
                locker.timeout(timeout);
            return locker;
        }
        else
        {
            STask task = locker.next = new STask();
            locker = new STaskLocker() { key = key };
            locks[key] = locker;
            await task;
            if (timeout > 0)
                locker.timeout(timeout);
            return locker;
        }
    }
    public static async STask<STaskLocker> Lock(long key, int timeout = 5000)
    {
        if (key == 0)
        {
            Loger.Error("key cannot is 0");
            return null;
        }
        if (!locks2.TryGetValue(key, out var locker) || locker.Disposed)
        {
            locks2[key] = locker = new STaskLocker() { key2 = key };
            if (timeout > 0)
                locker.timeout(timeout);
            return locker;
        }
        else
        {
            STask task = locker.next = new STask();
            locker = new STaskLocker() { key2 = key };
            locks2[key] = locker;
            await task;
            if (timeout > 0)
                locker.timeout(timeout);
            return locker;
        }
    }
    public static void UnLock(object key)
    {
        if (!locks.TryGetValue(key, out var locker))
        {
            Loger.Error($"没有对应的锁 key={key}");
            return;
        }
        locker.Dispose();
    }
    public static void UnLock(long key)
    {
        if (!locks2.TryGetValue(key, out var locker))
        {
            Loger.Error($"没有对应的锁 key={key}");
            return;
        }
        locker.Dispose();
    }

    public void Dispose()
    {
        Disposed = true;
        if (key != null && locks.TryGetValue(key, out var v) && v == this)
            locks.Remove(key);
        if (key2 != 0 && locks2.TryGetValue(key2, out var v2) && v2 == this)
            locks2.Remove(key2);
        next?.TrySetResult();
    }

    async void timeout(int time)
    {
        await STask.Delay(time);
        if (this.Disposed) return;
        this.Dispose();
    }
}
