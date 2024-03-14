using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;


namespace FFramework.Audio
{
    public static class FAudio
    {
        public static ResourcePackage Package { get; set; } = null;
        private static Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

        private static void Check()
        {
            if (Package == null) throw new System.InvalidOperationException("Please point a package for FAudio");
        }

        private static async FTask TryLoad(string asset)
        {
            if (clips.ContainsKey(asset))
            {
                await FTask.CompletedTask;
                return;
            }

            var handle = Package.LoadAssetAsync(asset);
            await handle;
            clips.Add(asset, (AudioClip)handle.AssetObject);
        }


        /// <summary>
        /// 音源包装器
        /// </summary>
        class SourceWarpper : FUnit
        {
            public GameObject obj;
            public AudioSource source;



            public SourceWarpper()
            {
                this.obj = new GameObject();
                this.source = obj.AddComponent<AudioSource>();
                source.playOnAwake = false;
            }

            ~SourceWarpper()
            {
                source.Pause();
                GameObject.Destroy(obj);
            }

            public class SourceWarpperPoolable : IPoolable<SourceWarpper>
            {
                int IPoolable.Capacity => 10000;

                SourceWarpper IPoolable<SourceWarpper>.OnCreate()
                {
                    return new SourceWarpper();
                }

                void IPoolable<SourceWarpper>.OnDestroy(SourceWarpper obj)
                {

                }

                void IPoolable<SourceWarpper>.OnGet(SourceWarpper obj)
                {

                }

                void IPoolable<SourceWarpper>.OnSet(SourceWarpper obj)
                {

                }
            }
        }


        //播放中
        static Dictionary<long, SourceWarpper> s_warp = new Dictionary<long, SourceWarpper>();


        /// <summary>
        /// 在某一层播放音效（不应该等待）
        /// </summary>
        /// <param name="address">资源地址</param>
        /// <param name="layer">层级</param>
        /// <param name="loop">循环状态</param>
        /// <returns></returns>
        public static async FTask Play(string address, string layer)
        {
            Check();
            //尝试取得Layer
            AudioLayer layerIns = GetLayer(layer) ?? GlobalLayer;
            //可能存在的资源加载
            await TryLoad(address);
            AudioClip clip = clips[address];

            //拿一个包装器
            var warpper = Pool.Get<SourceWarpper, SourceWarpper.SourceWarpperPoolable>();
            s_warp.Add(warpper.ID, warpper);

            //设置片段
            warpper.source.clip = clip;

            //播放
            warpper.source.Play();

            //等待播放完成
            var waitTask = FTask.Delay(clip.length);
            var token = waitTask.Token;
            await waitTask;

            token.OnCancel += () =>
                {
                    //回收
                    warpper.source.Stop();
                    warpper.source.clip = null;
                    Pool.Set(warpper);
                };



        }

        private static void Token_OnCancel()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 在某一层的指定位置播放音效
        /// </summary>
        /// <param name="address">资源地址</param>
        /// <param name="layer">层级</param>
        /// <param name="loop">循环状态</param>
        /// <returns></returns>
        public static async FTask PlayAt(string address, string layer, Vector3 position, bool loop = false)
        {
            Check();
            //尝试取得Layer
            AudioLayer layerIns = GetLayer(layer) ?? GlobalLayer;
            //可能存在的资源加载
            await TryLoad(address);
            AudioClip clip = clips[address];

            //拿一个包装器
            var warpper = Pool.Get<SourceWarpper, SourceWarpper.SourceWarpperPoolable>();
            warpper.obj.transform.position = position;

            //设置片段
            warpper.source.clip = clip;

            //播放
            warpper.source.Play();


        }

        /// <summary>
        /// 在某一层播放音效(注意会给物体添加组件)
        /// </summary>
        /// <param name="address">资源地址</param>
        /// <param name="layer">层级</param>
        /// <param name="loop">循环状态</param>
        /// <returns></returns>
        public static async FTask PlayFollow(string address, Transform target, string layer)
        {


            //if (target != null) warpper.obj.transform.SetParent(target, true);

        }



        /// <summary>
        /// 打断某个音效
        /// </summary>
        /// <param name="address">资源地址</param>
        /// <param name="layer">音频层</param>
        /// <returns></returns>
        public static async FTask Stop(string address, string layer)
        {

        }


        /// <summary>
        /// 根音频层
        /// </summary>
        static AudioLayer rootLayer = new AudioLayer("_Global", string.Empty);
        //缓存音频层
        static Dictionary<string, AudioLayer> layers = new Dictionary<string, AudioLayer>();

        /// <summary>
        /// 尝试创建按路径创建一个音效层
        /// </summary>
        /// <returns></returns>
        public static void CreateLayer(string path, string parentLayer = null)
        {
            string[] parts = path.Split('/');
            for (int i = 0; i < parts.Length; i++)
            {
                //尝试获取音频层
                AudioLayer layer = rootLayer[parts[i]];
                //不存在则创建
                if (layer == null)
                {
                    var nlayer = new AudioLayer(parts[i], path);
                    rootLayer.AddChildLayer(nlayer);
                    layers.Add(path, nlayer);
                }
            }
        }
        /// <summary>
        /// 获取音频层，路径任意点不存在返回null
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AudioLayer GetLayer(string path)
        {
            if (layers.ContainsKey(path)) return layers[path];
            return null;
        }

        /// <summary>
        /// 全局音频层，可以设置全局音量
        /// </summary>
        public static AudioLayer GlobalLayer => rootLayer;
    }

}
