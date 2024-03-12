using System;
using UnityEngine;

namespace AirFramework
{
    public class OnTriggerExit2DListener : MonoBehaviour
    {
        private Action<Collider2D> triggerAction = null; public event Action<Collider2D> OnHappen { add => triggerAction += value; remove => triggerAction -= value; }
        private void OnTriggerExit2D(Collider2D other)
        {
            triggerAction?.Invoke(other);
        }

    }

  
}
