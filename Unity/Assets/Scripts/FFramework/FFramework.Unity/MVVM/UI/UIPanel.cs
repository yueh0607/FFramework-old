using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework.MVVM
{
    public abstract class UIPanel<T> :  ViewModel<T>, IUIPanel<T> where T : View
    {

        public virtual async FTask OnHide()
        {
            await FTask.CompletedTask;
        }


        public virtual async FTask OnShow()
        {
            await FTask.CompletedTask;
        }

    
    }

}
