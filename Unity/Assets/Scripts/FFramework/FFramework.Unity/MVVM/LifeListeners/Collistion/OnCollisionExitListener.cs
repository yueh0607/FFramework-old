/***********************************************************************************
 * Author      : yueh0607
 * Version     : 2021.3.22f1c1
 * Date        : 2023/5/14 23:02:23
 * Description : Describe the function here.
************************************************************************************/


using System;
using UnityEngine;

namespace AirFramework
{
    public class OnCollisionExitListener : MonoBehaviour
    {
        private Action<Collision> action_list = null;

        public event Action<Collision> OnHappen
        {
            add => action_list+=value;
            remove => action_list-=value;
        }


        private void OnCollisionExit(Collision collision)
        {
            action_list?.Invoke(collision);
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnCollisionExitListener listener, Action<Collision> action)
        {
            listener.OnHappen += action;
        }
    }

}

