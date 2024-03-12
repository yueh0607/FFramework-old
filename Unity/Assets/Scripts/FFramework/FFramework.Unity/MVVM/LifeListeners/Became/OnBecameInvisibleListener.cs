using System;
using UnityEngine;

namespace AirFramework
{
    public class OnBecameInvisibleListener : MonoBehaviour
    {
        private Action action_list = null;

        public event Action OnHappen
        {
            add => action_list += value;
            remove => action_list -= value;
        }

        private void OnBecameInvisible()
        {
            action_list?.Invoke();
        }
    }

}
