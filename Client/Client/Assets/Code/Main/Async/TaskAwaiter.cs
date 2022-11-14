using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Main;
using System.Diagnostics;
using System.Security;

[DebuggerNonUserCode]
[AsyncMethodBuilder(typeof(TaskAwaiterBuilder))]
public class TaskAwaiter : ICriticalNotifyCompletion
{
    public TaskAwaiter()
    {

    }
    public TaskAwaiter(object tag)
    {
        this.Tag = tag;
    }
    public TaskAwaiter(Task warpTask)
    {
        this.WarpTask = warpTask;
        waitTask(this, warpTask);
    }

    Action _event;
    bool _isCanOp = true;

    public object Tag { get; }
    public Task WarpTask { get; }

    public bool IsDisposed { get; private set; }
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted { get; private set; }

    public static TaskAwaiter Completed { get; } = new TaskAwaiter() { IsCompleted = true, _isCanOp = false };

    public TaskAwaiter GetAwaiter()
    {
        return this;
    }
    public void GetResult()
    {

    }

    /// <summary>
    /// 取消
    /// </summary>
    public void TryCancel()
    {
        if (!_isCanOp) return;
        if (this.IsDisposed || this.IsCompleted) return;

        this.IsDisposed = true;
        this._event = null;
    }

    /// <summary>
    /// 执行下一步
    /// </summary>
    public void TrySetResult()
    {
        if (!_isCanOp) return;
        if (this.IsDisposed || this.IsCompleted) return;

        this.IsDisposed = true;
        this.IsCompleted = true;

        Action act = this._event;
        this._event = null;
        act?.Invoke();
    }

    /// <summary>
    /// 错误
    /// </summary>
    /// <param name="e"></param>
    public void SetException(Exception e)
    {
        Loger.Error("TaskAwaiter Error " + e);
        if (!_isCanOp) return;
        if (this.IsDisposed || this.IsCompleted) return;

        this.IsDisposed = true;

        Action act = this._event;
        this._event = null;
        act?.Invoke();
    }

    /// <summary>
    /// 重置回调
    /// </summary>
    public void Clear()
    {
        if (!_isCanOp) return;
        if (this.IsDisposed || this.IsCompleted) return;

        this._event = null;
    }

    void INotifyCompletion.OnCompleted(Action callBack)
    {
        this._event += callBack;
    }
    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action callBack)
    {
        this._event += callBack;
    }


    static async void waitTask(TaskAwaiter taskAwaiter,Task task)
    {
        await task;
        taskAwaiter.TrySetResult();
    }
    public static async TaskAwaiter All(IEnumerable<TaskAwaiter> itor)
    {
        if (itor == null)
            await TaskAwaiter.Completed;

        TaskAwaiter[] tasks = itor.ToArray();
        for (int i = 0; i < tasks.Length; i++)
            await tasks[i];
    }
    public static async TaskAwaiter<K[]> All<K>(IEnumerable<TaskAwaiter<K>> itor)
    {
        if (itor == null)
            return new K[0];
        else
        {
            TaskAwaiter<K>[] tasks = itor.ToArray();
            K[] rs = new K[tasks.Length];
            for (int i = 0; i < tasks.Length; i++)
                rs[i] = await tasks[i];
            return rs;
        }
    }
    public static TaskAwaiter Any(IEnumerable<TaskAwaiter> itor, bool canelOthersAfterCompleted = true)
    {
        if (itor == null)
            return TaskAwaiter.Completed;

        TaskAwaiter waiter = new();
        TaskAwaiter[] tasks = itor.ToArray();

        async void wait(TaskAwaiter task)
        {
            await task;
            waiter.TrySetResult();
            if (canelOthersAfterCompleted)
            {
                for (int j = 0; j < tasks.Length; j++)
                    tasks[j].TryCancel();
            }
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);
        return waiter;
    }
    public static TaskAwaiter<K> Any<K>(IEnumerable<TaskAwaiter<K>> itor, bool canelOthersAfterCompleted = true)
    {
        TaskAwaiter<K> waiter = new();
        if (itor == null) waiter.TrySetResult(default);
        else
        {
            TaskAwaiter<K>[] tasks = itor.ToArray();

            async void wait(TaskAwaiter<K> task)
            {
                await task;
                waiter.TrySetResult(task.GetResult());
                if (canelOthersAfterCompleted)
                {
                    for (int j = 0; j < tasks.Length; j++)
                        tasks[j].TryCancel();
                }
            }

            for (int i = 0; i < tasks.Length; i++)
                wait(tasks[i]);
        }
        return waiter;
    }
}
