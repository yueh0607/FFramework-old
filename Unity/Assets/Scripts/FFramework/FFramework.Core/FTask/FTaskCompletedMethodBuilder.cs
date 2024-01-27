using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Security;

namespace FFramework
{
    public struct FTaskCompletedMethodBuilder
    {
        // 1. Static Create method.
        [DebuggerHidden]
        public static FTaskCompletedMethodBuilder Create()
        {
            return new FTaskCompletedMethodBuilder();
        }

        // 2. TaskLike Task property(void)
        public readonly FTaskCompleted Task => default;

        // 3. SetException
        [DebuggerHidden]
        public readonly void SetException(Exception e)
        {
            FTaskToken.ErrorHandler?.Invoke(ExceptionDispatchInfo.Capture(e));
        }

        // 4. SetResult
        [DebuggerHidden]
        public readonly void SetResult()
        {
            // do nothing
        }

        // 5. AwaitOnCompleted
        [DebuggerHidden]
        public readonly void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : INotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            //awaiter.OnCompleted(stateMachine.MoveNext);
        }

        // 6. AwaitUnsafeOnCompleted
        [DebuggerHidden]
        [SecuritySafeCritical]
        public readonly void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine) where TAwaiter : ICriticalNotifyCompletion where TStateMachine : IAsyncStateMachine
        {
            //awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
        }

        // 7. Start
        [DebuggerHidden]
        public readonly void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            //stateMachine.MoveNext();
        }

        // 8. SetStateMachine
        [DebuggerHidden]
        public readonly void SetStateMachine(IAsyncStateMachine stateMachine)
        {
        }
    }
}
