using System;
using UnityEngine;

namespace AirFramework
{
    public class OnTriggerStay2DListener : MonoBehaviour
    {
        private Action<Collider2D> triggerAction = null; public event Action<Collider2D> OnHappen { add => triggerAction += value; remove => triggerAction -= value; }
        private void OnTriggerStay2D(Collider2D other)
        {
            triggerAction?.Invoke(other);
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnTriggerStay2DListener listener, Action<Collider2D> action)
        {
            listener.OnHappen += action;
        }
    }
}

