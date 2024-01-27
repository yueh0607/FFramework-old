using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FFramework
{
    /// <summary>
    /// 框架从此特性标记方法启动
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class NotifyAttribute : Attribute
    {
        public NotifyAttribute(string methodName) 
        {
            
        }

    }

   
}
