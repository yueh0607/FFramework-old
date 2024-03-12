using System;
using UnityEngine;

namespace AirFramework
{
    public class OnTriggerEnter2DListener : MonoBehaviour
    {
        // 使用 Action<Collider2D> 类型来代替 MessageOperatorBox
        private Action<Collider2D> triggerAction = null;

        public event Action<Collider2D> OnHappen
        {
            add => triggerAction += value;
            remove => triggerAction -= value;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // 安全调用事件
            triggerAction?.Invoke(other);
        }
    }

   
}

