using System.Collections.Generic;

namespace Kore.Utils
{
    /// <summary>
    /// General purpose object pool, used to avoid generating too much
    /// garbage by caching object and resetting them when they are
    /// created.
    /// </summary>
    /// <typeparam name="T">Poolable (Reset method) class with new operator</typeparam>
    class MiniPool<T> where T: IPoolable, new()
    {
        // Stack is slightly faster than a Queue: I guess because Pushing/Popping
        // from a stack access the same memory location, thus has slightly more
        // cache locality than a Queue (which access 2 different 
        // memory locations and eventually jump back when reach end of array).
        Stack<T> pool;

        public MiniPool( int initialCapacity = 2)
        {
            pool = new Stack< T>( initialCapacity);
        }

        /// <summary>
        /// Get a Resetted object from the pool, you have to return it back
        /// </summary>
        public T Acquire()
        {
            T obj;

            if( pool.Count == 0)
                (obj = new T()).Reset();

            else
                obj = pool.Pop();                

            return obj;
        }

        /// <summary>
        /// There is no need to "Release" the "Current" object, however you cannot
        /// Acquire another object as long as you are using "Current".
        /// If in doubt DON'T USE THIS: it is just a optimization for few cases.
        /// </summary>
        public T Current()
        {
            if (pool.Count == 0)
            {
                T temp = new T();
                temp.Reset();
                pool.Push( temp);
            }

            T obj = pool.Peek();

            return obj;
        }

        public void Release( T item)
        {
            item.Reset();
            pool.Push( item);
        }
    }
}
