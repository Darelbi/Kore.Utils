// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using System.Collections;

namespace Kore.Coroutines
{
    public interface ICoroutine
    {
        void Run( IEnumerator enumerator, Method method);
    }
}
