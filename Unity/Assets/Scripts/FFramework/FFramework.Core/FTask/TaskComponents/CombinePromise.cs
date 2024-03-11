using System;
using System.Collections.Generic;
using System.Linq;

namespace FFramework
{
    public class CombinePromise : IUpdate
    {
        private bool started = false;
        private FTask task = null;
        int completeCount = 0;
        int needCount = 0;
        
        static long next_id = long.MinValue;
        long curId;


        void IUpdate.Update(float deltaTime)
        {
            if (!started) return;
            bool result = false;
            FTaskTokenStatus status = task.Token?.Status ?? FTaskTokenStatus.Pending;
            switch (status)
            {
                case FTaskTokenStatus.Pending:
                    if (completeCount >= needCount) result = true;
                    break;
                case FTaskTokenStatus.Yield:
                    break;
                case FTaskTokenStatus.Cancelled:
                    task.GetAwaiter().SetCanceled();
                    Pool.Set<CombinePromise, CombinePromisePoolable>(this);
                    return;
            }

            if (result)
            {
                task.GetAwaiter().SetResult();
                Pool.Set<CombinePromise, CombinePromisePoolable>(this);
            }
        }

        //结合封装委托
        static async FTask Combine(FTask countTask, CombinePromise promise, long curId)
        {
            await countTask;
            if (curId == promise.curId)
                promise.completeCount++;
        }
        /// <summary>
        /// 开始进行计时回调，如果时间小于0，则按照生命周期更新频率进行回调，Task如果为null则会抛出异常
        /// </summary>
        /// <param name="task"></param>
        /// <param name="span"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void CombineCall(FTask task, List<FTask> tasks, int needCount)
        {
            this.needCount = needCount;
            this.task = task ?? throw new ArgumentNullException(nameof(task));

            foreach (var item in tasks)
            {
                Combine(item, this, curId).Forget();
            }
            started = true;
        }


        public class CombinePromisePoolable : IPoolable<CombinePromise>
        {
            int IPoolable.Capacity => 1000;

            CombinePromise IPoolable<CombinePromise>.OnCreate()
            {
                return new CombinePromise();
            }

            void IPoolable<CombinePromise>.OnDestroy(CombinePromise obj)
            {

            }

            void IPoolable<CombinePromise>.OnGet(CombinePromise obj)
            {
                obj.curId = next_id++;
                obj.EnableUpdate();
            }

            void IPoolable<CombinePromise>.OnSet(CombinePromise obj)
            {
                obj.DisableUpdate();
                obj.started = false;
                obj.task = null;
                obj.needCount = 0;
                obj.completeCount = 0;

            }
        }
    }



}
