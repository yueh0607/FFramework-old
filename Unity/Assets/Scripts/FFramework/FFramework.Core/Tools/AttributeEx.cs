using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FFramework
{
    public static class AttributeHelper
    {
        /// <summary>
        /// 在当前执行的程序集内反射全部具有此特性的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Type> ReflectAllCustomAttributeInExcutingAssembly<T>() where T : Attribute
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().
                Where((x) => x.GetCustomAttribute<T>() != null);
            return types.ToList();
        }



        /// <summary>
        /// 在当前执行的程序集内反射全部具有此特性的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Type> ReflectAllCustomAttributeInCallingAssembly<T>() where T : Attribute
        {
            var types = Assembly.GetCallingAssembly().GetTypes().
                Where((x) => x.GetCustomAttribute<T>() != null);
            return types.ToList();
        }

        /// <summary>
        /// 在当前执行的程序集内反射全部具有此特性的类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Type> ReflectAllCustomAttributeInAppDomain<T>() where T : Attribute
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                 .SelectMany(x => x.GetTypes())
                 .Where((x) => x.GetCustomAttribute<T>() != null)
                 .ToList();
        }

    }
}
