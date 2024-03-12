using System;
using UnityEngine;

namespace AirFramework
{
    public class OnValidateListener : MonoBehaviour
    {
        private Action action_list = null;

        public event Action OnHappen
        {
            add => action_list+=value;
            remove => action_list-=value;
        }

        private void OnValidate()
        {
            action_list?.Invoke();
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnValidateListener listener, Action action)
        {
            listener.OnHappen += action;
        }
    }
}
