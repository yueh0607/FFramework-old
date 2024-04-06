using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FFramework
{
    public partial class FTask
    {
        private static readonly FTaskCompleted completed = new FTaskCompleted();

        /// <summary>
        /// 已经完成的任务
        /// </summary>
        public static FTaskCompleted CompletedTask => completed;

        /// <summary>
        /// 推迟指定秒
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        public static FTask Delay(float seconds)
        {
            return Delay(TimeSpan.FromSeconds(seconds));
        }

        /// <summary>
        /// 推迟一定时间
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        public static FTask Delay(TimeSpan span)
        {
            DelayPromise promise = Pool.Get<DelayPromise, DelayPromise.DelayPromisePoolable>();
            FTask task = Pool.Get<FTask, FTask.FTaskPoolable>();

            promise.TimeCall(task, span);
            return task;
        }

        /// <summary>
        /// 推迟X帧，1为等待当前帧结束
        /// </summary>
        /// <param name="frameCount"></param>
        /// <returns></returns>
        public static async FTask DelayFrame(int frameCount = 1)
        {
            await FTask.CompletedTask;
            for (int i = 0; i < frameCount; i++)
            {
                await Delay(TimeSpan.Zero);
            }
        }

        /// <summary>
        /// 捕获本流程令牌
        /// </summary>
        /// <returns></returns>
        public static FTaskCatchToken CatchToken()
        {
            return Pool.Get<FTaskCatchToken, FTaskCatchToken.FTaskCatchTokenPoolable>();
        }
    

        /// <summary>
        /// Task转入FTask，非必要不要使用，会使得FTask任务链的挂起功能受到损失（例如在执行此任务时不会暂停）
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
                    if (source != null) fTask.Token.OnCancel += source.Cancel;
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
        /// <summary>
        /// 等待判别式为true
        /// </summary>
        /// <param name="judge"></param>
        /// <returns></returns>
        public static FTask Until(Func<bool> judge)
        {
            UntilPromise promise = Pool.Get<UntilPromise, UntilPromise.UntilPromisePoolable>();
            FTask task = Pool.Get<FTask, FTask.FTaskPoolable>();

            promise.UntilCall(task, judge);
            return task;
        }
        /// <summary>
        /// 等待全部完成
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static FTask WaitAll(List<FTask> tasks)
        {
            CombinePromise promise = Pool.Get<CombinePromise, CombinePromise.CombinePromisePoolable>();
            FTask task = Pool.Get<FTask, FTask.FTaskPoolable>();

            promise.CombineCall(task, tasks, tasks.Count);
            return task;
        }
        /// <summary>
        /// 等待任何一个完成
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static FTask WaitAny(List<FTask> tasks)
        {
            if (tasks.Count == 0) throw new InvalidOperationException("Wait any count=0");
            CombinePromise promise = Pool.Get<CombinePromise, CombinePromise.CombinePromisePoolable>();
            FTask task = Pool.Get<FTask, FTask.FTaskPoolable>();

            promise.CombineCall(task, tasks, 1);
            return task;
        }
        /// <summary>
        /// 等待全部任务完成
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static FTask WaitAll(params FTask[] tasks)
        {
            return WaitAll(tasks.ToList());
        }
        /// <summary>
        /// 等待任意一个任务完成
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        public static FTask WaitAny(params FTask[] tasks)
        {
            return WaitAny(tasks.ToList());
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
        /// 令FTask在后台以协程方式运行(开启未运行的task)
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

        /// <summary>
        /// 设置令牌
        /// </summary>
        /// <param name="task"></param>
        /// <param name="token"></param>
        public static void SetToken(this FTask task , FTaskToken token)
        {
            ((ITaskTokenHolder)task).SetToken(token);
            IChildTaskHolder deepestTask = task.GetAwaiter();
            while(true)
            {
                var nextLayerTask = deepestTask.GetChildTask();
                if (nextLayerTask is IChildTaskHolder nextLayerHolder)
                    deepestTask = nextLayerHolder;
                else break;
            }
            deepestTask.GetChildTask().SetToken(token);
        }
    }
}
