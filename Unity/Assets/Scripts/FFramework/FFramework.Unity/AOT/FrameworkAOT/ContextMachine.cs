////#define NETCOREAPP3_0_OR_GREATER

//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;


//#if NETCOREAPP3_0_OR_GREATER
//using System.Runtime.Loader;
//#endif


////流程
////1.在AOT创建上下文状态机，并加载初始程序集到后台上下文
////2.在AOT调用MoveNext ，使得后台上下文成为当前上下文，并自动触发入口点方法（禁止一切形式引用前后台上下文，包括反射
////  因为上下文只有在CLR内无引用时才能被卸载，要求必须从第三方负责此状态转换）
////3.在AOT调用UpdateMachine，检查当前上下文是否需要重新调用入口点方法
////4.在热更新程序集中加载程序集到后台上下文，并调用MoveNext
////5.在旧热更新程序集中交还程序执行权(入口方法return)，上下文状态机将自动调用下一个入口点

//// 执行权归属 ： 假定每个同步能够阻塞的单元拥有一个执行权
//// AOT程序集入口赋予状态机在程序生命永久的执行权，然后状态机赋予热更程序集执行权
//// 在热更程序集加载新的热更程序集后，需要向状态机交回执行权，状态机再次赋予新的热更程序集执行权
//// AOT程序集入口执行权->状态机更新执行权->热更新程序集入口执行权->状态机更新执行权->新热更程序集执行权


////引用归属
//// AOT程序集在设置后台上下文后，切换到前台并执行入口方法，此时的前台上下文不可卸载
//// 前台上下文交还执行权被丢弃后，进行卸载，后台成为新的前台上下文
//// 此外上下文卸载为异步过程，而且卸载具有不确定性，不允许以任何形式访问状态机管理的上下文，以免上下文滞留在运行时不能卸载



//public class ContextMachine
//{
//    private static long _contextId = long.MinValue;
//#if NETCOREAPP3_0_OR_GREATER
//        //上下文队列
//        Queue<AssemblyLoadContext> contexts = new Queue<AssemblyLoadContext>();
//#else
//    Queue<AppDomain> contexts = new Queue<AppDomain>();
//#endif
//    //创建可卸载的、名称不冲突的上下文
//    private void CreateNewConext()
//    {
//#if NETCOREAPP3_0_OR_GREATER
//            contexts.Enqueue(new AssemblyLoadContext($"ConextMachine_{_contextId++}", true));
//#else
//        contexts.Enqueue(AppDomain.CreateDomain($"ConextMachine_{_contextId++}"));
//#endif
//    }


//    Func<Assembly, MethodInfo> getEntry;
//    public Func<Assembly, MethodInfo> GetEntry => getEntry;
//    public ContextMachine(Func<Assembly, MethodInfo> getEntryPointFunc)
//    {
//        getEntry = getEntryPointFunc;
//        //创建后台上下文
//        CreateNewConext();

//        //设置后台上下文,但不会触发入口点更新
//        MoveNext();
//    }

//#if NETCOREAPP3_0_OR_GREATER
//        private AssemblyLoadContext CurrentContext { get; set; } = null;
//        private AssemblyLoadContext BackContext { get; set; } = null;
//#else
//    private AppDomain CurrentContext { get; set; } = null;
//    private AppDomain BackContext { get; set; } = null;
//#endif


//    /// <summary>
//    /// 加载程序集到后台上下文
//    /// </summary>
//    /// <param name="path"></param>
//    public void LoadAssemblyToNextContext(byte[] bytes)
//    {
//#if NETCOREAPP3_0_OR_GREATER
//        BackContext.LoadFromAssembly(bytes);
//#else
      
//        BackContext.Load(bytes);
//#endif
//    }

//    /// <summary>
//    /// 切换到后台上下文
//    /// </summary>
//    public void MoveNext()
//    {

//#if NETCOREAPP3_0_OR_GREATER
//        //卸载当前上下文
//        CurrentContext?.Unload();

//        //抛弃引用才能真正开始卸载（详情见MSDN）
//        CurrentContext = null;
           
//#else
//        //卸载当前上下文
//        if (CurrentContext != null)
//            AppDomain.Unload(CurrentContext);
//        CurrentContext = null;
//#endif
//        GC.Collect();

//        //挪动上下文
//        CurrentContext = BackContext;
//        //设置新的后台上下文
//        BackContext = contexts.Dequeue();
//        //备用上下文数量检查并创建
//        if (contexts.Count == 0) CreateNewConext();

//        //仅在入口点存在时才会需要刷新
//        if (CurrentContext != null) needRefresh++;
//    }


//    //指示当前上下文是否需要重新调用入口点
//    private int needRefresh = 0;
//    public int NeedRefresh => needRefresh;
//    public void UpdateMachine()
//    {
//        if (needRefresh > 0)
//        {
//#if NETCOREAPP3_0_OR_GREATER
//            foreach (var assembly in CurrentContext.Assemblies)
//            {
//#else
//            needRefresh--;
//            foreach (var assembly in CurrentContext.GetAssemblies())
//            {
//#endif
//                getEntry(assembly)?.Invoke(null, new object[] { this });
//            }
            
//        }
//    }
//}

