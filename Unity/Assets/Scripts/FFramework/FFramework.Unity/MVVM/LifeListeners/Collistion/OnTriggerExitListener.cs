using System;
using UnityEngine;

namespace AirFramework
{
    public class OnTriggerExitListener : MonoBehaviour
    {
        private Action<Collider> triggerAction = null; public event Action<Collider> OnHappen { add => triggerAction += value; remove => triggerAction -= value; }
        private void OnTriggerExit(Collider other)
        {
            triggerAction?.Invoke(other);
        }

    }


}
