using System;
using UnityEngine;

namespace AirFramework
{
    public class OnCollisionStayListener : MonoBehaviour
    {
        private Action<Collision> action_list = null;

        public event Action<Collision> OnHappen
        {
            add => action_list+=value;
            remove => action_list-=value;
        }


        private void OnCollisionStay(Collision collision)
        {
            action_list?.Invoke(collision);
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnCollisionStayListener listener, Action<Collision> action)
        {
            listener.OnHappen += action;
        }
    }


}

