using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FFramework.MVVM
{
    public interface IViewModel 
    {
        View View { get; set; }


        /// <summary>
        /// 在加载时调用
        /// </summary>
        /// <returns></returns>
        FTask OnLoad();

    
        /// <summary>
        /// 在卸载时调用
        /// </summary>
        /// <returns></returns>
        FTask OnUnload();

    }
    public interface IViewModel<T> : IViewModel where T : View
    {
        T TView { get; set; }
    }
}