using System;
using UnityEngine;

namespace AirFramework
{
    public class OnTriggerStayListener : MonoBehaviour
    {
        private Action<Collider> triggerAction = null; public event Action<Collider> OnHappen { add => triggerAction += value; remove => triggerAction -= value; }
        private void OnTriggerStay(Collider other)
        {
            triggerAction?.Invoke(other);
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnTriggerStayListener listener, Action<Collider> action)
        {
            listener.OnHappen += action;
        }
    }
}

