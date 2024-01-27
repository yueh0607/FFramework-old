namespace FFramework
{
    public interface ILerper<T>
    {
        /// <summary>
        /// 返回介于start和end对t的插值(允许返回值超过start和end) ，t在[0,1]之间
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        T Lerp(T start, T end, float t);

        /// <summary>
        /// 要求返回值必须在strat和end之间的插值函数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        T LerpClamp(T start, T end, float t);


        /// <summary>
        /// 判断值是否相等
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        bool ValueEqual(T v1, T v2);
    }
}
