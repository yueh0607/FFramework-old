using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FFramework.External
{
    public class BuffObject
    {
        /// <summary>
        /// 管理Buff增删
        /// </summary>
        IBuffEffect effect;

        /// <summary>
        /// Buff添加的实体
        /// </summary>
        IBuffAddableEntity entity;


    }

    public interface IBuffEffect
    {
        /// <summary>
        /// 添加效果
        /// </summary>
        void AddEffect(IBuffAddableEntity entity);

        /// <summary>
        /// 移除效果
        /// </summary>
        /// <param name="entity"></param>
        void RemoveEffect(IBuffAddableEntity entity);
    }
    public abstract class BuffEffect<T> : IBuffEffect where T :class, IBuffAddableEntity
    {

        public abstract void AddEffect(T entity);

        void IBuffEffect.AddEffect(IBuffAddableEntity entity)
        {
            AddEffect(entity as T);
        }

        public abstract void RemoveEffect(T entity);

        void IBuffEffect.RemoveEffect(IBuffAddableEntity entity)
        {
            RemoveEffect(entity as T);
        }
    }



    /// <summary>
    /// 能够添加Buff的实体
    /// </summary>
    public interface IBuffAddableEntity
    {
        
    }
}