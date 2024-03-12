using System;
using UnityEngine;

namespace AirFramework
{
    public class OnCanvasGroupChangedListener : MonoBehaviour
    {
        private Action action_list = null; public event Action OnHappen { add => action_list += value; remove => action_list -= value; }
        private void OnCanvasGroupChanged()
        {
            action_list?.Invoke();
        }

    }

   
}
