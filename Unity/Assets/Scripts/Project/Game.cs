using FFramework;
using FFramework.MVVM;
using FFramework.MVVM.RefCache;
using FFramework.MVVM.UI;
using System.Threading;
using UnityEngine;
using YooAsset;



public static class Game
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

    class Test<T,K> : FUnit
    {

    }

    public static GameController GameCon;
    /// <summary>
    /// 过程（一定会在初始化后执行）
    /// </summary>
    /// <param name="thread"></param>
    /// <returns></returns>
    public static async FTask Process(UnityThread thread)
    {
        Debug.Log("GameProcess");
        //Test<int,string> t = new Game.Test<int,string>();
        GameCon = new GameController();
        //FTaskToken token = new FTaskToken();
        //DoSom().SetToken(token);
        //token.Yield();
        //await FTask.Delay(1);
        //token.Continue();

        //await FTask.Delay(1);
        //Debug.Log(token.Status);
        await FTask.CompletedTask;
    }


    static async FTask DoSom()
    {
        Debug.Log("HH1");
        //await FTask.Delay(1);
        Debug.Log("HH2");
        var tokenCatch = await FTask.CatchToken();
        Debug.Log("HH3");
        Debug.Log(tokenCatch.Status);
        await FTask.Delay(1);
        Debug.Log("HH4");


    }
}
