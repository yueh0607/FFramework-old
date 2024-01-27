using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FFramework
{
    public static partial class MessageExtensions
    {
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe(this IFEventOperator<ISendEvent> container, Action message)
            => container.GetOperator().Events.Add(message);


        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1>(this IFEventOperator<ISendEvent<T1>> container, Action<T1> message)
            => container.GetOperator().Events.Add(message);


        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1, T2>(this IFEventOperator<ISendEvent<T1, T2>> container, Action<T1, T2> message)
            => container.GetOperator().Events.Add(message);
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1, T2, T3>(this IFEventOperator<ISendEvent<T1, T2, T3>> container, Action<T1, T2, T3> message)
            => container.GetOperator().Events.Add(message);
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>

        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1, T2, T3, T4>(this IFEventOperator<ISendEvent<T1, T2, T3, T4>> container, Action<T1, T2, T3, T4> message)
            => container.GetOperator().Events.Add(message);
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>



        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1, T2, T3, T4, T5>(this IFEventOperator<ISendEvent<T1, T2, T3, T4, T5>> container, Action<T1, T2, T3, T4, T5> message)
            => container.GetOperator().Events.Add(message);

        //---------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1>(this IFEventOperator<ICallEvent<T1>> container, Func<T1> message)
            => container.GetOperator().Events.Add(message);


        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1, T2>(this IFEventOperator<ICallEvent<T1, T2>> container, Func<T1, T2> message)
            => container.GetOperator().Events.Add(message);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1, T2, T3>(this IFEventOperator<ICallEvent<T1, T2, T3>> container, Func<T1, T2, T3> message)
            => container.GetOperator().Events.Add(message);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1, T2, T3, T4>(this IFEventOperator<ICallEvent<T1, T2, T3, T4>> container, Func<T1, T2, T3, T4> message)
            => container.GetOperator().Events.Add(message);


        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1, T2, T3, T4, T5>(this IFEventOperator<ICallEvent<T1, T2, T3, T4, T5>> container, Func<T1, T2, T3, T4, T5> message)
            => container.GetOperator().Events.Add(message);


        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="container"></param>
        /// <param name="message"></param>
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subscribe<T1, T2, T3, T4, T5, T6>(this IFEventOperator<ICallEvent<T1, T2, T3, T4, T5, T6>> container, Func<T1, T2, T3, T4, T5, T6> message)
            => container.GetOperator().Events.Add(message);
    }
}
