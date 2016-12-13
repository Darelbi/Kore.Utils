// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

namespace Kore.Coroutines
{
    public interface ICustomYield
    {
        void Update( Method method);

        bool HasDone();
    }
}
