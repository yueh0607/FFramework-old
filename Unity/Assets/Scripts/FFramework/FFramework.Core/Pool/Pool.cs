
/* 项目“Assembly-CSharp.Player”的未合并的更改
在此之前:
using System;
在此之后:
using FFramework.Internal;
using System;
*/
using FFramework.Internal;
using System;
using System.Collections.Generic;
/* 项目“Assembly-CSharp.Player”的未合并的更改
在此之前:
using UnityEngine;
using FFramework.Internal;
在此之后:
using UnityEngine;
*/


namespace FFramework
{
    public static class Pool
    {

        static readonly Dictionary<Type, Container> pools = new Dictionary<Type, Container>();


        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T Get<T>()
        {
            if (!pools.ContainsKey(typeof(T)))
                throw new InvalidOperationException("No such pool");

            return (T)pools[typeof(T)].Get();
        }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T Get<T, K>() where K : IPoolable<T>, new()
        {
            Pool.Create(new K());
            return (T)pools[typeof(T)].Get();
        }


        /// <summary>
        /// 放置一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Set<T>(T obj)
        {
            if (!pools.ContainsKey(typeof(T)))
                throw new InvalidOperationException("No such pool");
            if (obj == null) throw new NullReferenceException();
            pools[typeof(T)].Set(obj);
        }
        /// <summary>
        /// 放置一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Set<T, K>(T obj) where K : IPoolable<T>, new()
        {
            Pool.Create(new K());
            if (obj == null) throw new NullReferenceException();
            pools[typeof(T)].Set(obj);
        }

        /// <summary>
        /// 统计池内对象数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static int Count<T>()
        {
            if (!pools.ContainsKey(typeof(T)))
                throw new InvalidOperationException("No such pool");

            return pools[typeof(T)].Count;
        }
        /// <summary>
        /// 统计池内对象数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public static int Count<T, K>() where K : IPoolable<T>, new()
        {
            Pool.Create(new K());
            return pools[typeof(T)].Count;
        }

        /// <summary>
        /// 释放整个池（不存在则抛出异常）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="InvalidOperationException"></exception>
        public static void Release<T>()
        {
            if (!pools.ContainsKey(typeof(T)))
                throw new InvalidOperationException("No such pool");

            pools[typeof(T)].Release();
            pools.Remove(typeof(T));
        }
        /// <summary>
        /// 尝试释放整个池（允许不存在）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        public static void TryRelease<T>()
        {
            if (pools.ContainsKey(typeof(T)))
            {
                pools[typeof(T)].Release();
                pools.Remove(typeof(T));
            }
        }


        /// <summary>
        /// 创建一个对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="properties"></param>
        public static void Create<T>(IPoolable<T> properties)
        {
            if (!pools.ContainsKey(typeof(T)))
            {
                pools.Add(typeof(T), new Container<T>(properties));
            }
        }



        public class DefaultPoolable<T> : IPoolable<T> where T : new()
        {
            int IPoolable.Capacity => 1000;

            T IPoolable<T>.OnCreate()
            {
                return new T();
            }

            void IPoolable<T>.OnDestroy(T obj)
            {

            }

            void IPoolable<T>.OnGet(T obj)
            {

            }

            void IPoolable<T>.OnSet(T obj)
            {

            }
        }
    }


}
