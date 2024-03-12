using System;
using UnityEngine;

namespace AirFramework
{
    public class OnApplicationFocusListener : MonoBehaviour
    {
        public Action<bool> action_list = null;
        public event Action<bool> OnHappen 
        {
            add => action_list += value; 
            remove => action_list += value; 
        }
        private void OnApplicationFocus(bool focus)
        {
            action_list?.Invoke(focus);
        }

    }

  
}
