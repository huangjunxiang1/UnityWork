using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Reflection;
using Game;

[AsyncMethodBuilder(typeof(TaskAwaiterBuilder))]
public class TaskAwaiter : ICriticalNotifyCompletion
{
    static TaskAwaiter()
    {
        var asm = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < asm.Length; i++)
        {
            if (asm[i].FullName.StartsWith("mscorlib"))
            {
                runner = asm[i].GetType("System.Runtime.CompilerServices.AsyncMethodBuilderCore+MoveNextRunner");
                break;
            }
        }
        field = runner.GetField("m_stateMachine", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    static Type runner;
    static FieldInfo field;
    static Dictionary<object, HashSet<TaskAwaiter>> objAsync = new Dictionary<object, HashSet<TaskAwaiter>>();

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
    bool _isDisposed = false;
    List<TaskAwaiter> _route;//异步链的路线
    List<object> _routeTargets;

    public object Tag { get; }
    public Task WarpTask { get; }

    public bool IsDisposed
    {
        get
        {
            if (_isDisposed) return true;
            if (_route != null)
            {
                for (int i = 0; i < _route.Count; i++)
                {
                    if (!_route[i].IsDisposed)
                        return false;
                }
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 是否已完成
    /// </summary>
    public bool IsCompleted { get; private set; }

    public static TaskAwaiter Completed { get; } = new TaskAwaiter() { IsCompleted = true, _isCanOp = false };

    public static void RigisterAsync(object o, TaskAwaiter task)
    {
        if (!objAsync.TryGetValue(o, out var value))
            objAsync[o] = value = ObjectPool.Get<HashSet<TaskAwaiter>>();
        if (!value.Contains(task))
            value.Add(task);
    }
    public static void RemoveAllAsync(object o)
    {
        if (objAsync.TryGetValue(o, out var value))
        {
            objAsync.Remove(o);
            foreach (var item in value)
                item.TryCancel();
            value.Clear();
            ObjectPool.Return(value);
        }
    }
    public static void RemoveAsync(object o, TaskAwaiter task)
    {
        if (objAsync.TryGetValue(o, out var value))
            value.Remove(task);
    }

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

        this._isDisposed = true;

        if (this._routeTargets != null)
        {
            for (int i = 0; i < _routeTargets.Count; i++)
                RemoveAsync(_routeTargets[i], this);
            _routeTargets.Clear();
            ObjectPool.Return(_routeTargets);
        }
        if (_route != null)
        {
            _route.Clear();
            ObjectPool.Return(_route);
            _route = null;
        }
        this._event = null;
    }

    /// <summary>
    /// 执行下一步
    /// </summary>
    public bool TrySetResult()
    {
        if (!_isCanOp) return false;
        if (this.IsDisposed || this.IsCompleted) return false;
       
        this._isDisposed = true;
        this.IsCompleted = true;

        if (this._routeTargets != null)
        {
            for (int i = 0; i < _routeTargets.Count; i++)
                RemoveAsync(_routeTargets[i], this);
            _routeTargets.Clear();
            ObjectPool.Return(_routeTargets);
        }
        if (_route != null)
        {
            _route.Clear();
            ObjectPool.Return(_route);
            _route = null;
        }
        Action act = this._event;
        this._event = null;
        act?.Invoke();
        return true;
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

        this._isDisposed = true;

        if (this._routeTargets != null)
        {
            for (int i = 0; i < _routeTargets.Count; i++)
                RemoveAsync(_routeTargets[i], this);
            _routeTargets.Clear();
            ObjectPool.Return(_routeTargets);
        }
        if (_route != null)
        {
            _route.Clear();
            ObjectPool.Return(_route);
            _route = null;
        }
        Action act = this._event;
        this._event = null;
        act?.Invoke();
    }

    public void AddAsyncRoute(TaskAwaiter route)
    {
        _route ??= ObjectPool.Get<List<TaskAwaiter>>();
        _route.Add(route);
    }

    public void AddEvent(Action evt)
    {
        if (evt == null)
            return;
        if (IsCompleted)
        {
            evt.Invoke();
            return;
        }
        this._event += evt;
    }

    void INotifyCompletion.OnCompleted(Action callBack)
    {
        this._event += callBack;
        _asyncTargetAnalysis(callBack.Target);
    }
    void ICriticalNotifyCompletion.UnsafeOnCompleted(Action callBack)
    {
        this._event += callBack;
        _asyncTargetAnalysis(callBack.Target);
    }

    void _asyncTargetAnalysis(object target)
    {
        if (target.GetType() == runner)
        {
            var v = field.GetValue(target);
            var f = Types.GetStateMachineThisField(v.GetType());
            var o = f.GetValue(v);
            Type t = o.GetType();
#if ILRuntime
            if (o is ILRuntime.Runtime.Intepreter.ILTypeInstance ilInstance)
                t = ilInstance.Type.ReflectionType;
            else if (o is ILRuntime.Runtime.Enviorment.CrossBindingAdaptorType ilWarp)
                t = ilWarp.ILInstance.Type.ReflectionType;
#endif
            if (typeof(ObjectM).IsAssignableFrom(t) || Types.GetHotRootType().IsAssignableFrom(t))
            {
                if (!Types.GetMethodAsyncDontCancel(t, v.GetType()))
                {
                    _routeTargets ??= ObjectPool.Get<List<object>>();
                    _routeTargets.Add(o);
                    RigisterAsync(o, this);
                }
            }
        }
    }


    static async void waitTask(TaskAwaiter taskAwaiter,Task task)
    {
        await task;
        taskAwaiter.TrySetResult();
    }

    public static TaskAwaiter Delay(int millisecondsDelay)
    {
        return new TaskAwaiter(Task.Delay(millisecondsDelay));
    }
    public static async TaskAwaiter All(IEnumerable<TaskAwaiter> itor)
    {
        if (itor == null)
            await TaskAwaiter.Completed;

        TaskAwaiter[] tasks = itor.ToArray();
        for (int i = 0; i < tasks.Length; i++)
            await tasks[i];
    }
    public static async TaskAwaiter All(params TaskAwaiter[] tasks)
    {
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
    public static async TaskAwaiter<K[]> All<K>(params TaskAwaiter<K>[] tasks)
    {
        K[] rs = new K[tasks.Length];
        for (int i = 0; i < tasks.Length; i++)
            rs[i] = await tasks[i];
        return rs;
    }
    public static TaskAwaiter Any(IEnumerable<TaskAwaiter> itor)
    {
        if (itor == null)
            return TaskAwaiter.Completed;

        TaskAwaiter waiter = new();
        TaskAwaiter[] tasks = itor.ToArray();

        async void wait(TaskAwaiter task)
        {
            await task;
            waiter.TrySetResult();
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);
        return waiter;
    }
    public static TaskAwaiter Any(params TaskAwaiter[] tasks)
    {
        TaskAwaiter waiter = new();
        async void wait(TaskAwaiter task)
        {
            await task;
            waiter.TrySetResult();
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);
        return waiter;
    }
    public static TaskAwaiter<K> Any<K>(IEnumerable<TaskAwaiter<K>> itor)
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
            }

            for (int i = 0; i < tasks.Length; i++)
                wait(tasks[i]);
        }
        return waiter;
    }
    public static TaskAwaiter<K> Any<K>(params TaskAwaiter<K>[] tasks)
    {
        TaskAwaiter<K> waiter = new();

        async void wait(TaskAwaiter<K> task)
        {
            await task;
            waiter.TrySetResult(task.GetResult());
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);

        return waiter;
    }
    public static TaskAwaiter<K> AnyAfterCancel<K>(IEnumerable<TaskAwaiter<K>> itor)
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
                for (int i = 0; i < tasks.Length; i++)
                    tasks[i].TryCancel();
            }

