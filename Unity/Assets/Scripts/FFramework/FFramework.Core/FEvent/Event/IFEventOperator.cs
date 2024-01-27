namespace FFramework
{

    /// <summary>
    ///此接口用于支持内存级别的类型安全转换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFEventOperator<out T>
    {
        //禁止在这写任何内容！否则可能引发程序崩溃。
    }


}
