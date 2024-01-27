using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FFramework
{
    public static partial class MessageExtensions
    {
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe(this IFEventOperator<ISendEvent> container, Action message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe<T1>(this IFEventOperator<ISendEvent<T1>> container, Action<T1> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]

        public static void UnSubscribe<T1, T2>(this IFEventOperator<ISendEvent<T1, T2>> container, Action<T1, T2> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe<T1, T2, T3>(this IFEventOperator<ISendEvent<T1, T2, T3>> container, Action<T1, T2, T3> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe<T1, T2, T3, T4>(this IFEventOperator<ISendEvent<T1, T2, T3, T4>> container, Action<T1, T2, T3, T4> message)
            => container.GetOperator().Events.Remove(message);
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe<T1, T2, T3, T4, T5>(this IFEventOperator<ISendEvent<T1, T2, T3, T4, T5>> container, Action<T1, T2, T3, T4, T5> message)
            => container.GetOperator().Events.Remove(message);
        //-----------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe<T1>(this IFEventOperator<ICallEvent<T1>> container, Func<T1> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe<T1, T2>(this IFEventOperator<ICallEvent<T1, T2>> container, Func<T1, T2> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe<T1, T2, T3>(this IFEventOperator<ICallEvent<T1, T2, T3>> container, Func<T1, T2, T3> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe<T1, T2, T3, T4>(this IFEventOperator<ICallEvent<T1, T2, T3, T4>> container, Func<T1, T2, T3, T4> message)
            => container.GetOperator().Events.Remove(message);


        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe<T1, T2, T3, T4, T5>(this IFEventOperator<ICallEvent<T1, T2, T3, T4, T5>> container, Func<T1, T2, T3, T4, T5> message)
            => container.GetOperator().Events.Remove(message);

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribe<T1, T2, T3, T4, T5, T6>(this IFEventOperator<ICallEvent<T1, T2, T3, T4, T5, T6>> container, Func<T1, T2, T3, T4, T5, T6> message)
            => container.GetOperator().Events.Remove(message);

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribeAll(this IFEventOperator<ISendEvent> container)
            => container.GetOperator().Clear();
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribeAll<T1>(this IFEventOperator<ISendEvent<T1>> container)
            => container.GetOperator().Clear();
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribeAll<T1, T2>(this IFEventOperator<ISendEvent<T1, T2>> container)
            => container.GetOperator().Clear();
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribeAll<T1, T2, T3>(this IFEventOperator<ISendEvent<T1, T2, T3>> container)
            => container.GetOperator().Clear();
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribeAll<T1, T2, T3, T4>(this IFEventOperator<ISendEvent<T1, T2, T3, T4>> container)
            => container.GetOperator().Clear();
        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UnSubscribeAll<T1, T2, T3, T4, T5>(this IFEventOperator<ISendEvent<T1, T2, T3, T4, T5>> container)
            => container.GetOperator().Clear();
    }
}
