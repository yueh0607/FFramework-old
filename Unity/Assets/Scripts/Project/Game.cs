using FFramework;
using FFramework.MVVM;
using FFramework.MVVM.RefCache;
using FFramework.MVVM.UI;
using System.Threading;
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
        
        //加载光照
        var sun = await MV.Load<SunVM, Sun>();

        //加载地图
        //var map = await MV.Load<MapVM, Map>();
        MainWorldGenerator mwg = new();
        var genTask = mwg.Generate((x) => Debug.Log($"{x}"));
        CancellationTokenSource source = new CancellationTokenSource();
        await FTask.ToFTask(genTask,source);
        var mapModel = MV.GetModel<MapModel>();

        //加载玩家
        var player = await MV.Load<PlayerVM, Player>();
        player.TView.Player_Transform.position = new Vector3(MapModel.ChunkOrigin.x+MapModel.ChunkSize.x/2, MapModel.ChunkOrigin.y + MapModel.ChunkSize.y/2, MapModel.ChunkOrigin.z+MapModel.ChunkSize.x / 2);
        
        //加载主UI面板
        await UI.Show<GamePanelVM, GamePanel>();

        //模拟扣血
        MV.GetModel<PlayerModel>().Hp.Value -= 10;
        

        await FTask.CompletedTask;
    }
}
