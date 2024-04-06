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
            //((ITaskTokenHolder)task).SetToken(Pool.Get<FTaskToken, FTaskToken.FTaskTokenPoolable>());
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


        [DebuggerHidden]
        public readonly void SetResult()
        {
            task.GetAwaiter().SetResult();
        }


        [DebuggerHidden]
        public readonly void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is ITaskTokenHolder ftask)
            {
                //设置子任务
                ((IChildTaskHolder)task).SetChildTask(ftask);

                //获取当前Builder任务的令牌
                var rootToken = ((ITaskTokenHolder)task).GetToken();
                //设置awaiter的令牌
                ftask.SetToken(rootToken);

                //执行
                if (rootToken == null || rootToken.Status.IsPending())
                    awaiter.OnCompleted(stateMachine.MoveNext);
                //挂起
                else if (rootToken.Status.IsYield())
                    ((ITaskTokenStatusSetter)rootToken).YieldHold(stateMachine.MoveNext);
                //取消
                else if (rootToken.Status.IsCancelled())
                    task.GetAwaiter().SetCanceled();
                return;
            }
            throw new InvalidOperationException("Awaiter is not a FTaskAwaiter");
        }

        [SecuritySafeCritical, DebuggerHidden]
        public readonly void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            
            if (awaiter is ITaskTokenHolder ftask)
            {
                //设置子任务
                ((IChildTaskHolder)task.GetAwaiter()).SetChildTask(ftask);

                //获取当前Builder任务的令牌
                var rootToken = ((ITaskTokenHolder)task).GetToken();
                //设置awaiter的令牌
                ftask.SetToken(rootToken);

                //执行
                if (rootToken == null || rootToken.Status.IsPending())
                    awaiter.OnCompleted(stateMachine.MoveNext);
                //挂起
                else if (rootToken.Status.IsYield())
                    ((ITaskTokenStatusSetter)rootToken).YieldHold(stateMachine.MoveNext);
                //取消
                else if (rootToken.Status.IsCancelled())
                    task.GetAwaiter().SetCanceled();
                return;
            }
            throw new InvalidOperationException("Awaiter is not a FTaskAwaiter");
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
            //((ITaskTokenHolder)task).SetToken(Pool.Get<FTaskToken, FTaskToken.FTaskTokenPoolable>());
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
            task.GetAwaiter().SetResult(result);
        }


        [DebuggerHidden]
        public readonly void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is ITaskTokenHolder ftask)
            {
                //设置子任务
                ((IChildTaskHolder)task).SetChildTask(ftask);

                //获取当前Builder任务的令牌
                var rootToken = ((ITaskTokenHolder)task).GetToken();
                //设置awaiter的令牌
                ftask.SetToken(rootToken);

                //执行
                if (rootToken == null || rootToken.Status.IsPending())
                    awaiter.OnCompleted(stateMachine.MoveNext);
                //挂起
                else if (rootToken.Status.IsYield())
                    ((ITaskTokenStatusSetter)rootToken).YieldHold(stateMachine.MoveNext);
                //取消
                else if (rootToken.Status.IsCancelled())
                    task.GetAwaiter().SetCanceled();
                return;
            }
            throw new InvalidOperationException("Awaiter is not a FTaskAwaiter");
        }

        [SecuritySafeCritical, DebuggerHidden]
        public readonly void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            if (awaiter is ITaskTokenHolder ftask)
            {
                //设置子任务
                ((IChildTaskHolder)task).SetChildTask(ftask);

                //获取当前Builder任务的令牌
                var rootToken = ((ITaskTokenHolder)task).GetToken();
                //设置awaiter的令牌
                ftask.SetToken(rootToken);

                //执行
                if (rootToken == null || rootToken.Status.IsPending())
                    awaiter.OnCompleted(stateMachine.MoveNext);
                //挂起
                else if (rootToken.Status.IsYield())
                    ((ITaskTokenStatusSetter)rootToken).YieldHold(stateMachine.MoveNext);
                //取消
                else if (rootToken.Status.IsCancelled())
                    task.GetAwaiter().SetCanceled();
                return;
            }
            throw new InvalidOperationException("Awaiter is not a FTaskAwaiter");
        }

        [DebuggerHidden]
        public readonly void SetStateMachine(IAsyncStateMachine stateMachine)
        {

        }
    }
}