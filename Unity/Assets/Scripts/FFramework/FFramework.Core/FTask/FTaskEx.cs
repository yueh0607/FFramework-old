using System;
using System.Threading;
using System.Threading.Tasks;

namespace FFramework
{
    public partial class FTask
    {
        private static readonly FTaskCompleted completed = new FTaskCompleted();
        public static FTaskCompleted CompletedTask => completed;

        public static FTask Delay(float seconds)
        {
            return Delay(TimeSpan.FromSeconds(seconds));
        }

        public static FTask Delay(TimeSpan span)
        {
            DelayPromise promise = Pool.Get<DelayPromise, DelayPromise.DelayPromisePoolable>();
            FTask task = Pool.Get<FTask, FTask.FTaskPoolable>();

            promise.TimeCall(task, span);
            return task;
        }

        public static async FTask DelayFrame(int frameCount = 1)
        {
            await FTask.CompletedTask;
            for (int i = 0; i < frameCount; i++)
            {
                await Delay(TimeSpan.Zero);
            }
        }

        /// <summary>
        /// Task转入FTask，非必要不要使用，会使得FTask任务链的挂起功能受到损失（例如在执行此任务时不会暂停）
        /// 但是不影响取消功能，如果可以保证使用它的任务链不需要暂停，或者执行它的时候不会暂停，就可以完整支持挂起功能。
        /// </summary>
        /// <param name="task"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static FTask ToFTask(Task task, CancellationTokenSource source)
        {
            SynchronizationContext context = SynchronizationContext.Current;
            FTask fTask = Pool.Get<FTask, FTask.FTaskPoolable>();

            context.Post(async (state) =>
            {
                try
                {
                    await task;
                    context.Post(_ =>
                    {
                        fTask.awaiter.SetResult();
                    }, null);
                }
                catch (TaskCanceledException)
                {
                    fTask.GetAwaiter().SetCanceled();
                }
                catch (Exception e)
                {
                    fTask.GetAwaiter().SetException(e);
                }
                finally
                {
                    source.Dispose();
                }
            }, null);

            return fTask;
        }

        /// <summary>
        /// 值从start在duration时间内过度到end,Lerper可以使用FloatLerper.Instance或者手动new，
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="duration"></param>
        /// <param name="lerper"></param>
        /// <returns></returns>
        public static FTask Cross<T>(T start, T end, float duration, ILerper<T> lerper, Action<T> process = null)
        {
            var tween = Pool.Get<FTween<T>, FTween<T>.FTweenPoolable>();
            FTask task = Pool.Get<FTask, FTask.FTaskPoolable>();
            tween.StartCross(start, end, duration, lerper, task, process);
            return task;
        }
    }

    public static class FTaskThisEx
    {
        /// <summary>
        /// 消除波浪线警告
        /// </summary>
        /// <param name="task"></param>
        public static void Forget(this FTask task)
        {
            // do nothing..
        }
        /// <summary>
        /// 消除波浪线警告
        /// </summary>
        /// <param name="task"></param>
        public static void Forget<T>(this FTask<T> task)
        {
            // do nothing..
        }

        /// <summary>
        /// 令FTask在后台以协程方式运行
        /// </summary>
        /// <param name="task"></param>
        public static void Coroutine(this FTask task)
        {
            async FTask DoAsync()
            {
                await task;
            }
            DoAsync().Forget();
        }
        /// <summary>
        /// 令FTask在后台以协程方式运行
        /// </summary>
        /// <param name="task"></param>
        public static void Coroutine<T>(this FTask<T> task)
        {
            async FTask DoAsync()
            {
                await task;
            }
            DoAsync().Forget();
        }
    }
}
