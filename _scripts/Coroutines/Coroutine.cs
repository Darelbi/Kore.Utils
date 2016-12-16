// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using Kore.Utils;
using System.Collections;

namespace Kore.Coroutines
{
    /// <summary>
    /// Static class to access special coroutines without having to call each time
    /// "Instance" over CoroutineCore.
    /// </summary>
    public static class Coroutine
    {
        private static MiniPool< CoroutineNestedYieldable> nestedPool = new MiniPool< CoroutineNestedYieldable>( 1);

        public static void Run( IEnumerator enumerator, Method method = Method.Update)
        {
            CoroutineCore.Instance.Run( enumerator, method);
        }

        public static IYieldable Nested( IEnumerator enumerator)
        {
            var item = nestedPool.Current();
            item.Nested = enumerator;
            return item;
        }
    }

    internal class CoroutineNestedYieldable: IYieldable
    {
        public IEnumerator Nested;

        public void OnYield( ICoroutineEngine engine)
        {
            engine.PushOverCurrent( Nested);
        }

        public void Reset()
        {
            Nested = null;
        }
    }
}
