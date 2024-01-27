namespace FFramework
{
    public interface IPoolable
    {
        /// <summary>
        /// 对象池最大容量
        /// </summary>
        int Capacity { get; }

    }

    public interface IPoolable<T> : IPoolable
    {


        /// <summary>
        /// 创建时调用
        /// </summary>
        /// <returns></returns>
        T OnCreate();



        /// <summary>
        /// 获取时调用
        /// </summary>
        /// <param name="obj"></param>
        void OnGet(T obj);

        /// <summary>
        /// 放回时调用
        /// </summary>
        /// <param name="obj"></param>
        void OnSet(T obj);

        /// <summary>
        /// 销毁时调用
        /// </summary>
        /// <param name="obj"></param>
        void OnDestroy(T obj);
    }
}

