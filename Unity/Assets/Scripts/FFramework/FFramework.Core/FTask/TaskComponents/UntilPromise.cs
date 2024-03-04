using System;

namespace FFramework
{
    public class UntilPromise : IUpdate
    {
        private bool started = false;
        Func<bool> judge = null;
        private FTask task = null;


        void IUpdate.Update(float deltaTime)
        {
            if (!started) return;
            bool result = false;
            FTaskTokenStatus status = task.Token?.Status ?? FTaskTokenStatus.Pending;
            switch (status)
            {
                case FTaskTokenStatus.Pending:
                    result = judge();
                    break;
                case FTaskTokenStatus.Yield:
                    break;
                case FTaskTokenStatus.Cancelled:
                    task.GetAwaiter().SetCanceled();
                    Pool.Set<UntilPromise, UntilPromisePoolable>(this);
                    break;
            }

            if (result)
            {
                task.GetAwaiter().SetResult();
                Pool.Set<UntilPromise, UntilPromisePoolable>(this);
            }
        }

        /// <summary>
        /// 开始进行计时回调，如果时间小于0，则按照生命周期更新频率进行回调，Task如果为null则会抛出异常
        /// </summary>
        /// <param name="task"></param>
        /// <param name="span"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UntilCall(FTask task, Func<bool> Judge)
        {
            this.task = task ?? throw new ArgumentNullException(nameof(task));
            judge = Judge;
            started = true;
        }


        public class UntilPromisePoolable : IPoolable<UntilPromise>
        {
            int IPoolable.Capacity => 1000;

            UntilPromise IPoolable<UntilPromise>.OnCreate()
            {
                return new UntilPromise();
            }

            void IPoolable<UntilPromise>.OnDestroy(UntilPromise obj)
            {

            }

            void IPoolable<UntilPromise>.OnGet(UntilPromise obj)
            {
                obj.EnableUpdate();
            }

            void IPoolable<UntilPromise>.OnSet(UntilPromise obj)
            {
                obj.DisableUpdate();
                obj.started = false;
                obj.task = null;
               
            }
        }
    }
}
