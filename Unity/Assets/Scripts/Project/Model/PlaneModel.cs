using FFramework.MVVM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneModel : IModel
{
    /// <summary>
    /// 最大击杀数量
    /// </summary>
    [Save("MaxKillCount")]
    public BindableProperty<int> maxKillCount = new BindableProperty<int>(0);

    /// <summary>
    /// 当前击杀数量
    /// </summary>
    public BindableProperty<int> killCount = new BindableProperty<int>(0);
}   
