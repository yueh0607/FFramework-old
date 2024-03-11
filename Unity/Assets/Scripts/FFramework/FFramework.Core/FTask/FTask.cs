
using System.Diagnostics;

using System.Runtime.CompilerServices;

namespace FFramework
{
    public interface ITaskTokenProperty
    {
        /// <summary>
        /// 为对象设置令牌
        /// </summary>
        /// <param name="token"></param>
        void SetToken(FTaskToken token);
        /// <summary>
        /// 获取对象令牌（可能为null）
        /// </summary>
        /// <returns></returns>
        FTaskToken GetToken();
    }


    [AsyncMethodBuilder(typeof(Internal.FTaskMethodBuilder))]
    public partial class FTask :FUnit, ITaskTokenProperty
    {
        //等待器
        private readonly FTaskAwaiter awaiter;

        private FTaskToken token = null;

        /// <summary>
        /// 任务令牌，用于标识任务链的状态以及进行操作
        /// </summary>
        public FTaskToken Token => token;

        public FTask()
        {
            awaiter = new FTaskAwaiter(this);
        }


        /// <summary>
        /// 获取Awaiter，此方法同时为await FTask提供支持
        /// </summary>
        /// <returns></returns>
        public FTaskAwaiter GetAwaiter() => awaiter;


        /// <summary>
        /// 对象池资源类，用于配置对象在池内的行为
        /// </summary>
        public class FTaskPoolable : IPoolable<FTask>
        {
            int IPoolable.Capacity => 1000;

            FTask IPoolable<FTask>.OnCreate()
            {
                return new FTask();
            }

            void IPoolable<FTask>.OnDestroy(FTask obj)
            {

            }

            void IPoolable<FTask>.OnGet(FTask obj)
            {

            }

            void IPoolable<FTask>.OnSet(FTask obj)
            {
                obj.token = null;
                ((IReset)obj.awaiter).Reset();

            }
        }


        /// <summary>
        /// 为任务设置令牌（不希望主动进行调用采用显实现）
        /// </summary>
        /// <param name="token"></param>
        [DebuggerHidden]
        void ITaskTokenProperty.SetToken(FTaskToken token)
        {
            this.token = token;
        }
        /// <summary>
        /// 获取任务令牌（可能为null，不希望主动进行调用采用显实现）
        /// </summary>
        [DebuggerHidden]

        FTaskToken ITaskTokenProperty.GetToken()
        {
            return token;
        }

    }




    [AsyncMethodBuilder(typeof(Internal.FTaskMethodBuilder<>))]
    public partial class FTask<T> : FUnit, ITaskTokenProperty
    {
        //等待器
        private readonly FTaskAwaiter<T> awaiter;

        private FTaskToken token = null;

        /// <summary>
        /// 任务令牌，用于标识任务链的状态以及进行操作
        /// </summary>
        public FTaskToken Token => token;

        public FTask()
        {
            awaiter = new FTaskAwaiter<T>(this);
        }


        /// <summary>
        /// 获取Awaiter，此方法同时为await FTask提供支持
        /// </summary>
        /// <returns></returns>
        public FTaskAwaiter<T> GetAwaiter() => awaiter;


        /// <summary>
        /// 对象池资源类，用于配置对象在池内的行为
        /// </summary>
        public class FTaskPoolable : IPoolable<FTask<T>>
        {
            int IPoolable.Capacity => 1000;

            FTask<T> IPoolable<FTask<T>>.OnCreate()
            {
                return new FTask<T>();
            }

            void IPoolable<FTask<T>>.OnDestroy(FTask<T> obj)
            {

            }

            void IPoolable<FTask<T>>.OnGet(FTask<T> obj)
            {

            }

            void IPoolable<FTask<T>>.OnSet(FTask<T> obj)
            {
                obj.token = null;
                ((IReset)obj.awaiter).Reset();

            }
        }


        /// <summary>
        /// 为任务设置令牌（不希望主动进行调用采用显实现）
        /// </summary>
        /// <param name="token"></param>
        [DebuggerHidden]
        void ITaskTokenProperty.SetToken(FTaskToken token)
        {
            this.token = token;
        }
        /// <summary>
        /// 获取任务令牌（可能为null，不希望主动进行调用采用显实现）
        /// </summary>
        [DebuggerHidden]
        FTaskToken ITaskTokenProperty.GetToken()
        {
            return token;
        }
    }
}