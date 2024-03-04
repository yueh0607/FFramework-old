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
    public static class MV
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
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TModel GetModel<TModel>() where TModel : class, IModel
        {
            return SingletonProperty<TModel>.Instance;
        }
        /// <summary>
        /// 释放预制体资源句柄
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        public static void Release<TView>() where TView : View
        {
            if (prefabCache.ContainsKey(typeof(TView)))
            {
                prefabCache[typeof(TView)].Release();
                prefabCache.Remove(typeof(TView));
            }
        }

        /// <summary>
        /// 加载并实例化一个VM-V
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TView"></typeparam>
        /// <param name="active"></param>
        /// <returns></returns>
        public static async FTask<TViewModel> Load<TViewModel, TView>(EInitActive active = EInitActive.Any) where TViewModel : class, IViewModel<TView>, new() where TView : View
        {
            GameObject instance = null;

            //检查预制体资源句柄
            if (!prefabCache.ContainsKey(typeof(TView)))
            {
                prefabCache.Add(typeof(TView), Package.LoadAssetAsync(LoadConverter(typeof(TView))));
            }
            var handle = prefabCache[typeof(TView)];

            if (handle.IsValid)
            {
                if (!handle.IsDone)
                    await handle;
            }
            else
            {
                prefabCache[typeof(TView)].Release();
                handle = Package.LoadAssetAsync(LoadConverter(typeof(TView)));
                prefabCache[typeof(TView)] = handle;
                await handle;
            }
            var insHandle = handle.InstantiateAsync();
            await insHandle;
            instance = insHandle.Result;

            if (EInitActive.Active == active)
                instance.SetActive(true);
            else if (active == EInitActive.NotActive)
                instance.SetActive(false);

            TView view = instance.AddComponent<TView>();
            TViewModel viewmodel = new TViewModel();
            viewmodel.TView = view;
            await viewmodel.OnLoad();
            return viewmodel; 
        }

        /// <summary>
        /// 从池加载
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <typeparam name="TView"></typeparam>
        /// <param name="active"></param>
        /// <returns></returns>
        public static async FTask<TViewModel> LoadFromPool<TViewModel, TView, TPoolable>(EInitActive active = EInitActive.Any) where TViewModel : class, IViewModel<TView>, new() where TView : View where TPoolable : IPoolable<TViewModel>, new()
        {
            GameObject instance = null;

            //检查预制体资源句柄
            if (!prefabCache.ContainsKey(typeof(TView)))
            {
                 prefabCache.Add(typeof(TView), Package.LoadAssetAsync(LoadConverter(typeof(TView))));
            }
            var handle = prefabCache[typeof(TView)];

            if (handle.IsValid)
            {
                if (!handle.IsDone)
                    await handle;
            }
            else
            {
                prefabCache[typeof(TView)].Release();
                handle = Package.LoadAssetAsync(LoadConverter(typeof(TView)));
                prefabCache[typeof(TView)] = handle;
                await handle;
            }


            //检查孵化器
            if (!spawners.ContainsKey(typeof(TViewModel)))
            {
                Spawner sp = new Spawner(handle);
                sp.OnCreate = null;
                sp.OnDestroy = null;
                sp.OnGet = null;
                sp.OnSet = null;
                spawners.Add(typeof(TViewModel), sp);
            }
            Spawner spawner = spawners[typeof(TViewModel)];




            var insHandle = spawner.GetAsync();

            instance = await insHandle;

            if (EInitActive.Active == active)
                instance.SetActive(true);
            else if (active == EInitActive.NotActive)
                instance.SetActive(false);

            TView view = instance.GetComponent<TView>() ?? instance.AddComponent<TView>();


            TViewModel viewmodel = Pool.Get<TViewModel, TPoolable>();
            viewmodel.TView = view;

            await viewmodel.OnLoad();
            return viewmodel;
        }




        /// <summary>
        /// 卸载并销毁一个VM-V
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static async FTask Unload<TViewModel>(TViewModel viewModel) where TViewModel : IViewModel
        {
            await viewModel.OnUnload();
            GameObject.Destroy(viewModel.View.gameObject);
        }

        /// <summary>
        /// 卸载并销毁一个VM-V
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static async FTask UnloadToPool<TViewModel, TPoolable>(TViewModel viewModel) where TViewModel : IViewModel where TPoolable : IPoolable<TViewModel>, new()
        {
            await viewModel.OnUnload();
            if (spawners.ContainsKey(typeof(TViewModel)))
            {
                Spawner spawner = spawners[typeof(TViewModel)];
                await spawner.SetAsync(viewModel.View.gameObject);
            }
            else
            {
                GameObject.Destroy(viewModel.View.gameObject);
            }
            Pool.Set<TViewModel, TPoolable>(viewModel);
        }

    }
}
