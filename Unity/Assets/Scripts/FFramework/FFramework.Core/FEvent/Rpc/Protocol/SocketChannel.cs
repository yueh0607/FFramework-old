using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FFramework.Network
{
    public abstract class SocketClient
    {
        public abstract ValueTask Send(IBufferWriter<byte> writer);

        public abstract ValueTask<IBufferWriter<byte>> Receive();
    }
}