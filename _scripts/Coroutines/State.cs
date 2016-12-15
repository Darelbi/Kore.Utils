// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using System.Collections;
using Kore.Utils;

namespace Kore.Coroutines
{
    public static class State 
    {
        private static MiniPool< StateChangeYieldable> changePool = new MiniPool< StateChangeYieldable>(2);

        public static IYieldable Change( IEnumerator state)
        {
            var item = changePool.Current();
            item.NextState = state;
            return item;
        }

        // Allows fine control over states to squize maximum performance
        public static StateCache Cache()
        {
            //statefull and persistent object handled by users, cannot get
            //it from a pool, but they can use a pool for it if they want.
            return new StateCache();
        }
    }

    internal class StateChangeYieldable : IYieldable, IPoolable
    {
        public IEnumerator NextState;

        public void OnYield( ICoroutineEngine engine)
        {
            engine.ReplaceCurrentWith( NextState);
        }

        public void Reset()
        {
            NextState = null;
        }
    }
}
