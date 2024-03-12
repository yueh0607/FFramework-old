using System;
using UnityEngine;

namespace AirFramework
{
    public class OnDrawGizmosListener : MonoBehaviour
    {
        private Action action_list = null;

        public event Action OnHappen
        {
            add => action_list+=value;
            remove => action_list-=value;
        }

        private void OnDrawGizmos()
        {
            action_list?.Invoke();
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnDrawGizmosListener listener, Action action)
        {
            listener.OnHappen += action;
        }
    }
}
