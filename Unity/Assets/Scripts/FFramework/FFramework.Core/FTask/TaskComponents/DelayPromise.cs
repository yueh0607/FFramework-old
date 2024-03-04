using System;

namespace FFramework
{
    public class DelayPromise : IUpdate
    {
        private bool started = false;
        private double seconds = 0;
        private double current = 0;
        private FTask task = null;


        void IUpdate.Update(float deltaTime)
        {
            if (!started) return;

            FTaskTokenStatus status = task.Token?.Status ?? FTaskTokenStatus.Pending;
            switch (status)
            {
                case FTaskTokenStatus.Pending:
                    current += deltaTime;
                    break;
                case FTaskTokenStatus.Yield:
                    break;
                case FTaskTokenStatus.Cancelled:
                    task.GetAwaiter().SetCanceled();
                    Pool.Set<DelayPromise, DelayPromisePoolable>(this);
                    break;
            }

            if (current >= seconds)
            {
                task.GetAwaiter().SetResult();
                Pool.Set<DelayPromise, DelayPromisePoolable>(this);
            }
        }

        /// <summary>
        /// 开始进行计时回调，如果时间小于0，则按照生命周期更新频率进行回调，Task如果为null则会抛出异常
        /// </summary>
        /// <param name="task"></param>
        /// <param name="span"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void TimeCall(FTask task, TimeSpan span)
        {
            this.task = task ?? throw new ArgumentNullException(nameof(task));
            seconds = span.TotalSeconds;
            seconds = Math.Clamp(seconds, 0, double.MaxValue);
            started = true;
        }


        public class DelayPromisePoolable : IPoolable<DelayPromise>
        {
            int IPoolable.Capacity => 1000;

            DelayPromise IPoolable<DelayPromise>.OnCreate()
            {
                return new DelayPromise();
            }

            void IPoolable<DelayPromise>.OnDestroy(DelayPromise obj)
            {

            }

            void IPoolable<DelayPromise>.OnGet(DelayPromise obj)
            {
                obj.EnableUpdate();
            }

            void IPoolable<DelayPromise>.OnSet(DelayPromise obj)
            {
                obj.DisableUpdate();
                obj.started = false;
                obj.task = null;
                obj.current = 0;
                obj.seconds = 0;
            }
        }
    }
}
