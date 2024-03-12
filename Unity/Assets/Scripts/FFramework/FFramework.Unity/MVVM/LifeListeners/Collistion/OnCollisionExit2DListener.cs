using System;
using UnityEngine;

namespace AirFramework
{
    public class OnCollisionExit2DListener : MonoBehaviour
    {
        private Action<Collision2D> action_list = null;

        public event Action<Collision2D> OnHappen
        {
            add => action_list+=value;
            remove => action_list-=value;
        }


        private void OnCollisionExit2D(Collision2D collision2D)
        {
            action_list?.Invoke(collision2D);
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnCollisionExit2DListener listener, Action<Collision2D> action)
        {
            listener.OnHappen += action;
        }
    }
}

