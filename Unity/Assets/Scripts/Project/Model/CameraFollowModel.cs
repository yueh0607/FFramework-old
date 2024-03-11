using FFramework.MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowModel : IModel
{
    //相机同步速度
    public float syncSpeed = 0.03f;
    //偏移值
    public Vector3 offset = new Vector3(0, 100, 0);

    


}
