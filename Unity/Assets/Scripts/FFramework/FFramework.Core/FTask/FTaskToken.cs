using System;
using System.Runtime.ExceptionServices;
using UnityEngine;

namespace FFramework
{
    public interface ITaskTokenStatusSetter
    {
        /// <summary>
        /// 设置令牌状态
        /// </summary>
        /// <param name="status"></param>
        /// <param name="anyParam"></param>
        void SetStatus(FTaskTokenStatus status, object anyParam);
        /// <summary>
        /// 保持挂起状态
        /// </summary>
        /// <param name="moveNext"></param>
        void YieldHold(Action moveNext);
    }

    public class FTaskToken : FUnit, ITaskTokenStatusSetter
    {

    

        /// <summary>
        /// 令牌状态
        /// </summary>
        FTaskTokenStatus status = FTaskTokenStatus.Pending;

        /// <summary>
        /// 令牌状态
        /// </summary>
        public FTaskTokenStatus Status => status;


        public event Action OnContinue = null;

        public event Action OnYield = null;

        public event Action OnCancel = null;

        /// <summary>
        /// 取消任务（不允许对已取消的任务进行操作）
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Cancel()
        {
            if (status == FTaskTokenStatus.Cancelled)
                throw new InvalidOperationException($"Illegal suspension(from {status} to {FTaskTokenStatus.Yield})");

            status = FTaskTokenStatus.Cancelled;
            OnCancel?.Invoke();
        }
        /// <summary>
        /// 挂起任务（不允许对已挂起或者取消的任务进行操作）
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Yield()
        {
            if (status != FTaskTokenStatus.Pending)
                throw new InvalidOperationException($"Illegal suspension(from {status} to {FTaskTokenStatus.Yield})");

            status = FTaskTokenStatus.Yield;
            OnYield?.Invoke();
        }

        /// <summary>
        /// 继续任务（不允许对取消的，或者没有挂起的任务进行操作）
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Continue()
        {
            if (status != FTaskTokenStatus.Yield)
                throw new InvalidOperationException($"Illegal suspension(from {status} to {FTaskTokenStatus.Yield})");

            status = FTaskTokenStatus.Pending;
            YieldState?.Invoke();
            YieldState = null;
            OnContinue?.Invoke();
        }


        /// <summary>
        /// 设置令牌状态（不希望主动调用此方法，使用显实现屏蔽IDE提示）
        /// </summary>
        /// <param name="status"></param>
        /// <param name="anyParam"></param>
        void ITaskTokenStatusSetter.SetStatus(FTaskTokenStatus status, object anyParam)
        {
            this.status = status;
        }


        private Action YieldState = null;
        void ITaskTokenStatusSetter.YieldHold(Action moveNext)
        {
            YieldState = moveNext;
        }

        public class FTaskTokenPoolable : IPoolable<FTaskToken>
        {
            int IPoolable.Capacity => 1000;

            FTaskToken IPoolable<FTaskToken>.OnCreate()
            {
                return new FTaskToken();
            }

            void IPoolable<FTaskToken>.OnDestroy(FTaskToken obj)
            {

            }

            void IPoolable<FTaskToken>.OnGet(FTaskToken obj)
            {
     
            }

            void IPoolable<FTaskToken>.OnSet(FTaskToken obj)
            {
                obj.status = FTaskTokenStatus.Pending;
                obj.OnCancel = null;
                obj.OnYield = null;
                obj.OnContinue = null;
            }
        }
    }
}
