using System;
using UnityEngine;

namespace AirFramework
{
    public class OnJointBreakListener : MonoBehaviour
    {
        private Action<float> action_list = null;

        public event Action<float> OnHappen
        {
            add => action_list+=value;
            remove => action_list-=value;
        }

        private void OnJointBreak(float breakForce)
        {
            action_list?.Invoke(breakForce);
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnJointBreakListener listener, Action<float> action)
        {
            listener.OnHappen += action;
        }
    }
}
