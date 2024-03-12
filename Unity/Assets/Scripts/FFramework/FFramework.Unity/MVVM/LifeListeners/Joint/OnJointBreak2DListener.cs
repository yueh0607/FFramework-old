using System;
using UnityEngine;

namespace AirFramework
{
    public class OnJointBreak2DListener : MonoBehaviour
    {
        private Action<Joint2D> action_list = null;

        public event Action<Joint2D> OnHappen
        {
            add => action_list+=value;
            remove => action_list-=value;
        }

        private void OnJointBreak2D(Joint2D joint2D)
        {
            action_list?.Invoke(joint2D);
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnJointBreak2DListener listener, Action<Joint2D> action)
        {
            listener.OnHappen += action;
        }
    }
}
