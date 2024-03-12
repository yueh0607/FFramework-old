using System;
using UnityEngine;

namespace AirFramework
{
    public class OnApplicationPauseListener : MonoBehaviour
    {
        public Action<bool> action_list = null; 
        public event Action<bool> OnHappen { add => action_list += value; remove => action_list += value; }
        private void OnApplicationPause(bool pause)
        {
            action_list?.Invoke(pause);
        }

    }


}
