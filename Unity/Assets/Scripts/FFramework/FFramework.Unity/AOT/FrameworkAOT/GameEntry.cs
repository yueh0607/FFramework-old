using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using YooAsset;
using Debug = UnityEngine.Debug;

/// <summary>
/// 在UnityEngine的运行时全局入口，Unity环境以此来驱动
/// </summary>
public class GameEntry : MonoBehaviour
{

    private static GameEntry globalEntry = null;
    public static GameEntry Current
    {
        get
        {
            if (globalEntry == null) throw new InvalidOperationException("This attribute is only valid after scene load");
            return globalEntry;
        }
    }


    /// <summary>
    /// 更新状态机
    /// </summary>
    ContextMachine machine = new ContextMachine(
        (assembly) =>
        assembly?.GetType(AOTConfig.EntryPointClass)?.GetMethod(AOTConfig.StaticEntryMethod, BindingFlags.Public | BindingFlags.Static));

    bool canReload = false;

    /// <summary>
    /// 上下文状态机，谨慎操作
    /// </summary>
    public ContextMachine Machine => machine;

    void Update()
    {
        machine.UpdateMachine();
    }

    /// <summary>
    /// 加载DLL
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadDlls(ResourcePackage package)
    {
        Debug.Log("准备加载上下文..");

#if FF_SIMULATED
        foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            machine.GetEntry?.Invoke(assembly)?.Invoke(null,new object[] { machine });
        }
        yield break;
        
#elif FF_OFFLINE || FF_HOST
        //补充元数据(此过程也可以在热更新程序集内完成)

        AssetInfo[] metaDataAssetInfos = package.GetAssetInfos(AOTConfig.MetaDataDllBytesTag);
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDll in metaDataAssetInfos)
        {
            var handle = package.LoadRawFileAsync(aotDll);
            yield return handle;
            byte[] dllBytes = handle.GetRawFileData();
            handle.Dispose();
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
        }
        Debug.Log($"补充元数据完毕! 元数据程序集数量={metaDataAssetInfos.Length}");


        
        //加载热更新程序集
        AssetInfo[] hotUpdateAssetInfos = package.GetAssetInfos(AOTConfig.HotUpdateDllBytesTag);
        foreach (var aotDll in hotUpdateAssetInfos)
        {
            var handle = package.LoadRawFileAsync(aotDll);
            yield return handle;
            var data = handle.GetRawFileData();
            machine.LoadAssemblyToNextContext(data);
            handle.Dispose();
        }

        if(hotUpdateAssetInfos.Length==0)
        {
            Debug.LogError("无任何热更新程序集(dll.bytes)资源!!!");
            yield break;
        }    

        Debug.Log($"加载热更程序集到后台上下文完毕... 热更新程序集数量={hotUpdateAssetInfos.Length}");

#endif
    }

    IEnumerator InitFramework()
    {
        Debug.Log("准备初始化资源管理..");

        YooAssets.Initialize();
        var gameLogic = YooAssets.CreatePackage(AOTConfig.GameLogicPackageName);

        //编辑器模拟模式
#if FF_SIMULATED
        var initParameters = new EditorSimulateModeParameters();
        var simulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline,AOTConfig.GameLogicPackageName);
        initParameters.SimulateManifestFilePath  = simulateManifestFilePath;

#elif FF_OFFLINE||FF_HOST   //不需要热更资源
        var initParameters = new OfflinePlayModeParameters();
#endif
 
        var initOperation = gameLogic.InitializeAsync(initParameters);
        yield return initOperation;


        if (initOperation.Status == EOperationStatus.Succeed)
        {
            Debug.Log("资源包初始化成功！");
        }
        else
        {
            Debug.LogError($"资源包初始化失败：{initOperation.Error}");
            yield break;
        }

        //加载DLL并进入入口
        yield return LoadDlls(gameLogic);
        //销毁资源包缓存，在热更程序集可以进行重新加载
        YooAssets.DestroyPackage(AOTConfig.GameLogicPackageName);
#if FF_OFFLINE||FF_HOST
        Debug.Log("切换状态机上下文...");
        machine.MoveNext();
        yield return new WaitUntil(() => machine.NeedRefresh == 0);
#if UNITY_EDITOR
        watch.Stop();
#endif
        Debug.Log($"热更程序集加载完毕： 耗时={watch.Elapsed.TotalSeconds}s");
#endif

    }
    void Awake()
    {
        StartCoroutine(InitFramework());

    }



#if UNITY_EDITOR
    static Stopwatch watch = new Stopwatch();
#endif

    static bool initialized = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AOTGameInit()
    {
        if (!initialized)
        {
            
#if UNITY_EDITOR
            watch.Start();
#endif
            Debug.Log("AOT 启动..");
            GameObject launcher = new GameObject("[FFramework.UnityRunner]");
            GameObject.DontDestroyOnLoad(launcher);
            globalEntry = launcher.AddComponent<GameEntry>();
            initialized = true;
            Debug.Log("创建AOT 入口");
        }
    }


}
