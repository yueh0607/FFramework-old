using FFramework.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FFramework
{
    public class FOperator<T> : IFEventOperator<T>
    {
        public Type MsgType { get; set; }
        public List<Delegate> Events { get; private set; } = new List<Delegate>();

        /// <summary>
        /// 委托数
        /// </summary>
        public int Count => Events.Count;

        public int IntervalIndex { get; private set; } = 0;


        /// <summary>
        /// 清空操作器
        /// </summary>
        public void Clear()
        {
            Events.Clear();
        }



        public void Add(Delegate dele)
        {
            Events.Add(dele);
        }

        public bool Remove(Delegate dele)
        {
            int index = Events.IndexOf(dele);
            if (index <= IntervalIndex) --IntervalIndex;

            return Events.Remove(dele);
        }

        public bool GetNext(out Delegate dele)
        {
            if (IntervalIndex >= 0 && IntervalIndex < Events.Count)
            {

                dele = Events[IntervalIndex++];
                return true;
            }
            dele = null;
            return false;
        }

        public void Reset()
        {
            IntervalIndex = 0;
        }

    }



    internal static class CoV_EX
    {
        [DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static FOperator<IFEventBase> GetOperator<T>(this IFEventOperator<T> ico)
        {
            return (FOperator<IFEventBase>)ico;
        }

    }
}
