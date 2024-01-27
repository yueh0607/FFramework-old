using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace FFramework
{
    public class FTaskSetResultSource<T>
    {
        private FTaskSetResultSource() { }
        public FTask Task { get; set; } = null;

        private Action<T> completeAction = null;
        public Action<T> CompleteAction
        {
            get
            {
                completeAction ??= Complete;
                return completeAction;
            }
        }
        private void Complete(T operation)
        {
            Task.GetAwaiter().SetResult();
            Pool.Set<FTaskSetResultSource<T>, FTaskSetResultSourcePoolable>(this);
        }
        public static FTaskSetResultSource<T> GetFromPool()
        {
            
            return Pool.Get<FTaskSetResultSource<T>, FTaskSetResultSourcePoolable>();
        }

        public class FTaskSetResultSourcePoolable : IPoolable<FTaskSetResultSource<T>>
        {
            int IPoolable.Capacity => 1000;

            FTaskSetResultSource<T> IPoolable<FTaskSetResultSource<T>>.OnCreate()
            {
                return new FTaskSetResultSource<T>();
            }

            void IPoolable<FTaskSetResultSource<T>>.OnDestroy(FTaskSetResultSource<T> obj)
            {
               
            }

            void IPoolable<FTaskSetResultSource<T>>.OnGet(FTaskSetResultSource<T> obj)
            {
                
            }

            void IPoolable<FTaskSetResultSource<T>>.OnSet(FTaskSetResultSource<T> obj)
            {
                obj.Task = null;
            }
        }
    }
}
