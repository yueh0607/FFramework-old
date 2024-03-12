using System;
using UnityEngine;

namespace AirFramework
{
    public class OnCollisionEnter2DListener : MonoBehaviour
    {
        // 使用 Action<Collision2D> 类型替代 MessageOperatorBox
        private Action<Collision2D> collisionAction = null;

        public event Action<Collision2D> OnCollisionEnter
        {
            add => collisionAction += value;
            remove => collisionAction -= value;
        }

        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            // 安全调用事件
            collisionAction?.Invoke(collision2D);
        }
    }



}

