using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Net.Sockets.Kcp;
using System.Security.Cryptography;
using System.Threading.Tasks;


namespace FFramework.Network
{
    public class KcpChannel : IKcpCallback,IRentable
    {
        UdpClient udp;

       public SimpleSegManager.Kcp kcp;
       public SimpleSegManager.KcpIO kio;
        public IPEndPoint EndPoint { get; private set; }

        public KcpChannel(int port, uint conv = 2001)
            : this(port, null, conv)
        {

        }

        public KcpChannel(int port, IPEndPoint endPoint, uint conv = 2001)
        {
            udp = new UdpClient();
            kcp = new SimpleSegManager.Kcp(conv, this, null);
            kio = new SimpleSegManager.KcpIO(conv);
            this.EndPoint = endPoint;
            UdpInputKcp();
            UpdateKcp();
        }


        public async FTask Send(ReadOnlyMemory<byte> bytes)
        {
            //kcp.Send(bytes);
        }

        public async ValueTask<byte[]> RentArrayReceiveAsync()
        {
            var (buffer, avalidLength) = kcp.TryRecv();
            while (buffer == null)
            {
                await Task.Delay(10);
                (buffer, avalidLength) = kcp.TryRecv();
            }
            byte[] bytes = ArrayPool<byte>.Shared.Rent(avalidLength);
            buffer.Memory.Slice(0,avalidLength).CopyTo(bytes);
            buffer.Dispose();
            return bytes;
        }


        //从UDP端口输入数据包转发KCP
        private async void UdpInputKcp()
        {
            UdpReceiveResult result = await udp.ReceiveAsync();
            EndPoint = result.RemoteEndPoint;
            kcp.Input(result.Buffer);
            UdpInputKcp();
        }

        void IKcpCallback.Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            var s = buffer.Memory.Span.Slice(0, avalidLength).ToArray();
            udp.SendAsync(s, s.Length, EndPoint);
            buffer.Dispose();
        }

        //使用世界协调时更新KCP
        async void UpdateKcp(int ms = 10)
        {
            while (true)
            {
                kcp.Update(DateTimeOffset.UtcNow);
                await Task.Delay(ms);
            }
        }

        IMemoryOwner<byte> IRentable.RentBuffer(int length)
        {
            return MemoryPool<byte>.Shared.Rent(length);
        }
    }
}