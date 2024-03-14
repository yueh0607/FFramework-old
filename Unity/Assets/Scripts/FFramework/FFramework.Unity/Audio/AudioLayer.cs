using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace FFramework.Audio
{
    public class AudioLayer
    {
    
        Dictionary<string, AudioLayer> childLayers = new Dictionary<string, AudioLayer>();

        private string _path;

        private string _name;
        /// <summary>
        /// 层名
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// 路径
        /// </summary>
        public string Path => _path;

        private AudioLayer parentLayer = null;

        /// <summary>
        /// 父层级
        /// </summary>
        public AudioLayer ParentLayer => parentLayer;

        public AudioLayer(string name,string path)
        {
            _path = path;
            _name = name;
        }


        private float _volume = 1;
        /// <summary>
        /// 音量(0-1)
        /// </summary>
        public float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = Math.Clamp(value, 0, 1);
            }
        }



        /// <summary>
        /// 添加子音效层
        /// </summary>
        /// <param name="layer"></param>
        public void AddChildLayer(AudioLayer layer)
        {
            //存在则冲突
            if (childLayers.ContainsKey(layer.Name)) throw new InvalidOperationException($"Layer Name Conflict: {layer.Name}");
            childLayers.Add(layer.Name,layer);
            layer.parentLayer = this;
        }

        /// <summary>
        /// 移除子音效层
        /// </summary>
        /// <param name="layer"></param>
        public void RemoveChildLayer(AudioLayer layer)
        {
            //不存在则异常
            if (!childLayers.ContainsKey(layer.Name)) throw new InvalidOperationException($"Layer not exist: {layer.Name}");
            childLayers.Remove(layer.Name);
            layer.parentLayer = null;
        }


        public AudioLayer this[string name]
        {
            get
            {
                if (childLayers.ContainsKey(name)) return childLayers[name];
                return null;
            }
        }

        /// <summary>
        /// 通知列表
        /// </summary>
        public List<AudioSource> NotifyList { get; private set; } = new List<AudioSource>();


    }
}

