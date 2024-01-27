using MemoryPack;
using System.Collections;
using System.Collections.Generic;


namespace FFramework.Rpc
{
    [MemoryPackable]
    public partial struct RpcPack
    {
        

    }
    [MemoryPackable]
    public partial struct RpcPack<T0>
    {
 
        public T0 a;
    }
    [MemoryPackable]
    public partial struct RpcPack<T0,T1>
    {
        
        public T0 a;
        public T1 b;
    }
    [MemoryPackable]
    public partial struct RpcPack<T0, T1, T2>
    {

        public T0 a;
        public T1 b;
        public T2 c;
    }
    [MemoryPackable]
    public partial struct RpcPack<T0, T1, T2, T3>
    {
     
        public T0 a;
        public T1 b;
        public T2 c;
        public T3 d;
    }
    [MemoryPackable]
    public partial struct RpcPack<T0, T1, T2, T3,T4>
    {
       
        public T0 a;
        public T1 b;
        public T2 c;
        public T3 d;
        public T4 e;
    }
    [MemoryPackable]
    public partial struct RpcPack<T0, T1, T2, T3, T4,T5>
    {
    
        public T0 a;
        public T1 b;
        public T2 c;
        public T3 d;
        public T4 e;
        public T5 f;
    }
}