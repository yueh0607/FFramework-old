using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FFramework.Buffers
{
    /// <summary>
    /// 从数组池借出的缓冲区
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class RecyclableBufferWriter<T> : IBufferWriter<T>,IDisposable
    {
        private T[] buffer = null; // 实际的缓冲区
        private int index;   // 当前写入位置


        private RecyclableBufferWriter()
        {
            
        }

        public Memory<T> GetMemory(int sizeHint = 0)
        {

            if(buffer == null)
            {
                buffer = ArrayPool<T>.Shared.Rent(sizeHint);
            }
            // 如果缓冲区不够大，可以从 ArrayPool 中借用内存块
            else if (buffer.Length < index + sizeHint)
            {
                var newSize = Math.Max(buffer.Length * 2, index + sizeHint);
                var newBuffer = ArrayPool<T>.Shared.Rent(newSize);

                // 将旧数据复制到新的内存块中
                buffer.AsSpan(0, index).CopyTo(newBuffer);

                // 归还旧的内存块
                ArrayPool<T>.Shared.Return(buffer);

                // 更新缓冲区和写入位置
                buffer = newBuffer;
            }

            // 返回可写入的内存块
            return buffer.AsMemory(index);
        }
    
        public Span<T> GetSpan(int sizeHint = 0)
        {
            return GetMemory(sizeHint).Span;
        }

        /// <summary>
        /// 获取已经写入的Span
        /// </summary>
        /// <returns></returns>
        public Span<T> GetWrittenSpan()
        {
            return buffer.AsSpan(0, index);
        }
        /// <summary>
        /// 获取已经写入的Memory
        /// </summary>
        /// <returns></returns>
        public Memory<T> GetWrittenMemory()
        {
            return buffer.AsMemory(0, index);
        }

        public void Advance(int count)
        {
            // 更新写入位置
            index += count;
        }

        public void Clear()
        {
            // 清空缓冲区
            index = 0;
        }


        public static RecyclableBufferWriter<T> Rent()
        {
            return Pool.Get<RecyclableBufferWriter<T>, RecyclableBufferWriter<T>.RecyclableBufferWriterPoolable>();
        }

        public static void Return(RecyclableBufferWriter<T> writer)
        {
            Pool.Set<RecyclableBufferWriter<T>, RecyclableBufferWriter<T>.RecyclableBufferWriterPoolable>(writer);
        }
        /// <summary>
        /// 自回收
        /// </summary>
        void IDisposable.Dispose()
        {
            Return(this);
        }

        public class RecyclableBufferWriterPoolable : IPoolable<RecyclableBufferWriter<T>>
        {
            int IPoolable.Capacity => throw new NotImplementedException();

            RecyclableBufferWriter<T> IPoolable<RecyclableBufferWriter<T>>.OnCreate()
            {
                return new RecyclableBufferWriter<T>();
            }

            void IPoolable<RecyclableBufferWriter<T>>.OnDestroy(RecyclableBufferWriter<T> obj)
            {
                ArrayPool<T>.Shared.Return(obj.buffer);
            }

            void IPoolable<RecyclableBufferWriter<T>>.OnGet(RecyclableBufferWriter<T> obj)
            {
                
            }

            void IPoolable<RecyclableBufferWriter<T>>.OnSet(RecyclableBufferWriter<T> obj)
            {
                obj.index = 0;
            }
        }
    }
}