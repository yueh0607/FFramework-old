using FFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

namespace FFramework
{
    public class Spawner : IUpdate
    {

        AssetHandle handle;
        public Spawner(AssetHandle handle)
        {
            this.handle = handle;
        }
        private static async FTask OnGetDefault(GameObject go)
        {
            go.SetActive(true);
            await FTask.CompletedTask;
        }
        private static async FTask OnSetDefault(GameObject go)
        {
            go.SetActive(false);
            await FTask.CompletedTask;
        }
        private static async FTask OnDesDefault(GameObject go)
        {
            GameObject.Destroy(go);
            await FTask.CompletedTask;
        }
        public Func<GameObject, FTask> OnCreate { get; set; } = null;
        public Func<GameObject, FTask> OnSet { get; set; } = OnSetDefault;
        public Func<GameObject, FTask> OnGet { get; set; } = OnGetDefault;
        public Func<GameObject, FTask> OnDestroy { get; set; } = OnDesDefault;

        Queue<GameObject> objs = new Queue<GameObject>();

        //池自动维持大于最小数量以防备突然的增长（估计使用数量最小值即可）
        public int MinCount { get; set; } = 0;

        //池会维护最大数量不高于此值，防止内存占用过大（估计使用数量最大值即可）
        public int MaxCount = int.MaxValue;


        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public async FTask<GameObject> GetAsync()
        {
            if (isDestroing) throw new InvalidOperationException("This spawner is destroing");
            if (objs.Count == 0) await Create();
            GameObject go = objs.Dequeue();

            FTask task = OnGet?.Invoke(go);
            if (task != null) await task;
            else await FTask.CompletedTask;
            return go;
        }

        /// <summary>
        /// 放回
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async FTask SetAsync(GameObject obj)
        {
            if (isDestroing) throw new InvalidOperationException("This spawner is destroing");
            if (objs.Count >= MaxCount)
            {
                var task = OnDestroy?.Invoke(obj);
                if (task == null) await FTask.CompletedTask;
                else await task;
                return;
            }
            var task2 = OnSet?.Invoke(obj);
            if (task2 == null) await FTask.CompletedTask;
            else await task2;

            objs.Enqueue(obj);
        }


        /// <summary>
        /// 异步向队列添加对象
        /// </summary>
        /// <returns></returns>
        private async FTask Create()
        {
            var insHandle = handle.InstantiateAsync();
            await insHandle;
            OnCreate?.Invoke(insHandle.Result);
            objs.Enqueue(insHandle.Result);
        }

        bool isDestroing = false;
        ~Spawner()
        {
            isDestroing = true;
            foreach (var item in objs)
            {
                var task = OnDestroy?.Invoke(item);
                task.Coroutine();
            }
            objs.Clear();
        }

        void IUpdate.Update(float deltaTime)
        {
            if (objs.Count < MinCount)
            {
                Create().Coroutine();
            }
        }
    }
}