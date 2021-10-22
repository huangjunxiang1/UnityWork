using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


public sealed class TaskAwaiterBuilder
{
    TaskAwaiter _awaiter;
    public static TaskAwaiterBuilder Create()
    {
        TaskAwaiterBuilder builder = new TaskAwaiterBuilder() { _awaiter = new TaskAwaiter() };
        return builder;
    }
    public TaskAwaiter Task => _awaiter;

    public void SetException(Exception ex)
    {
        this._awaiter.SetException(ex);
    }
    public void SetResult()
    {
        this._awaiter.TrySetResult();
    }
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
    {
        awaiter.OnCompleted(stateMachine.MoveNext);
    }
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
    {
        awaiter.OnCompleted(stateMachine.MoveNext);
    }
    public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
    {
        stateMachine.MoveNext();
    }
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {

    }
}
