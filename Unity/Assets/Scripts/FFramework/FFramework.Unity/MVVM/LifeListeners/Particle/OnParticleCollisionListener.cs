using System;
using UnityEngine;

namespace AirFramework
{
    public class OnParticleCollisionListener : MonoBehaviour
    {
        private Action<GameObject> action_list = null;

        public event Action<GameObject> OnHappen
        {
            add => action_list+=value;
            remove => action_list-=value;
        }


        private void OnParticleCollision(GameObject other)
        {
            action_list?.Invoke(other);
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnParticleCollisionListener listener, Action<GameObject> action)
        {
            listener.OnHappen += action;
        }
    }
}

