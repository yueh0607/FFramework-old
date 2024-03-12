using System;
using UnityEngine;

namespace AirFramework
{
    public class OnAnimatorIKListener : MonoBehaviour
    {
        public Action<int> action_list = null;

        public event Action<int> OnHappen
        {
            add => action_list+=value;
            remove => action_list+=value;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            action_list?.Invoke(layerIndex);
        }

    }

}
