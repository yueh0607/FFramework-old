using System;
using UnityEngine;

namespace AirFramework
{
    public class OnApplicationQuitListener : MonoBehaviour
    {
        private Action action_list =null;

        public event Action OnHappen
        {
            add => action_list+=value;
            remove => action_list -= value;
        }

        private void OnApplicationQuit()
        {
            action_list?.Invoke();
        }

    }

}
