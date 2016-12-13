// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using System.Collections;

namespace Kore.Coroutines
{
    /// <summary>
    /// Static class to access special coroutines without having to call each time
    /// "Instance" over CoroutineCore.
    /// </summary>
    public static class Coroutine
    {
        public static void Run( IEnumerator enumerator, Method method = Method.Update)
        {
            CoroutineCore.Instance.Run( enumerator, method);
        }

        public static IYieldable Nested( IEnumerator enumerator)
        {
            return new CoroutineNestedYieldable( enumerator);
        }
    }

    internal class CoroutineNestedYieldable: IYieldable
    {
        IEnumerator _nested;
        public CoroutineNestedYieldable( IEnumerator nested)
        {
            _nested = nested;
        }

        public void OnYield( ICoroutineEngine engine)
        {
            engine.PushOverCurrent( _nested);
        }
    }
}
