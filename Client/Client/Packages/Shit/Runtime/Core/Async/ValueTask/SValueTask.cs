using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[AsyncMethodBuilder(typeof(SValueTaskBuilder))]
public struct SValueTask : ICriticalNotifyCompletion, IDispose
{
    internal class TaskItem
    {
        public uint version;
        public Action action;
    }
    internal static List<TaskItem> taskItems = new() { new TaskItem { version = 1 } };
    internal static ConcurrentQueue<int> poolIndexs = new(new int[] { 0 });

    public static Action<int, SValueTask> DelayHandle;

    public static SValueTask Create()
    {
        SValueTask task = new();
        TaskItem ti;
        if (!poolIndexs.TryDequeue(out task.index))
        {
            lock (taskItems)
            {
                task.index = taskItems.Count;
                taskItems.Add(ti = new TaskItem { });
            }
        }
        else
            ti = taskItems[task.index];
        task.version = ti.version;
        return task;
    }

    int index;
    uint version;

    public bool Disposed => taskItems[index].version != version;
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted => taskItems[index].version != version ;

    public SValueTask GetAwaiter()
    {
        return this;
    }
    public void GetResult()
    {
        
    }

    public void TryCancel()
    {
        if (this.Disposed) return;
        ++taskItems[index].version;
        taskItems[index].action = null;
        poolIndexs.Enqueue(this.index);
    }
    public void TrySetResult()
    {
        if (this.Disposed) return;
        ++taskItems[index].version;
        Action act = taskItems[index].action;
        taskItems[index].action = null;
        poolIndexs.Enqueue(this.index);
        act?.Invoke();
    }

    public void Dispose() => this.TryCancel();

    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="e"></param>
    public void SetException(Exception e)
    {
        Loger.Error("SValueTask Error " + e);
        this.TrySetResult();
    }

    void INotifyCompletion.OnCompleted(Action continuation)
    {
        taskItems[index].action += continuation;
    }

    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
    {
        taskItems[index].action += continuation;
    }


    public static SValueTask Delay(int millisecondsDelay)
    {
        var task = SValueTask.Create();
#if DebugEnable
        if (DelayHandle == null)
            throw new Exception("muse be set DelayHandle before invoke Delay");
#endif
        DelayHandle.Invoke(millisecondsDelay, task);
        return task;
    }
}