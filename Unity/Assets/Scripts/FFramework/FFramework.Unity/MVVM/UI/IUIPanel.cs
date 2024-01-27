using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework.MVVM
{
    public interface IUIPanel : IViewModel
    {
        /// <summary>
        /// 在Show时调用
        /// </summary>
        /// <returns></returns>
        FTask OnShow();


        /// <summary>
        /// 在Hide时调用
        /// </summary>
        /// <returns></returns>
        FTask OnHide();

    }
    public interface IUIPanel<T> : IUIPanel, IViewModel<T> where T : View
    {
        
    }

 
}
