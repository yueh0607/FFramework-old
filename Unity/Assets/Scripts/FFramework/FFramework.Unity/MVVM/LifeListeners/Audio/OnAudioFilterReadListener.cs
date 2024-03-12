using System;
using UnityEngine;

namespace AirFramework
{
    public class OnAudioFilterReadListener : MonoBehaviour
    {
        private Action<float[], int> action_list =null;

        public event Action<float[], int> OnHappen
        {
            add => action_list += value;
            remove => action_list -= value;
        }

        private void OnAudioFilterRead(float[] data, int channels)
        {
            action_list?.Invoke(data, channels);
        }

    }

}
