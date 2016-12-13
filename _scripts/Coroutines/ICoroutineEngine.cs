// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using System.Collections;

namespace Kore.Coroutines
{
    public interface ICoroutineEngine
    {
        void ReplaceCurrentWith( IEnumerator nextState);

        void RegisterCustomYield( ICustomYield customYield);

        void PushOverCurrent( IEnumerator nested);
    }
}
