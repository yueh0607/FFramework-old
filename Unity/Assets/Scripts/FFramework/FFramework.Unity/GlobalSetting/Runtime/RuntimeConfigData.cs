using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

namespace FFramework.FUnityEditor
{

    public enum RuntimeMode
    {
        /// <summary>
        /// 编辑器模拟
        /// </summary>
        Simulated = 0,
        /// <summary>
        /// 本地单机
        /// </summary>
        Offline = 1,
        /// <summary>
        /// 联机OR网络
        /// </summary>
        Host = 2
    }
 
}