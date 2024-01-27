using FFramework.Internal;
using System;

namespace FFramework
{
    public static class CoEventManagerEx1
    {
        //[DebuggerHidden]
        public static IFEventOperator<EventType> Operator<EventType>(this object cov) where EventType : ISendEventBase
        {
            Type type = typeof(EventType);
            if (!FEvent.local_container.ContainsKey(type))
            {
                var op = new FOperator<IFEventBase>();
                op.MsgType = type;
                FEvent.local_container.Add(type, op);
            }
            FOperator<IFEventBase> cop = FEvent.local_container[type];
            return CoUnsafeAs.As<FOperator<IFEventBase>, FOperator<EventType>>(ref cop);
        }
    }
    public static class CoEventManagerEx2
    {
        //[DebuggerHidden]
        public static IFEventOperator<EventType> Operator<EventType>(this object cov) where EventType : ICallEventBase
        {
            Type type = typeof(EventType);
            if (!FEvent.local_container.ContainsKey(type))
            {
                var op = new FOperator<IFEventBase>();
                op.MsgType = type;
                FEvent.local_container.Add(type, op);
            }
            FOperator<IFEventBase> cop = FEvent.local_container[type];
            return CoUnsafeAs.As<FOperator<IFEventBase>, FOperator<EventType>>(ref cop);
        }
    }
}
