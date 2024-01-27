using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FFramework.MVVM.UI
{
    public static class UI 
    {
        private static Dictionary<Type, IUIPanel> panels = new Dictionary<Type,IUIPanel>();

        //private static Stack<IUIPanel> stack = new Stack<IUIPanel>();

        /// <summary>
        /// 加载并取得面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public static async FTask<T> GetPanel<T,K>() where T:class,IUIPanel<K>,new() where K:View
        {
            T viewModel = null;
            if (panels.ContainsKey(typeof(T)))
            {
                viewModel = (T)panels[typeof(T)];
                await FTask.CompletedTask;
            }
            else
            {
                viewModel = await MVVM.Load<T, K>();
                panels.Add(typeof(T), viewModel);
            }
            return viewModel;
        }

        /// <summary>
        /// 展示UI面板
        /// </summary>
        public static async FTask Show<T,K>() where T :class, IUIPanel<K>, new() where K : View
        {
            T viewModel = await GetPanel<T,K>(); 
            await viewModel.Show();  
        }

       
        /// <summary>
        /// 隐藏UI面板
        /// </summary>
        public static async FTask Hide<T, K>() where T : class, IUIPanel<K>, new() where K : View
        {
            T viewModel = await GetPanel<T, K>();
            await viewModel.Hide();
        }

        /// <summary>
        /// 销毁并卸载面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <returns></returns>
        public static async FTask Unload<T, K>() where T : class, IUIPanel<K>, new() where K : View
        {
            await MVVM.Unload<T>(await GetPanel<T, K>());
            panels.Remove(typeof(T));
        }
    }

    public static class UIPanelEx
    {
        public static async FTask Show<T>(this IUIPanel<T> panel) where T : View
        {
            await panel.OnShow();
        }
        public static async FTask Hide<T>(this IUIPanel<T> panel) where T : View
        {
            await panel.OnHide();
        }
    }
}