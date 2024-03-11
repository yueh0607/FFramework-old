using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public class FUnit 
    { 

        static OIDAllocator oidAllocator = new OIDAllocator();
        long _id;
        public long ID => _id;
        public FUnit()
        {
            _id = oidAllocator.NextID();
        }


    }
}
