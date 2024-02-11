using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FFramework
{
    public struct FTaskCompleted : ICriticalNotifyCompletion
    {
        [DebuggerHidden]
        public readonly FTaskCompleted GetAwaiter()
        {
            return this;
        }
  
        [DebuggerHidden]
        public readonly bool IsCompleted => true;

        [DebuggerHidden]
        public readonly void GetResult()
        {
        }

        [DebuggerHidden]
        public readonly void OnCompleted(Action continuation)
        {
            //UnsafeOnCompleted(continuation);
        }

        [DebuggerHidden]
        public readonly void UnsafeOnCompleted(Action continuation)
        {
            //continuation?.Invoke();
        }
    }
}
