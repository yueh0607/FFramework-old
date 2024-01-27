namespace FFramework
{
    public enum FTaskTokenStatus : byte
    {
        /// <summary>
        /// 表示任务正常执行
        /// </summary>
        Pending = 0,
        /// <summary>
        /// 表示任务需要挂起
        /// </summary>
        Yield = 1,
        /// <summary>
        /// 表示任务需要取消
        /// </summary>
        Cancelled = 2,
        /// <summary>
        /// 表示任务成功完成
        /// </summary>
        Success = 3,
        /// <summary>
        /// 表示任务失败,通常会将异常信息存储在令牌中
        /// </summary>
        Faulted = 4,
    }

    public static class FTaskTokenStatusHelper
    {
        public static bool IsCompleted(this FTaskTokenStatus status)
        {
            if (status == FTaskTokenStatus.Cancelled ||
                status == FTaskTokenStatus.Success ||
                status == FTaskTokenStatus.Faulted)
                return true;
            return false;
        }
        public static bool IsYield(this FTaskTokenStatus status)
        {
            if (status == FTaskTokenStatus.Yield)
                return true;
            return false;
        }
        public static bool IsPending(this FTaskTokenStatus status)
        {
            if (status == FTaskTokenStatus.Pending)
                return true;
            return false;
        }
        public static bool IsSuccess(this FTaskTokenStatus status)
        {
            if (status == FTaskTokenStatus.Success)
                return true;
            return false;
        }

        public static bool IsFaulted(this FTaskTokenStatus status)
        {
            if (status == FTaskTokenStatus.Faulted)
                return true;
            return false;
        }

        public static bool IsCancelled(this FTaskTokenStatus status)
        {
            if (status == FTaskTokenStatus.Cancelled)
                return true;
            return false;
        }



    }
}