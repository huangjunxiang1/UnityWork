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
    internal static List<TaskItem> taskItems = new();
    internal static ConcurrentQueue<int> poolIndexs = new();

    public SValueTask(int a)
    {
        TaskItem ti;
        if (!poolIndexs.TryDequeue(out this.index))
        {
            lock (taskItems)
            {
                this.index = taskItems.Count;
                taskItems.Add(ti = new TaskItem { });
            }
        }
        else
            ti = taskItems[this.index];
        this.version = ti.version;
    }

    int index;
    uint version;

    public bool Disposed => taskItems[index].version != version;
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted => taskItems[index].version != version;

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
        lock (taskItems)
        {
            poolIndexs.Enqueue(this.index);
            ++taskItems[index].version;
            taskItems[index].action = null;
        }
    }
    public void TrySetResult()
    {
        if (this.Disposed) return;
        Action act;
        lock (taskItems)
        {
            poolIndexs.Enqueue(this.index);
            ++taskItems[index].version;
            act = taskItems[index].action;
            taskItems[index].action = null;
        }
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
}