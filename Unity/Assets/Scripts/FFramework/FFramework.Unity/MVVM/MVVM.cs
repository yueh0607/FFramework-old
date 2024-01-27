using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace FFramework.MVVM
{
    public enum EInitActive
    {
        Any,
        Active,
        NotActive
    }
    public static class MVVM
    {
        /// <summary>
        /// 根据类型取得加载String
        /// </summary>
        public static Func<Type, string> LoadConverter = (x) => x.Name;
        private static Dictionary<Type, AssetHandle> prefabCache = new Dictionary<Type, AssetHandle>();
        private static Dictionary<Type, Spawner> spawners = new Dictionary<Type, Spawner>();

        /// <summary>
        /// 预制体资源包
        /// </summary>
        public static ResourcePackage Package { get; set; } = null;
        /// <summary>
        /// 获取模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetModel<T>() where T : class, IModel
        {
            return SingletonProperty<T>.Instance;
        }
        /// <summary>
        /// 释放预制体资源句柄
        /// </summary>
        /// <typeparam name="K"></typeparam>
        public static void Release<K>() where K : View
        {
            if (prefabCache.ContainsKey(typeof(K)))
            {
                prefabCache[typeof(K)].Release();
                prefabCache.Remove(typeof(K));
            }
        }

        /// <summary>
        /// 加载并实例化一个VM-V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="active"></param>
        /// <returns></returns>
        public static async FTask<T> Load<T, K>(EInitActive active = EInitActive.Any) where T : class, IViewModel<K>, new() where K : View
        {
            GameObject instance = null;

            //检查预制体资源句柄
            if (!prefabCache.ContainsKey(typeof(K)))
            {
                prefabCache.Add(typeof(K), Package.LoadAssetAsync(LoadConverter(typeof(K))));
            }
            var handle = prefabCache[typeof(K)];

            if (handle.IsValid)
            {
                if (!handle.IsDone)
                    await handle;
            }
            else
            {
                prefabCache[typeof(K)].Release();
                handle = Package.LoadAssetAsync(LoadConverter(typeof(K)));
                prefabCache[typeof(K)] = handle;
                await handle;
            }
            var insHandle = handle.InstantiateAsync();
            await insHandle;
            instance = insHandle.Result;

            if (EInitActive.Active == active)
                instance.SetActive(true);
            else if (active == EInitActive.NotActive)
                instance.SetActive(false);

            K view = instance.AddComponent<K>();
            T viewmodel = new T();
            viewmodel.TView = view;
            await viewmodel.OnLoad();
            return viewmodel;
        }

        /// <summary>
        /// 从池加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="active"></param>
        /// <returns></returns>
        public static async FTask<T> LoadFromPool<T, K, P>(EInitActive active = EInitActive.Any) where T : class, IViewModel<K>, new() where K : View where P : IPoolable<T>, new()
        {
            GameObject instance = null;

            //检查预制体资源句柄
            if (!prefabCache.ContainsKey(typeof(K)))
            {
                prefabCache.Add(typeof(K), Package.LoadAssetAsync(LoadConverter(typeof(K))));
            }
            var handle = prefabCache[typeof(K)];

            if (handle.IsValid)
            {
                if (!handle.IsDone)
                    await handle;
            }
            else
            {
                prefabCache[typeof(K)].Release();
                handle = Package.LoadAssetAsync(LoadConverter(typeof(K)));
                prefabCache[typeof(K)] = handle;
                await handle;
            }


            //检查孵化器
            if (!spawners.ContainsKey(typeof(T)))
            {
                Spawner sp = new Spawner(handle);
                sp.OnCreate = null;
                sp.OnDestroy = null;
                sp.OnGet = null;
                sp.OnSet = null;
                spawners.Add(typeof(T), sp);
            }
            Spawner spawner = spawners[typeof(T)];




            var insHandle = spawner.GetAsync();

            instance = await insHandle;

            if (EInitActive.Active == active)
                instance.SetActive(true);
            else if (active == EInitActive.NotActive)
                instance.SetActive(false);

            K view = instance.GetComponent<K>() ?? instance.AddComponent<K>();


            T viewmodel = Pool.Get<T, P>();
            viewmodel.TView = view;

            await viewmodel.OnLoad();
            return viewmodel;
        }




        /// <summary>
        /// 卸载并销毁一个VM-V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static async FTask Unload<T>(T viewModel) where T : IViewModel
        {
            await viewModel.OnUnload();
            GameObject.Destroy(viewModel.View.gameObject);
        }

        /// <summary>
        /// 卸载并销毁一个VM-V
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static async FTask UnloadToPool<T, P>(T viewModel) where T : IViewModel where P : IPoolable<T>, new()
        {
            await viewModel.OnUnload();
            if (spawners.ContainsKey(typeof(T)))
            {
                Spawner spawner = spawners[typeof(T)];
                await spawner.SetAsync(viewModel.View.gameObject);
            }
            else
            {
                GameObject.Destroy(viewModel.View.gameObject);
            }
            Pool.Set<T, P>(viewModel);
        }

    }
}
