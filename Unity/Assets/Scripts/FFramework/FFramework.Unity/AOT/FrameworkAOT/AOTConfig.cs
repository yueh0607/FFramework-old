using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AOTConfig 
{
    //这个名字无伤大雅，只是一个物体名
    public const string RunnerName = "[FFramework.UnityRunner]";
    //热更新程序集入口类名
    public const string EntryPointClass = "FFramework.EntryPoint";
    //热更程序集入口方法名（公开&&静态&&参数为void）
    public const string StaticEntryMethod = "Main";


    //游戏逻辑包名 ，注意需要使用RawFileBuildPipeline构建原生文件包
    public const string GameLogicPackageName = "GameLogicPackage";
    //补充元数据程序集标签
    public const string MetaDataDllBytesTag = "MetaDataAssembly";
    //热更新程序集标签
    public const string HotUpdateDllBytesTag = "HotUpdateAssembly";


}
