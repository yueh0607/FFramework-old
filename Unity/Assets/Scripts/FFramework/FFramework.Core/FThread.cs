using Assets.Scripts.FFramework;
using System;
using System.Diagnostics;
using System.Threading;


namespace FFramework
{
    /// <summary>
    /// 架构线程元素，需要主动进行启动，在析构时自动中断线程的执行。
    /// </summary>
    public class FThread : FVirtualThread
    {
        private readonly Thread thread;

        /// <summary>
        /// 标识当前FThread是否存活
        /// </summary>
        public bool IsAlive => thread.IsAlive;

        private int frameCount = int.MaxValue;
        private float frameTime = -1;
        private float timeUnit = 1;

        /// <summary>
        /// 重新计算帧属性
        /// </summary>
        private void ReCalFrameProperties()
        {
            frameTime = timeUnit / frameCount;
        }

        /// <summary>
        /// 帧数量默认为int.MaxValue，范围[1,int.MaxValue]
        /// </summary>
        public int FrameCount
        {
            get => frameCount;
            set
            {
                frameCount = Math.Clamp(value, 1, int.MaxValue);
                ReCalFrameProperties();
            }
        }
        /// <summary>
        /// 时间单元默认=1f ，范围[1e-6f,float.MaxValue]
        /// </summary>
        public float TimeUnit
        {
            get => timeUnit;
            set
            {
                timeUnit = Math.Clamp(value, 1e-6f, float.MaxValue);
                ReCalFrameProperties();
            }
        }

        /// <summary>
        /// 帧时间 = 时间单元 / 帧数量  
        /// </summary>
        public float FrameTime => frameTime;

        /// <summary>
        /// 设置每秒帧率，此方法将会修改TimeUnit以及FrameCount
        /// </summary>
        /// <param name="frameCount"></param>
        public void SetFrameCountPerSecond(int frameCount)
        {
            timeUnit = 1;
            FrameCount = frameCount;
        }

        public FThread()
        {
            ReCalFrameProperties();
            thread = new Thread(ThreadLoop);
        }
        ~FThread()
        {
            //（封装屏蔽用户对thread的操作以确保不会发生异常）
            if (thread != null && thread.IsAlive)
            {
                //先阻塞，再中断
                thread.Join();
                thread.Interrupt();
            }
        }


        ///<summary> 开启线程 </summary>
        /// <exception cref="OutOfMemoryException">如果开启线程的内存不足，则抛出此异常</exception>
        /// <exception cref="ThreadStateException">如果线程已经开启，则抛出此异常</exception>
        public void Start()
        {
            thread.Start();
        }

        /// <summary>
        /// 线程主循环
        /// </summary>
        public void ThreadLoop()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            float deltaTime = 0;
            while (true)
            {
                deltaTime += watch.ElapsedMilliseconds / 1000f;
                if (deltaTime >= frameTime)
                {
                    base.FrameCall?.Invoke(deltaTime);
                    deltaTime = 0f;
                }
            }
        }

    }
}
