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
    /// 加载DLL
    /// </summary>
    /// <returns></returns>
    public IEnumerator LoadDlls(ResourcePackage package)
    {
        Debug.Log("准备加载上下文..");

#if FF_SIMULATED
        int entryCount = 0;
        foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            MethodInfo method = assembly.GetType(AOTConfig.EntryPointClass,false)?.GetMethod(AOTConfig.StaticEntryMethod, BindingFlags.Public | BindingFlags.Static);
            method?.Invoke(null, null);
            if (method != null) entryCount++;
        }
        Debug.Log($"找到入口: {entryCount}");
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


        List<Assembly> hotUpdateAssemblies = new List<Assembly>();
        //加载热更新程序集
        AssetInfo[] hotUpdateAssetInfos = package.GetAssetInfos(AOTConfig.HotUpdateDllBytesTag);
        foreach (var aotDll in hotUpdateAssetInfos)
        {
            var handle = package.LoadRawFileAsync(aotDll);
            yield return handle;
            var data = handle.GetRawFileData();
            hotUpdateAssemblies.Add(Assembly.Load(data));
            handle.Dispose();
        }

        if (hotUpdateAssetInfos.Length == 0)
        {
            Debug.LogError("无任何热更新程序集(dll.bytes)资源!!!");
            yield break;
        }
        else Debug.Log($"加载热更程序集完毕... 热更新程序集数量={hotUpdateAssetInfos.Length}");

        int entryCount = 0;

        //入口调用
        for (int i = 0; i < hotUpdateAssemblies.Count; i++)
        {
            MethodInfo method = hotUpdateAssemblies[i].GetType(AOTConfig.EntryPointClass).GetMethod(AOTConfig.StaticEntryMethod, BindingFlags.Public | BindingFlags.Static);
            entryCount += (method != null) ? 1 : 0;

            method?.Invoke(null, null);
        }

        Debug.Log($"找到入口: {entryCount}");

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

#if UNITY_EDITOR
        watch.Stop();
        Debug.Log($"热更程序集加载完毕： 耗时={watch.Elapsed.TotalSeconds}s");
#else
        Debug.Log($"热更程序集加载完毕");
#endif

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
