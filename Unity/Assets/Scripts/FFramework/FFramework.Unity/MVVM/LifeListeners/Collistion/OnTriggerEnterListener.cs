using System;
using UnityEngine;

namespace AirFramework
{
    public class OnTriggerEnterListener : MonoBehaviour
    {
        // 使用 Action<Collider> 类型替代 MessageOperatorBox
        private Action<Collider> triggerAction = null;

        public event Action<Collider> OnHappen
        {
            add => triggerAction += value;
            remove => triggerAction -= value;
        }

        private void OnTriggerEnter(Collider other)
        {
            // 安全调用事件
            triggerAction?.Invoke(other);
        }
    }


}
