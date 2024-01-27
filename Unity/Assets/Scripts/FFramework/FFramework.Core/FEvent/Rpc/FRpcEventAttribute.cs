using FFramework.Internal;
using System;


namespace FFramework.Network
{
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class FRpcEventAttribute : Attribute
    {
        /// <summary>
        /// 判断是否为事件
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsRpcEvent(Type type)
        {
            return typeof(IFEventBase).IsAssignableFrom(type);
        }
    }

}
