
using FFramework;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace FFramework
{
    public partial class FTaskCatchToken : FUnit, ITaskTokenHolder,ICriticalNotifyCompletion
    {

        private FTaskToken token = null;

        /// <summary>
        /// 任务令牌，用于标识任务链的状态以及进行操作
        /// </summary>
        public FTaskToken Token => token;

        public FTaskCatchToken() { }


        public FTaskCatchToken GetAwaiter() => this;


        public class FTaskCatchTokenPoolable : IPoolable<FTaskCatchToken>
        {
            int IPoolable.Capacity => 1000;

            FTaskCatchToken IPoolable<FTaskCatchToken>.OnCreate()
            {
                return new FTaskCatchToken();
            }

            void IPoolable<FTaskCatchToken>.OnDestroy(FTaskCatchToken obj)
            {

            }

            void IPoolable<FTaskCatchToken>.OnGet(FTaskCatchToken obj)
            {

            }

            void IPoolable<FTaskCatchToken>.OnSet(FTaskCatchToken obj)
            {
                obj.token = null;
            }
        }


        public bool IsCompleted => false;
        public FTaskToken GetResult()
        {
            FTaskToken token = this.token;
            Pool.Set(this);
            return token;
        }

        /// <summary>
        /// 为任务设置令牌（不希望主动进行调用采用显实现）
        /// </summary>
        /// <param name="token"></param>
        [DebuggerHidden]
        void ITaskTokenHolder.SetToken(FTaskToken token)
        {
            this.token = token;
            if (token != null)
                continuation?.Invoke();
        }
        /// <summary>
        /// 获取任务令牌（可能为null，不希望主动进行调用采用显实现）
        /// </summary>
        [DebuggerHidden]

        FTaskToken ITaskTokenHolder.GetToken()
        {
            return token;
        }

        public Action continuation;
        void ICriticalNotifyCompletion.UnsafeOnCompleted(Action continuation)
        {
            this.continuation = continuation; 
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }
    }
}