using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AOTConfig 
{
    //这个名字无伤大雅，只是一个物体名
    public static readonly string RunnerName = "[FFramework.UnityRunner]";
    //热更新程序集入口类名
    public static readonly string EntryPointClass = "FFramework.EntryPoint";
    //热更程序集入口方法名（公开&&静态&&参数为ContextMachine）
    public static readonly string StaticEntryMethod = "Main";


    //游戏逻辑包名 ，注意需要使用RawFileBuildPipeline构建原生文件包
    public static readonly string GameLogicPackageName = "GameLogicPackage";
    //补充元数据程序集标签
    public static readonly string MetaDataDllBytesTag = "MetaDataAssembly";
    //热更新程序集标签
    public static readonly string HotUpdateDllBytesTag = "HotUpdateAssembly";


}
