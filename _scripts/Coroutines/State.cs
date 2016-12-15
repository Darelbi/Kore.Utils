// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using System.Collections;

namespace Kore.Coroutines
{
    public static class State 
    {
        public static IYieldable Change( IEnumerator state)
        {
            return new StateChangeYieldable( state);
        }

        // Allows fine control over states to squize maximum performance
        public static StateCache Cache()
        {
            return new StateCache();
        }
    }

    internal class StateChangeYieldable : IYieldable
    {
        IEnumerator _nextState;

        public StateChangeYieldable( IEnumerator nextState)
        {
            _nextState = nextState;
        }

        public void OnYield( ICoroutineEngine engine)
        {
            engine.ReplaceCurrentWith( _nextState);
        }
    }

    internal class StateEnterYieldable : IYieldable
    {
        public delegate void EnterCallback();

        EnterCallback _callback;
        bool executed;

        public StateEnterYieldable( EnterCallback callback)
        {
            _callback = callback;
        }

        public void OnYield( ICoroutineEngine engine)
        {
            if (executed)
                return;

            _callback();
            executed = true;
        }
    }
}
