
using FFramework.Buffers;
using FFramework.Rpc;
using System;
using System.Buffers;
using System.IO;
using System.Runtime.CompilerServices;
namespace FFramework
{
    public static partial class MessageExtensions
    {
        private static object bytes;

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="container"></param>
        public static void RpcSend(this IFEventOperator<ISendEvent> container)
        {
            var mop = container.GetOperator();
            long hash = SHA64.GetHash(mop.MsgType);

            RpcPack pack = new RpcPack();
            var writer = RecyclableBufferWriter<byte>.Rent();
            MemoryPack.MemoryPackSerializer.Serialize(writer, hash);
            MemoryPack.MemoryPackSerializer.Serialize(writer,pack);
            //FEvent.Client.kio.Send
            
            //FEvent.Client.Send(writer.GetWrittenSpan());
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="container"></param>
        public static void RpcSend<T1>(this IFEventOperator<ISendEvent<T1>> container, T1 arg1)
        {
            var mop = container.GetOperator();
            long hash = SHA64.GetHash(mop.MsgType);

            RpcPack<T1> pack = new RpcPack<T1>();

            pack.a = arg1;
            IBufferWriter<byte> writer = new ArrayBufferWriter<byte>(Unsafe.SizeOf<RpcPack<T1>>());
            MemoryPack.MemoryPackSerializer.Serialize(writer, hash);
            MemoryPack.MemoryPackSerializer.Serialize(writer, pack);
            //FEvent.Client.Send(writer.GetSpan());
        }
        public static void RpcSend<T1, T2>(this IFEventOperator<ISendEvent<T1, T2>> container, T1 arg1, T2 arg2)
        {
            var mop = container.GetOperator();
            long hash = SHA64.GetHash(mop.MsgType);

            RpcPack<T1, T2> pack = new RpcPack<T1, T2>();
            //pack.hashId = hash;
            pack.a = arg1;
            pack.b = arg2;

            byte[] bytes = MemoryPack.MemoryPackSerializer.Serialize(pack);
            //FEvent.Client.Send(bytes.AsSpan());
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="container"></param>
        public static void RpcSend<T1, T2, T3>(this IFEventOperator<ISendEvent<T1, T2, T3>> container, T1 arg1, T2 arg2, T3 arg3)
        {
            var mop = container.GetOperator();
            long hash = SHA64.GetHash(mop.MsgType);

            RpcPack<T1, T2, T3> pack = new RpcPack<T1, T2, T3>();
            //pack.hashId = hash;
            pack.a = arg1;
            pack.b = arg2;
            pack.c = arg3;

            byte[] bytes = MemoryPack.MemoryPackSerializer.Serialize(pack);
            //FEvent.Client.Send(bytes.AsSpan());
        }
        public static void RpcSend<T1, T2, T3, T4>(this IFEventOperator<ISendEvent<T1, T2, T3, T4>> container, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var mop = container.GetOperator();
            long hash = SHA64.GetHash(mop.MsgType);

            RpcPack<T1, T2, T3, T4> pack = new RpcPack<T1, T2, T3, T4>();
            //pack.hashId = hash;
            pack.a = arg1;
            pack.b = arg2;
            pack.c = arg3;
            pack.d = arg4;

            byte[] bytes = MemoryPack.MemoryPackSerializer.Serialize(pack);
            //FEvent.Client.Send(bytes.AsSpan());
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="container"></param>
        public static void RpcSend<T1, T2, T3, T4, T5>(this IFEventOperator<ISendEvent<T1, T2, T3, T4, T5>> container, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var mop = container.GetOperator();
            long hash = SHA64.GetHash(mop.MsgType);

            RpcPack<T1, T2, T3, T4, T5> pack = new RpcPack<T1, T2, T3, T4, T5>();
            //pack.hashId = hash;
            pack.a = arg1;
            pack.b = arg2;
            pack.c = arg3;
            pack.d = arg4;
            pack.e = arg5;

            byte[] bytes = MemoryPack.MemoryPackSerializer.Serialize(pack);
            //FEvent.Client.Send(bytes.AsSpan());
        }
    }
}
