using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFramework
{
    public abstract class FUnit 
    { 

        static OIDAllocator oidAllocator = new OIDAllocator();
        long _id;
        public long ID => _id;
        public FUnit()
        {
            _id = oidAllocator.NextID();
#if UNITY_EDITOR
            funit.Add(new WeakReference(this));

#endif
        }

#if UNITY_EDITOR
        ~FUnit()
        {
            constructs.Add((ID, this.GetType().GetDisplayName()));
        }
        static List<WeakReference> funit = new List<WeakReference>();
        static List<(long id, string type)> constructs = new List<(long id, string type)>();
        /// <summary>
        /// 仅编辑器可用
        /// </summary>
        public bool IsInPool { get; set; } = false;

        public static List<(long id, string type)> GetConstructs()
        {
            return constructs;
        }
        public static List<WeakReference> GetWeakReferences()
        {
            List<WeakReference> weaks = new List<WeakReference>();
            for(int i=0;i<funit.Count; i++)
            {
                if (funit[i].IsAlive)
                {
                    weaks.Add(funit[i]);
                }
            }
            funit = weaks;
            return weaks;
        }
#endif

      

    }
}
