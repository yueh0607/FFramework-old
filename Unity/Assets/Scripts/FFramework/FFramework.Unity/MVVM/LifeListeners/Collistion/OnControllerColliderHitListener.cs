using System;
using UnityEngine;

namespace AirFramework
{
    public class OnControllerColliderHitListener : MonoBehaviour
    {
        private Action<ControllerColliderHit> action_list = null;

        public event Action<ControllerColliderHit> OnHappen
        {
            add => action_list += value;
            remove => action_list -=value ;
        }


        private void OnControllerColliderHit(UnityEngine.ControllerColliderHit hit)
        {
            action_list?.Invoke(hit);
        }

    }

   
}
