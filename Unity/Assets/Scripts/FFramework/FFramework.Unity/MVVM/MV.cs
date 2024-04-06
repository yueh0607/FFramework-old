using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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
        private static Dictionary<Type, Dictionary<long, IModel>> models = new Dictionary<Type, Dictionary<long, IModel>>();
        
        /// <summary>
        /// 预制体资源包
        /// </summary>
        public static ResourcePackage Package { get; set; } = null;
        /// <summary>
        /// 获取模型
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static TModel GetModel<TModel>(long id = 0) where TModel : class, IModel
        {
            InitModel(typeof(TModel), id);
            return (TModel)models[typeof(TModel)][id];
        }

        /// <summary>
        /// 根据类型获取Model，未创建则返回null，不作检查
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IModel GetModelByType(Type type, long id = 0)
        {
            if (!models.ContainsKey(type)) return null;
            if (!models[type].ContainsKey(id)) return null;
            return models[type][id];
        }
        /// <summary>
        /// 立刻初始化一个Model
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public static void InitModel(Type type, long id = 0)
        {
            if (!typeof(IModel).IsAssignableFrom(type)) throw new InvalidOperationException("type is not IModel");
            //不存在分级
            if (!models.ContainsKey(type))
            {
                models.Add(type, new Dictionary<long, IModel>());
            }
            //不存在子集
            if (!models[type].ContainsKey(id))
            {
                models[type].Add(id, (IModel)Activator.CreateInstance(type));
            }
        }

        /// <summary>
        /// 获取此类型的全部Model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static Dictionary<long, IModel> GetModels<TModel>() where TModel : class, IModel
        {
            return GetModels(typeof(TModel));
        }
        /// <summary>
        /// 获取此类型的全部Model
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Dictionary<long, IModel> GetModels(Type type)
        {
            if (!typeof(IModel).IsAssignableFrom(type)) return new Dictionary<long, IModel>();
            //不存在分级
            if (!models.ContainsKey(type))
            {
                models.Add(type, new Dictionary<long, IModel>());
            }
            return models[type];
        }

        public static void DestroyModels<TModel>() where TModel : class, IModel
        {
            //Remove
            DestroyModels(typeof(TModel));
        }
        public static void DestroyModels(Type type)
        {
            //Remove
            if (models.ContainsKey(type))
            {
                models.Remove(type);
            }
        }

        public static void DestroyModel<TModel>(long id = 0) where TModel : class, IModel
        {
            //Remove
            DestroyModel(typeof(TModel), id);
        }
        public static void DestroyModel(Type type, long id = 0)
        {
            //Remove
            if (models.ContainsKey(type))
            {
                if (models[type].ContainsKey(id))
                {
                    models[type].Remove(id);
                }
            }
        }

        /// <summary>
        /// 存档
        /// </summary>
        /// <param name="saveOrder">存档ID</param>
        /// <param name="modelPreFrame">每帧保存多少个</param>
        /// <returns></returns>
        public static FTask SaveModels(int saveOrder = 0)
        {


            string path = Application.persistentDataPath + $"/Saves/{saveOrder}.save";
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            CancellationTokenSource cts = new CancellationTokenSource();
            Interlocked.MemoryBarrier();
            Task task = Task.Run(() =>
            {
                
                //ModelTypeName          -> ID -> SaveFieldName -> Value
                Dictionary<string, Dictionary<long, Dictionary<string, string>>> saveMap = new Dictionary<string, Dictionary<long, Dictionary<string, string>>>();

                //遍历类型
                foreach (var kvp in models)
                {
                    if (kvp.Key.GetCustomAttribute<ModelSaveIgnoreAttribute>() != null) continue;
                    string typeSign = $"{kvp.Key.FullName},{kvp.Key.Module}";
                    //添加类型映射
                    saveMap.Add(typeSign, new Dictionary<long, Dictionary<string, string>>());
                    foreach (var kvp2 in kvp.Value)
                    {
                        
                        //添加id映射
                        saveMap[typeSign].Add(kvp2.Key, new Dictionary<string, string>());
                        //获取所有字段
                        FieldInfo[] infos = kvp.Key.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        foreach (var fi in infos)
                        {
                            //字段如果确认保存
                            SaveAttribute st = fi.GetCustomAttribute<SaveAttribute>();
                            if (st != null)
                            {
                                string str = ModelSerialize.Serialize(fi.GetValue(kvp2.Value));
                                saveMap[typeSign][kvp2.Key].Add(st.SaveName, str);
                            }
                        }

                    }
                }

                //转JSON字符串
                string jsonFormatString = JsonConvert.SerializeObject(saveMap, Formatting.Indented);

                //覆盖写入
                StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8);
                writer.Write(jsonFormatString);
                writer.Dispose();
                Debug.Log("存档完成!");
            });
            return FTask.ToFTask(task, cts);
        }
        /// <summary>
        /// 读档
        /// </summary>
        /// <param name="saveOrder"></param>
        /// <returns></returns>
        public static FTask ReadSaves(int saveOrder = 0)
        {
            string path = Application.persistentDataPath + $"/Saves/{saveOrder}.save";
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            CancellationTokenSource cts = new CancellationTokenSource();
            if (!File.Exists(path)) return FTask.ToFTask(Task.CompletedTask, cts);

            Task task = Task.Run(() =>
            {
                //反序列化为映射字典
                Dictionary<string, Dictionary<long, Dictionary<string, string>>> saveMap;
                StreamReader reader = new StreamReader(path, System.Text.Encoding.UTF8, false);
                string json = reader.ReadToEnd();
                reader.Dispose();
                saveMap = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<long, Dictionary<string, string>>>>(json);


                //从映射字典解析
                foreach (var kvp in saveMap)
                {
                    //解析Model名称
                    Type type = Type.GetType(kvp.Key);
                    if (type == null) continue;

                    //恢复原始Model
                    if (!models.ContainsKey(type))
                    {
                        models.Add(type, new Dictionary<long, IModel>());
                    }
                    foreach (var kvp2 in kvp.Value)
                    {
                        long id = kvp2.Key;

                        //恢复原始Model
                        if (!models[type].ContainsKey(id))
                        {
                            models[type].Add(id, (IModel)Activator.CreateInstance(type));
                        }

                        //指定Model
                        IModel model = models[type][id];
                        FieldInfo[] infos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                        foreach (var fi in infos)
                        {
                            SaveAttribute st = fi.GetCustomAttribute<SaveAttribute>();
                            if (st != null && kvp2.Value.ContainsKey(st.SaveName))
                            {
                                //恢复值
                                fi.SetValue(model, ModelSerialize.DeSerialize(kvp2.Value[st.SaveName], fi.FieldType));
                            }
                        }

                    }
                }

                Debug.Log("读档完成!");
            });
            return FTask.ToFTask(task, cts);
        }

        /// <summary>
        /// 删档
        /// </summary>
        /// <param name="saveOrder"></param>
        public static void DeleteSaves(int saveOrder = 0)
        {
            string path = Application.persistentDataPath + $"/Saves/{saveOrder}.save";
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            if (File.Exists(path)) File.Delete(path);
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
            view.ViewModelReference = viewmodel;
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
            view.ViewModelReference = viewmodel;
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
