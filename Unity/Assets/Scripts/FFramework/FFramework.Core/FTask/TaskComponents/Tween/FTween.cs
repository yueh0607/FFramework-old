using System;

namespace FFramework
{




    public partial class FTween<T> : IUpdate
    {
        private ILerper<T> lerper;
        private bool started = false;
        private T start;
        private T end;
        private T currentValue;
        private FTask task;
        private float duration = 0;
        private float currentTime = 0;


        private FTween()
        {

        }


        /// <summary>
        /// 回调设置器
        /// </summary>
        public event Action<T> Process = null;

        /// <summary>
        /// 当前的插值结果
        /// </summary>
        public T CurrentValue => currentValue;


        /// <summary>
        /// 限制Current在0-duration之间的Setter，表示当前时间
        /// </summary>
        private float CurrentTime
        {
            get => currentTime;
            set
            {
                currentTime = Math.Clamp(value, 0, duration);
            }
        }

        /// <summary>
        /// 步进
        /// </summary>
        /// <param name="stepValue"></param>
        void IUpdate.Update(float deltaTime)
        {
            if (!started) return;

            switch (task.Token.Status)
            {
                case FTaskTokenStatus.Pending:
                    CurrentTime += deltaTime;
                    break;
                case FTaskTokenStatus.Yield:
                    break;
                case FTaskTokenStatus.Cancelled:
                    task.GetAwaiter().SetCanceled();
                    Pool.Set<FTween<T>, FTween<T>.FTweenPoolable>(this);
                    break;
            }

            currentValue = lerper.LerpClamp(start, end, CurrentTime / duration);
            Process?.Invoke(currentValue);
            if (lerper.ValueEqual(CurrentValue, end))
            {
                task.GetAwaiter().SetResult();
                Pool.Set<FTween<T>, FTween<T>.FTweenPoolable>(this);
                return;
            }
        }

        /// <summary>
        /// 开始过渡
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="duration"></param>
        /// <param name="lerper"></param>
        /// <exception cref="NullReferenceException"></exception>
        public void StartCross(T start, T end, float duration, ILerper<T> lerper, FTask task, Action<T> process)
        {
            this.task = task;
            this.start = start;
            this.end = end;
            if (process != null) Process += process;
            this.duration = duration;
            this.lerper = lerper ?? throw new NullReferenceException("Lerper is null");
            started = true;
        }


        public class FTweenPoolable : IPoolable<FTween<T>>
        {
            int IPoolable.Capacity => 100;

            FTween<T> IPoolable<FTween<T>>.OnCreate()
            {
                return new FTween<T>();
            }

            void IPoolable<FTween<T>>.OnDestroy(FTween<T> obj)
            {

            }

            void IPoolable<FTween<T>>.OnGet(FTween<T> obj)
            {
                obj.EnableUpdate();
            }

            void IPoolable<FTween<T>>.OnSet(FTween<T> obj)
            {
                obj.DisableUpdate();
                obj.started = false;
                obj.currentTime = 0;
                obj.Process = null;
                obj.lerper = null;
                obj.start = default;
                obj.end = default;
                obj.currentValue = default;
                obj.duration = 0;

            }
        }
    }
}
