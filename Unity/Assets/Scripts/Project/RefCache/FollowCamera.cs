﻿/*******************************************************
 * Code Generated By FFramework
 * DateTime : 2024/3/6 8:36:54
 * UVersion : 2021.3.29f1
 *******************************************************/
using UnityEngine;
using FFramework.MVVM;

namespace FFramework.MVVM.RefCache
{
    public class FollowCamera : FFramework.MVVM.View
    {
        public UnityEngine.Transform @FollowCamera_Transform { get; private set; } = default;


        public void InitRefs()
        {
            @FollowCamera_Transform = GetComponent<UnityEngine.Transform>();

        }

        
    }
    
}