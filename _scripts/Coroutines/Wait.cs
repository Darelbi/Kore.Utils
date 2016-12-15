// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

namespace Kore.Coroutines
{
    public static class Wait
    {
        public static IYieldable For( float seconds)
        {
            if (seconds <= 0)
                return null;

            return new WaitForYieldable( seconds);
        }
    }

    internal class WaitForYieldable : IYieldable, ICustomYield
    {
        float _timeToWait;

        public WaitForYieldable( float timeToWait)
        {
            _timeToWait = timeToWait;
        }

        public bool HasDone()
        {
            return _timeToWait <= 0;
        }

        public void OnYield( ICoroutineEngine engine)
        {
            engine.RegisterCustomYield( this);
        }

        public void Update( Method method)
        {
            _timeToWait -= 
                method == Method.FixedUpdate?
                UnityEngine.Time.fixedDeltaTime:
                UnityEngine.Time.deltaTime;
        }
    }
}

