﻿/*******************************************************
 * Code Generated By FFramework
 * DateTime : 2024/3/8 16:15:28
 * UVersion : 2021.3.29f1
 *******************************************************/
using UnityEngine;
using FFramework.MVVM;

namespace FFramework.MVVM.RefCache
{
    public class MenuCamera : FFramework.MVVM.View
    {
        public UnityEngine.Camera @MenuCamera_Camera { get; private set; } = default;


        public void InitRefs()
        {
            @MenuCamera_Camera = GetComponent<UnityEngine.Camera>();

        }

        
    }
    
}
