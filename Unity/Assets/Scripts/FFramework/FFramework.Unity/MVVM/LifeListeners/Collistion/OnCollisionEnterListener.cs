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
    public class OnCollisionEnterListener : MonoBehaviour
    {
        // 使用 Action<Collision2D> 类型替代 MessageOperatorBox
        private Action<Collision> collisionAction = null;

        public event Action<Collision> OnHappen
        {
            add => collisionAction += value;
            remove => collisionAction -= value;
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 安全调用事件
            collisionAction?.Invoke(collision);
        }
    }
}
