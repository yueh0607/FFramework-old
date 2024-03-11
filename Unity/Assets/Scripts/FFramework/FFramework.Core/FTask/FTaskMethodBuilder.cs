using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;

namespace FFramework.Internal
{

    public readonly struct FTaskMethodBuilder
    {
        [DebuggerHidden]
        public static FTaskMethodBuilder Create() => new FTaskMethodBuilder(Pool.Get<FTask, FTask.FTaskPoolable>());

        private readonly FTask task;

        [DebuggerHidden]
        public FTaskMethodBuilder(FTask task)
        {
            this.task = task;
            ((ITaskTokenProperty)task).SetToken(Pool.Get<FTaskToken, FTaskToken.FTaskTokenPoolable>());
        }

        [DebuggerHidden]
        public readonly FTask Task => task;


        [DebuggerHidden]
        public readonly void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        [DebuggerHidden]
        public readonly void SetException(Exception exception)
        {
            task.GetAwaiter().SetException(exception);
            
        }


        //[DebuggerHidden]
        public readonly void SetResult()
        {
            //if (task.Token != null)
                //((ITaskTokenStatusSetter)task.Token).SetStatus(FTaskTokenStatus.Success, null);
            task.GetAwaiter().SetResult();
        }


        //[DebuggerHidden]
        public readonly void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
            if (awaiter is ITaskTokenProperty ftask)
            {
                //if(ftask.GetToken()!=null)
                //绑定当前任务的令牌
                ftask.SetToken(((ITaskTokenProperty)task).GetToken());
            }
        }

        [SecuritySafeCritical, DebuggerHidden]
        public readonly void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
            if (awaiter is ITaskTokenProperty ftask)
            {
                //if (ftask.GetToken() != null)
                    ftask.SetToken(((ITaskTokenProperty)task).GetToken());
            }
        }

        [DebuggerHidden]
        public readonly void SetStateMachine(IAsyncStateMachine stateMachine)
        {

        }
    }

    public readonly struct FTaskMethodBuilder<T>
    {
        [DebuggerHidden]
        public static FTaskMethodBuilder<T> Create() => new FTaskMethodBuilder<T>(Pool.Get<FTask<T>, FTask<T>.FTaskPoolable>());

        private readonly FTask<T> task;

        [DebuggerHidden]
        public FTaskMethodBuilder(FTask<T> task)
        {
            this.task = task;
            ((ITaskTokenProperty)task).SetToken(Pool.Get<FTaskToken, FTaskToken.FTaskTokenPoolable>());
        }

        [DebuggerHidden]
        public readonly FTask<T> Task => task;


        [DebuggerHidden]
        public readonly void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            stateMachine.MoveNext();
        }

        [DebuggerHidden]
        public readonly void SetException(Exception exception)
        {
            task.GetAwaiter().SetException(exception);
        }


        [DebuggerHidden]
        public readonly void SetResult(T result)
        {
           // if (task.Token != null)
                //((ITaskTokenStatusSetter)task.Token).SetStatus(FTaskTokenStatus.Success, null);
            task.GetAwaiter().SetResult(result);
        }


        [DebuggerHidden]
        public readonly void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
            if (awaiter is ITaskTokenProperty ftask)
            {
                //if (ftask.GetToken() != null)
                    ftask.SetToken(((ITaskTokenProperty)task).GetToken());
            }
        }

        [SecuritySafeCritical, DebuggerHidden]
        public readonly void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            awaiter.OnCompleted(stateMachine.MoveNext);
            if (awaiter is ITaskTokenProperty ftask)
            {
                //if (ftask.GetToken() != null)
                    ftask.SetToken(((ITaskTokenProperty)task).GetToken());
            }
        }

        [DebuggerHidden]
        public readonly void SetStateMachine(IAsyncStateMachine stateMachine)
        {

        }
    }
}