            for (int i = 0; i < tasks.Length; i++)
                wait(tasks[i]);
        }
        return waiter;
    }
    public static TaskAwaiter<K> AnyAfterCancel<K>(params TaskAwaiter<K>[] tasks)
    {
        TaskAwaiter<K> waiter = new();

        async void wait(TaskAwaiter<K> task)
        {
            await task;
            waiter.TrySetResult(task.GetResult());
            for (int i = 0; i < tasks.Length; i++)
                tasks[i].TryCancel();
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);

        return waiter;
    }
    public static TaskAwaiter AnyAfterCancel(IEnumerable<TaskAwaiter> itor)
    {
        if (itor == null)
            return TaskAwaiter.Completed;

        TaskAwaiter waiter = new();
        TaskAwaiter[] tasks = itor.ToArray();

        async void wait(TaskAwaiter task)
        {
            await task;
            waiter.TrySetResult();
            for (int i = 0; i < tasks.Length; i++)
                tasks[i].TryCancel();
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);
        return waiter;
    }
    public static TaskAwaiter AnyAfterCancel(params TaskAwaiter[] tasks)
    {
        TaskAwaiter waiter = new();
        async void wait(TaskAwaiter task)
        {
            await task;
            waiter.TrySetResult();
            for (int i = 0; i < tasks.Length; i++)
                tasks[i].TryCancel();
        }

        for (int i = 0; i < tasks.Length; i++)
            wait(tasks[i]);
        return waiter;
    }
}
