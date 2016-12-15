// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using Kore.Utils;
using System.Collections;

namespace Kore.Coroutines
{
    public class StateCache
    {
        CachedStateChangeYieldable _change;
        bool executed;

        internal StateCache()
        {
            _change = new CachedStateChangeYieldable();
        }

        public IYieldable EnterState( KoreCallback onEnterState)
        {
            if (executed == false)
            {
                onEnterState();
                executed = true;
            }

            return null;
        }

        public IYieldable Change( IEnumerator nextState)
        {
            executed = false;
            _change._nextState = nextState;
            return _change;
        }

        public IYieldable Change( IEnumerator nextState, KoreCallback onExitState)
        {
            executed = false;
            onExitState();
            _change._nextState = nextState;
            return _change;
        }
    }

    internal class CachedStateChangeYieldable : IYieldable
    {
        public IEnumerator _nextState;

        public void OnYield( ICoroutineEngine engine)
        {
            engine.ReplaceCurrentWith(_nextState);
        }
    }
}
