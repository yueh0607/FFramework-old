using System;
using UnityEngine;

namespace AirFramework
{
    public class OnAnimatorMoveListener : MonoBehaviour
    {
        public Action action_list = null;

        public event Action OnHappen
        {
            add => action_list += value;
            remove => action_list += value;
        }

        private void OnAnimatorMove()
        {
            action_list?.Invoke();
        }

    }


}
