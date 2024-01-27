using System.Collections;

namespace FFramework.Internal
{
    public abstract class Container
    {
        protected Queue pool = new Queue();

        public int Count => pool.Count;
        public abstract object Get();

        public abstract void Set(object obj);

        public abstract void Release();
    }


    public class Container<T> : Container
    {
        IPoolable<T> poolable;
        public Container(IPoolable<T> poolable)
        {
            this.poolable = poolable;
        }

        public override object Get()
        {
            if (pool.Count == 0)
            {
                T t_obj = poolable.OnCreate();
                poolable.OnGet(t_obj);
                return t_obj;
            }
            object obj = pool.Dequeue();
            poolable.OnGet((T)obj);
            return obj;
        }

        public override void Release()
        {
            while (pool.Count != 0)
            {
                var obj = (T)pool.Dequeue();
                poolable.OnDestroy(obj);
            }
        }

        public override void Set(object obj)
        {
            poolable.OnSet((T)obj);
            if (pool.Count < poolable.Capacity)
            {
                pool.Enqueue(obj);
            }
            else
            {
                poolable.OnDestroy((T)obj);
            }
        }
    }
}

