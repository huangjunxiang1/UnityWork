using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TaskAwaiterCreater
{
    readonly List<TaskAwaiter> tasks = new();

    public TaskAwaiter Create()
    {
        TaskAwaiter task = new();
        tasks.Add(task);
        waitRemove(task);
        return task;
    }
    public TaskAwaiter<T> Create<T>()
    {
        TaskAwaiter<T> task = new();
        tasks.Add(task);
        waitRemove(task);
        return task;
    }
    public TaskAwaiter Create(object tag)
    {
        TaskAwaiter task = new(tag);
        tasks.Add(task);
        waitRemove(task);
        return task;
    }
    public TaskAwaiter<T> Create<T>(object tag)
    {
        TaskAwaiter<T> task = new(tag);
        tasks.Add(task);
        waitRemove(task);
        return task;
    }
    public TaskAwaiter GetOrCreate(ref TaskAwaiter task)
    {
        if (task == null || task.IsCompleted || task.IsDisposed)
        {
            task = new TaskAwaiter();
            this.Add(task);
        }
        return task;
    }
    public TaskAwaiter<T> GetOrCreate<T>(ref TaskAwaiter<T> task)
    {
        if (task == null || task.IsCompleted || task.IsDisposed)
        {
            task = new TaskAwaiter<T>();
            this.Add(task);
        }
        return task;
    }
    public TaskAwaiter GetOrCreate(ref TaskAwaiter task,object tag)
    {
        if (task == null || task.IsCompleted || task.IsDisposed)
        {
            task = new TaskAwaiter(tag);
            this.Add(task);
        }
        else
        {
            if (!tag.Equals(task.Tag))
            {
                task.TryCancel();
                tasks.Remove(task);
                task = new TaskAwaiter(tag);
                this.Add(task);
            }
            else
            {
                task.Clear();
                waitRemove(task);//重新添加一个等待移除
            }
        }
        return task;
    }
    public TaskAwaiter<T> GetOrCreate<T>(ref TaskAwaiter<T> task, object tag)
    {
        if (task == null || task.IsCompleted || task.IsDisposed)
        {
            task = new TaskAwaiter<T>(tag);
            this.Add(task);
        }
        else
        {
            if (!tag.Equals(task.Tag))
            {
                task.TryCancel();
                tasks.Remove(task);
                task = new TaskAwaiter<T>(tag);
                this.Add(task);
            }
            else
            {
                task.Clear();
                waitRemove(task);//重新添加一个等待移除
            }
        }
        return task;
    }
    public TaskAwaiter Add(TaskAwaiter task)
    {
        tasks.Add(task);
        waitRemove(task);
        return task;
    }
    public void Remove(TaskAwaiter task)
    {
        tasks.Remove(task);
    }
    public void Dispose()
    {
        for (int i = 0; i < tasks.Count; i++)
            tasks[i].TryCancel();
        tasks.Clear();
    }

    async void waitRemove(TaskAwaiter task)
    {
        await task;
        Remove(task);
    }
}
