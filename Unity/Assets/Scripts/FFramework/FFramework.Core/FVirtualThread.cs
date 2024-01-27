using FFramework;
using System;

namespace Assets.Scripts.FFramework
{
    public class FVirtualThread
    {

        private static readonly OIDAllocator oid_allocator = new OIDAllocator();

        private readonly long uid;


        /// <summary>
        /// FThread唯一ID(仅在进程存活期间有效)
        /// </summary>
        /// 
        public long UID => uid;

        public FVirtualThread()
        {
            uid = oid_allocator.NextID();
        }

#nullable enable
        public Action<float>? FrameCall { get; set; } = null;
#nullable disable
    }
}
