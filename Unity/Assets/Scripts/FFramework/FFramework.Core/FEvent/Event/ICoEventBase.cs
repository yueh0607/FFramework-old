using FFramework.Internal;

//表明FFramework中不愿意公开但却不得不公开的部分内容，如非必要请勿使用
namespace FFramework.Internal
{

    /// <summary>
    /// 非必要慎用：事件基础接口，无法处理事件，仅在事件系统内用作表示事件
    /// </summary>
    public interface IFEventBase { }



    /// <summary>
    /// 非必要慎用：用于标识事件参数类型和返回值类型的接口，但是无法标识调用类型
    /// </summary>
    public interface IGenericEvent : IFEventBase { }
    /// <summary>
    /// 非必要慎用：用于标识事件参数类型和返回值类型的接口，但是无法标识调用类型
    /// </summary>
    public interface IGenericEvent<T1> : IFEventBase { }
    /// <summary>
    /// 非必要慎用：用于标识事件参数类型和返回值类型的接口，但是无法标识调用类型
    /// </summary>
    public interface IGenericEvent<T1, T2> : IFEventBase { }
    /// <summary>
    /// 非必要慎用：用于标识事件参数类型和返回值类型的接口，但是无法标识调用类型
    /// </summary>
    public interface IGenericEvent<T1, T2, T3> : IFEventBase { }
    /// <summary>
    /// 非必要慎用：用于标识事件参数类型和返回值类型的接口，但是无法标识调用类型
    /// </summary>
    public interface IGenericEvent<T1, T2, T3, T4> : IFEventBase { }
    /// <summary>
    /// 非必要慎用：用于标识事件参数类型和返回值类型的接口，但是无法标识调用类型
    /// </summary>
    public interface IGenericEvent<T1, T2, T3, T4, T5> : IFEventBase { }
    /// <summary>
    /// 非必要慎用：用于标识事件参数类型和返回值类型的接口，但是无法标识调用类型
    /// </summary>
    public interface IGenericEvent<T1, T2, T3, T4, T5, T6> : IFEventBase { }


    /// <summary>
    /// 非必要慎用：用于标识事件调用类型的接口，不能表示参数类型和返回值类型
    /// </summary>
    public interface ISendEventBase : IFEventBase { }

    /// <summary>
    /// 非必要慎用：用于标识事件调用类型的接口，不能表示参数类型和返回值类型
    /// </summary>
    public interface ICallEventBase : IFEventBase { }
}


namespace FFramework
{
#pragma warning disable

    /// <summary>
    /// 用于标识无返回值的事件类型，泛型参数类型代表事件参数类型
    /// </summary>
    public interface ISendEvent : ISendEventBase, IGenericEvent { }
    /// <summary>
    /// 用于标识无返回值的事件类型，泛型参数类型代表事件参数类型
    /// </summary>
    public interface ISendEvent<T1> : ISendEventBase, IGenericEvent<T1> { }
    /// <summary>
    /// 用于标识无返回值的事件类型，泛型参数类型代表事件参数类型
    /// </summary>
    public interface ISendEvent<T1, T2> : ISendEventBase, IGenericEvent<T1, T2> { }
    /// <summary>
    /// 用于标识无返回值的事件类型，泛型参数类型代表事件参数类型
    /// </summary>
    public interface ISendEvent<T1, T2, T3> : ISendEventBase, IGenericEvent<T1, T2, T3> { }
    /// <summary>
    /// 用于标识无返回值的事件类型，泛型参数类型代表事件参数类型
    /// </summary>
    public interface ISendEvent<T1, T2, T3, T4> : ISendEventBase, IGenericEvent<T1, T2, T3, T4> { }
    /// <summary>
    /// 用于标识无返回值的事件类型，泛型参数类型代表事件参数类型
    /// </summary>
    public interface ISendEvent<T1, T2, T3, T4, T5> : ISendEventBase, IGenericEvent<T1, T2, T3, T4, T5> { }
    /// <summary>
    /// 用于标识无返回值的事件类型，泛型参数类型代表事件参数类型
    /// </summary>
    public interface ISendEvent<T1, T2, T3, T4, T5, T6> : ISendEventBase, IGenericEvent<T1, T2, T3, T4, T5, T6> { }

    /// <summary>
    /// 用于标识有返回值的事件类型，泛型参数类型代表事件参数类型以及返回值类型
    /// </summary>
    public interface ICallEvent : ICallEventBase, IGenericEvent { }
    /// <summary>
    /// 用于标识有返回值的事件类型，泛型参数类型代表事件参数类型以及返回值类型
    /// </summary>
    public interface ICallEvent<T1> : ICallEventBase, IGenericEvent<T1> { }
    /// <summary>
    /// 用于标识有返回值的事件类型，泛型参数类型代表事件参数类型以及返回值类型
    /// </summary>
    public interface ICallEvent<T1, T2> : ICallEventBase, IGenericEvent<T1, T2> { }
    /// <summary>
    /// 用于标识有返回值的事件类型，泛型参数类型代表事件参数类型以及返回值类型
    /// </summary>
    public interface ICallEvent<T1, T2, T3> : ICallEventBase, IGenericEvent<T1, T2, T3> { }
    /// <summary>
    /// 用于标识有返回值的事件类型，泛型参数类型代表事件参数类型以及返回值类型
    /// </summary>
    public interface ICallEvent<T1, T2, T3, T4> : ICallEventBase, IGenericEvent<T1, T2, T3, T4> { }
    /// <summary>
    /// 用于标识有返回值的事件类型，泛型参数类型代表事件参数类型以及返回值类型
    /// </summary>
    public interface ICallEvent<T1, T2, T3, T4, T5> : ICallEventBase, IGenericEvent<T1, T2, T3, T4, T5> { }
    /// <summary>
    /// 用于标识有返回值的事件类型，泛型参数类型代表事件参数类型以及返回值类型
    /// </summary>
    public interface ICallEvent<T1, T2, T3, T4, T5, T6> : ICallEventBase, IGenericEvent<T1, T2, T3, T4, T5, T6> { }

#pragma warning restore
}
