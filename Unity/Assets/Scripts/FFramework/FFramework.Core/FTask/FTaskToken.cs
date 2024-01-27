using System;
using System.Runtime.ExceptionServices;

namespace FFramework
{
    public interface ITaskTokenStatusSetter
    {
        void SetStatus(FTaskTokenStatus status, object anyParam);
    }

    public class FTaskToken : ITaskTokenStatusSetter
    {
        bool inPool = false;
        private FTaskToken() { }
        ~FTaskToken()
        {
            if (!inPool)
            {
                throw new InvalidOperationException("FTaskToken must be recycled.");
            }
        }


        public static Action<ExceptionDispatchInfo> ErrorHandler = null;

        FTaskTokenStatus status = FTaskTokenStatus.Pending;

        /// <summary>
        /// 令牌预期状态
        /// </summary>
        public FTaskTokenStatus Status => status;

        private ExceptionDispatchInfo failedLog = null;

        /// <summary>
        /// 如果Status为Faulted，此属性将包含异常信息，否则为null
        /// </summary>
        public ExceptionDispatchInfo FailedException => failedLog;

        /// <summary>
        /// 取消任务（不允许对已取消的任务进行操作）
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void Cancel()
        {
            if (status == FTaskTokenStatus.Cancelled)
                throw new InvalidOperationException($"Illegal suspension(from {status} to {FTaskTokenStatus.Yield})");

            status = FTaskTokenStatus.Cancelled;
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
        }



        /// <summary>
        /// 设置令牌状态（不希望主动调用此方法，使用显实现屏蔽IDE提示）
        /// </summary>
        /// <param name="status"></param>
        /// <param name="anyParam"></param>
        void ITaskTokenStatusSetter.SetStatus(FTaskTokenStatus status, object anyParam)
        {
            if (status == FTaskTokenStatus.Faulted)
            {
                failedLog = anyParam as ExceptionDispatchInfo;
                ErrorHandler?.Invoke(failedLog);
            }
            this.status = status;
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
                obj.inPool = false;
            }

            void IPoolable<FTaskToken>.OnSet(FTaskToken obj)
            {
                obj.inPool = true;
                obj.status = FTaskTokenStatus.Pending;
                obj.failedLog = null;
            }
        }
    }
}
