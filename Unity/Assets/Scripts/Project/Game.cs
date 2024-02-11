using FFramework;
using FFramework.MVVM;
using FFramework.MVVM.RefCache;
using UnityEngine;
using YooAsset;



public class Game
{
    /// <summary>
    /// 初始化
    /// </summary>
    /// <returns></returns>
    public static async FTask Initialize()
    {
        
        //初始化YooAsset
        ResourcePackage defaultPackage = YooAssets.CreatePackage("GameArts");
        YooAssets.SetDefaultPackage(defaultPackage);

#if FF_SIMULATED
        var initParameters = new EditorSimulateModeParameters();
        var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline, "GameArts");
        initParameters.SimulateManifestFilePath = simulateManifestFilePath;
#endif
#if FF_OFFLINE
        OfflinePlayModeParameters initParameters = new OfflinePlayModeParameters();
#endif
        await defaultPackage.InitializeAsync(initParameters);
        Debug.Log($"GameArts 资源包初始化状态: {defaultPackage.InitializeStatus}");

        MV.Package = defaultPackage;
        MV.LoadConverter = (type) => type.Name;
        Debug.Log("MVVM初始化完成");

        await FTask.CompletedTask;
    }

    /// <summary>
    /// 过程（一定会在初始化后执行）
    /// </summary>
    /// <param name="thread"></param>
    /// <returns></returns>
    public static async FTask Process(UnityThread thread)
    {
        Debug.Log("GameProcess");
        var map = await MV.Load<MapVM, Map>();
        var player = await MV.Load<PlayerVM, Player>();
        player.TView.Player_Transform.position = map.TView.transform.position + Vector3.up;
        


        await FTask.CompletedTask;
    }
}
