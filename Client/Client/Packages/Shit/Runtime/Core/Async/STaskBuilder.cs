using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[DebuggerNonUserCode]
public struct STaskBuilder
{
    STask _task;
    public static STaskBuilder Create()
    {
        return new();
    }
    public STask Task => _task;

    public void SetException(Exception ex)
    {
        this._task.SetException(ex);
    }
    public void SetResult()
    {
        this._task.TrySetResult();
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
        //this._task.MakeAutoCancel(CoreTypes.AsyncInvokeIsNeedAutoCancel(stateMachine.GetType()));
        stateMachine.MoveNext();
    }
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {

    }
}
