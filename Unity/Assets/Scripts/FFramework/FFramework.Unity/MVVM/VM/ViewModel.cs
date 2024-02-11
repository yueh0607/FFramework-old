using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework.MVVM
{
    public class ViewModel<T> : IViewModel<T> where T : View
    {
        /// <summary>
        /// 视图
        /// </summary>
        public T TView { get =>(T)View; set=> View=value ; }
        /// <summary>
        /// 视图
        /// </summary>
        public View View { get; set ; }

        /// <summary>
        /// 在加载时调用
        /// </summary>
        /// <returns></returns>
        public virtual async FTask OnLoad()
        {
            Debug.Log("ViewModel OnLoad1");
            await FTask.CompletedTask;
            Debug.Log("ViewModel OnLoad2");
        }



        /// <summary>
        /// 在卸载时调用
        /// </summary>
        /// <returns></returns>
        public virtual async FTask OnUnload()
        {
            await FTask.CompletedTask;
        }

    }
}
