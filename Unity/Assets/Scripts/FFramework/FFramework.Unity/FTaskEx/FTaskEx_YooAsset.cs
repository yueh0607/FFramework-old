using FFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public static class FTaskEx
{

    public static FTaskAwaiter GetAwaiter(this YooAsset.AssetHandle operationBase)
    {
        FTask task = Pool.Get<FTask, FTask.FTaskPoolable>();
        FTaskSetResultSource<AssetHandle> source = FTaskSetResultSource<AssetHandle>.GetFromPool();
        source.Task = task;
        operationBase.Completed += source.CompleteAction;
        return task.GetAwaiter();
    }
    public static FTaskAwaiter GetAwaiter(this YooAsset.AsyncOperationBase operationBase)
    {
        FTask task = Pool.Get<FTask, FTask.FTaskPoolable>();
        FTaskSetResultSource<AsyncOperationBase> source = FTaskSetResultSource<AsyncOperationBase>.GetFromPool();
        source.Task = task;
        operationBase.Completed += source.CompleteAction;
       
        return task.GetAwaiter();
    }
}
