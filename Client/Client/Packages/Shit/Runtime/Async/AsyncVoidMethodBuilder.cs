using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NTask = TaskAwaiter;

namespace System.Runtime.CompilerServices
{
    public class AsyncVoidMethodBuilder : AsyncBaseBuilder
    {
        public static AsyncVoidMethodBuilder Create()
        {
            return new();
        }

        public void SetException(Exception ex)
        {
            Loger.Error("AsyncVoidMethodBuilder Error " + ex);
        }
        public void SetResult()
        {

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
            this.Target = Types.GetStateMachineThisField(stateMachine.GetType())?.GetValue(stateMachine) as IAsyncDisposed;
            stateMachine.MoveNext();
        }
        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {

        }
    }
}
