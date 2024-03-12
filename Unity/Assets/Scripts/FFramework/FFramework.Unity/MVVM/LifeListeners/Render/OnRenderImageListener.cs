using System;
using UnityEngine;

namespace AirFramework
{
    public class OnRenderImageListener : MonoBehaviour
    {
        private Action<RenderTexture, RenderTexture> action_list = null;

        public event Action<RenderTexture, RenderTexture> OnHappen
        {
            add => action_list+=value;
            remove => action_list-=value;
        }


        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            action_list?.Invoke(source, destination);
        }

    }

    public static partial class Listener_Ex
    {
        public static void Bind(this OnRenderImageListener listener, Action<RenderTexture, RenderTexture> action)
        {
            listener.OnHappen += action;
        }
    }
}
