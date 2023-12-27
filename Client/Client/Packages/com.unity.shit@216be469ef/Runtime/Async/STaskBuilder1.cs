using Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[DebuggerNonUserCode]
public sealed class STaskBuilder1<T> 
{
    STask<T> _task;
    public static STaskBuilder1<T> Create()
    {
        return new();
    }
    public STask<T> Task => _task;

    public void SetException(Exception ex)
    {
        this._task.SetException(ex);
    }
    public void SetResult(T result)
    {
        this._task.TrySetResult(result);
    }
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
    {
        awaiter.OnCompleted(stateMachine.MoveNext);
    }
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
    {
        awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
    }
    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
    {
        this._task = new();
        this._task.MakeAutoCancel(Types.AsyncInvokeIsNeedAutoCancel(stateMachine.GetType()));
        stateMachine.MoveNext();
    }
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {

    }
}
