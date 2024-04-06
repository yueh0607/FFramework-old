using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace FFramework
{
    public static partial class TypeExtensions
    {

        /// <summary>
        /// 抽象则抛异常
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException"></exception>
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IfAbstractThrowException(this Type type)
        {
            if (type.IsAbstract) throw new ArgumentException("Type parameters are not allowed to be abstract");
        }
        public static string GetDisplayName(this Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            StringBuilder sb = new StringBuilder();

            // 获取类型名称
            string typeName = type.Name.Substring(0, type.Name.IndexOf('`'));

            sb.Append(typeName);
            sb.Append("<");

            Type[] genericArguments = type.GetGenericArguments();
            for (int i = 0; i < genericArguments.Length; i++)
            {
                if (i > 0)
                    sb.Append(", ");
                sb.Append(genericArguments[i].Name);
            }
            sb.Append(">");

            return sb.ToString();
        }
    }
}

