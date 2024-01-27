using FFramework.Internal;
using FFramework.Network;
using FFramework.Rpc;
using System;
using System.Collections.Generic;
using System.Net.Sockets.Kcp;


namespace FFramework
{
    public static class FEvent
    {
        //事件容器
        internal static Dictionary<Type, FOperator<IFEventBase>> local_container = new Dictionary<Type, FOperator<IFEventBase>>();
        //移除标记
        private static HashSet<Type> removeMarks = new HashSet<Type>();

        //如果认为容器中空余的事件Operator占内存，则随便写个脚本计时调用这个方法即可。
        public static void ReleaseEmpty()
        {
            foreach (var con in local_container)
            {
                if (con.Value.Count == 0) removeMarks.Add(con.Key);
            }
            foreach (var tp in removeMarks)
            {
                local_container.Remove(tp);
            }
            removeMarks.Clear();
        }

        /// <summary>
        /// 在静态类中无法使用this，则使用CoEvents.Instance.Operator
        /// </summary>
        public readonly static object Instance = new object();

        internal static KcpChannel Client { get; private set; }
        ///// <summary>
        ///// 初始化Rpc信道
        ///// </summary>
        ///// <param name="client"></param>
        //public static void InitializeRpc(IKcpIO client)
        //{
            
        //    Client = client;
        //    StartReceive(Client, OnRecv);
        //}
        //private static void OnRecv(byte[] bytes)
        //{
        //    int size = sizeof(long);
        //    long hashId = MemoryPack.MemoryPackSerializer.Deserialize<long>(new ReadOnlySpan<byte>(bytes,0,size));
        //    FRpcStrategyAdapter.ApplyStrategy(hashId, new ReadOnlySpan<byte>(bytes, size, bytes.Length - size));
        //}
        //private static async void StartReceive(IKcpIO client, Action<byte[]> OnRecv)
        //{
        //    while (true)
        //    {
        //        var res = await client.ReceiveAsync();
        //        OnRecv?.Invoke(res);
        //    }
        //}

    }


}
