using FFramework;
using UnityEngine;



namespace FFramework
{
    public class EntryPoint
    {
        public static void Main()
        {
            Debug.Log($"热更程序集入口调用: {nameof(FFramework.EntryPoint.Main)}");
            FTaskToken.ErrorHandler = (x) => x.Throw();
            UnityGlobal.GameAwake().Coroutine();
        }
    }
}
public class UnityGlobal
{

    private static bool initialized = false;

    private static UnityThread mainThread;

    public static async FTask GameAwake()
    {
        if (!initialized)
        {
            mainThread = GameEntry.Current.gameObject.AddComponent<UnityThread>();
            await Game.Initialize();
            Debug.Log("Ori");
            Game.Process(mainThread).Coroutine();
            initialized = true;
        }
    }

    /// <summary>
    /// 是否在UnityEngine内初始化完成(场景加载后使用无需验证此值)
    /// </summary>
    public static bool Initialized => initialized;

    /// <summary>
    /// Unity主线程（可以访问生命周期函数）
    /// </summary>
    public static UnityThread MainThread => mainThread;


}
