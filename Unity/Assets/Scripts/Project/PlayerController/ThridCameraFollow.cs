using FFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThridCameraFollow : IUpdate
{
    public Transform Follower { get; set; }
    public Transform Target { get; set; }
    public float Distance { get; set; }
    public float Smooth { get; set; }

    /// <summary>
    /// 创建跟随器
    /// </summary>
    /// <param name="follower"></param>
    /// <param name="target"></param>
    public ThridCameraFollow(Transform follower, Transform target, float distance, float smooth)
    {
        this.Follower = follower;
        this.Target = target;
        this.Distance = distance;
        this.Smooth = smooth;

        lastFrameMousePostion = Input.mousePosition;
    }

    void IUpdate.Update(float deltaTime)
    {
        Rotate();

    }

    //上一帧鼠标位置
    Vector3 lastFrameMousePostion;
    void Rotate()
    {
        //鼠标移动距离
        Vector2 mousePositionDelta = Input.mousePosition - lastFrameMousePostion;
        //获取鼠标在屏幕移动的比率，消除分辨率影响
        Vector2 mouseRatioDelta = mousePositionDelta / new Vector2(Screen.width, Screen.height);

        //想要应用的旋转
        Quaternion rotation = Quaternion.Euler(new Vector2(-mouseRatioDelta.x, mouseRatioDelta.y));

        //被跟随者指向摄象机的位移向量
        Vector3 positionDelta = -(Target.position - Follower.position);
        //把旋转应用到位移向量上，计算新的位移向量
        positionDelta = rotation * positionDelta;

        //新的摄象机位置，如果足够小，可以认为是连线在球面上
        Vector3 newTargetPosition = Target.position + positionDelta;

        //插值设置位置
        newTargetPosition = Vector3.Slerp(Follower.position, newTargetPosition, Smooth * Time.deltaTime);

        Follower.position = newTargetPosition;
    }
}
