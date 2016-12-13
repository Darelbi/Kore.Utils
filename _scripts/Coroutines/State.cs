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
}
