using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;


namespace FFramework
{

    public class FTaskAwaiter : ICriticalNotifyCompletion, IReset, ITaskTokenProperty
    {
        private Action continuation = null;
        private readonly FTask task = null;
        [DebuggerHidden]
        public FTaskAwaiter(in FTask task)
        {
            this.task = task;
        }
        // / <summary>
        /// 在异步链的某一流程完成后调用（此方法将由编译器生成到代码）
        /// </summary>
        /// <param name="continuation"></param>
        [DebuggerHidden]
        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        /// <summary>
        /// 在异步链的某一流程完成后调用（此方法将由编译器生成到代码）
        /// </summary>
        /// <param name="continuation"></param>
        [DebuggerHidden]
        public void UnsafeOnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }

        /// <summary>
        /// 标识任务是否已经完成（此属性将由编译器生成到代码）
        /// </summary>
        [DebuggerHidden]
        public bool IsCompleted => isCompleted;

        private bool isCompleted = false;

        /// <summary>
        /// 用于返回Task执行的结果（注意此方法将由编译器生成到代码，不需要开发者进行调用）
        /// </summary>
        [DebuggerHidden]
        public void GetResult()
        {

        }

        /// <summary>
        /// 设置任务的结果为成功并完成任务
        /// </summary>
        [DebuggerHidden]
        public void SetResult()
        {
            isCompleted = true;
            continuation?.Invoke();
            if (task.Token != null)
                ((ITaskTokenStatusSetter)task.Token).SetStatus(FTaskTokenStatus.Success, null);
            Pool.Set<FTask, FTask.FTaskPoolable>(task);
        }

        /// <summary>
        /// 设置任务的结果为异常并完成
        /// </summary>
        /// <param name="exception"></param>
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            ExceptionDispatchInfo e = ExceptionDispatchInfo.Capture(exception);
            if (task.Token != null)
                ((ITaskTokenStatusSetter)task.Token).SetStatus(FTaskTokenStatus.Faulted, e);
        }
        /// <summary>
        /// 设置任务结果为取消并完成
        /// </summary>
        [DebuggerHidden]
        public void SetCanceled()
        {
            isCompleted = true;
            if (task.Token != null)
                ((ITaskTokenStatusSetter)task.Token).SetStatus(FTaskTokenStatus.Cancelled, null);
            Pool.Set<FTask, FTask.FTaskPoolable>(task);
        }

        /// <summary>
        /// 重置Awaiter
        /// </summary>
        [DebuggerHidden]
        void IReset.Reset()
        {
            continuation = null;
            isCompleted = false;
        }


        /// <summary>
        /// 为任务设置令牌（不希望主动进行调用采用显实现）
        /// </summary>
        /// <param name="token"></param>
        [DebuggerHidden]
        void ITaskTokenProperty.SetToken(FTaskToken token)
        {
            ((ITaskTokenProperty)task).SetToken(token);
        }
        /// <summary>
        /// 获取任务令牌（可能为null，不希望主动进行调用采用显实现）
        /// </summary>
        [DebuggerHidden]
        FTaskToken ITaskTokenProperty.GetToken()
        {
            return ((ITaskTokenProperty)task).GetToken();
        }

    }

    public class FTaskAwaiter<T> : ICriticalNotifyCompletion, IReset, ITaskTokenProperty
    {
        private Action continuation = null;
        private readonly FTask<T> task = null;
        [DebuggerHidden]
        public FTaskAwaiter(in FTask<T> task)
        {
            this.task = task;
        }
        // / <summary>
        /// 在异步链的某一流程完成后调用（此方法将由编译器生成到代码）
        /// </summary>
        /// <param name="continuation"></param>
        [DebuggerHidden]
        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        /// <summary>
        /// 在异步链的某一流程完成后调用（此方法将由编译器生成到代码）
        /// </summary>
        /// <param name="continuation"></param>
        [DebuggerHidden]
        public void UnsafeOnCompleted(Action continuation)
        {
            this.continuation = continuation;
        }

        /// <summary>
        /// 标识任务是否已经完成（此属性将由编译器生成到代码）
        /// </summary>
        [DebuggerHidden]
        public bool IsCompleted => isCompleted;

        private bool isCompleted = false;


        private T result = default;
        /// <summary>
        /// 用于返回Task执行的结果（注意此方法将由编译器生成到代码，不需要开发者进行调用）
        /// </summary>
        [DebuggerHidden]
        public T GetResult()
        {
            return result;
        }

        /// <summary>
        /// 设置任务的结果为成功并完成任务
        /// </summary>
        [DebuggerHidden]
        public void SetResult(T result)
        {
            this.result = result;
            isCompleted = true;
            continuation?.Invoke();
            if (task.Token != null)
                ((ITaskTokenStatusSetter)task.Token).SetStatus(FTaskTokenStatus.Success, null);
            Pool.Set<FTask<T>, FTask<T>.FTaskPoolable>(task);
        }

        /// <summary>
        /// 设置任务的结果为异常并完成
        /// </summary>
        /// <param name="exception"></param>
        [DebuggerHidden]
        public void SetException(Exception exception)
        {
            ExceptionDispatchInfo e = ExceptionDispatchInfo.Capture(exception);
            if (task.Token != null)
                ((ITaskTokenStatusSetter)task.Token).SetStatus(FTaskTokenStatus.Faulted, e);
        }
        /// <summary>
        /// 设置任务结果为取消并完成
        /// </summary>
        [DebuggerHidden]
        public void SetCancelled()
        {
            isCompleted = true;
            if (task.Token != null)
                ((ITaskTokenStatusSetter)task.Token).SetStatus(FTaskTokenStatus.Cancelled, null);
            Pool.Set<FTask<T>, FTask<T>.FTaskPoolable>(task);
        }

        /// <summary>
        /// 重置Awaiter
        /// </summary>
        [DebuggerHidden]
        void IReset.Reset()
        {
            result = default;
            continuation = null;
            isCompleted = false;
        }


        /// <summary>
        /// 为任务设置令牌（不希望主动进行调用采用显实现）
        /// </summary>
        /// <param name="token"></param>
        [DebuggerHidden]
        void ITaskTokenProperty.SetToken(FTaskToken token)
        {
            ((ITaskTokenProperty)task).SetToken(token);
        }
        /// <summary>
        /// 获取任务令牌（可能为null，不希望主动进行调用采用显实现）
        /// </summary>
        [DebuggerHidden]
        FTaskToken ITaskTokenProperty.GetToken()
        {
            return ((ITaskTokenProperty)task).GetToken();
        }

    }
}