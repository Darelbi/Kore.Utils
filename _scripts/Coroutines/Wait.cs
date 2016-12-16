// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using Kore.Utils;

namespace Kore.Coroutines
{
    public static class Wait
    {
        private static MiniPool< WaitForYieldable> waitPool = new MiniPool< WaitForYieldable>( 16);

        public static IYieldable For( float seconds)
        {
            if (seconds <= 0)
                return null;

            var item = waitPool.Acquire();
            item.waitPool = waitPool;
            item.TimeToWait = seconds;
            return item;
        }
    }

    internal class WaitForYieldable : IYieldable, ICustomYield
    {
        public float TimeToWait;
        public MiniPool< WaitForYieldable> waitPool;

        public bool HasDone()
        {
            if( TimeToWait <= 0)
            {
                waitPool.Release( this);
                return true;
            }

            return false;
        }

        public void OnYield( ICoroutineEngine engine)
        {
            engine.RegisterCustomYield( this);
        }

        public void Reset()
        {
            
        }

        public void Update( Method method)
        {
            TimeToWait -= 
                method == Method.FixedUpdate?
                UnityEngine.Time.fixedDeltaTime:
                UnityEngine.Time.deltaTime;
        }
    }
}

