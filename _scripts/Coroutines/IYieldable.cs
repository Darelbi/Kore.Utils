// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using Kore.Utils;

namespace Kore.Coroutines
{
    /// <summary>
    /// Interface to allow users implement custom yield instructions
    /// </summary>
    public interface IYieldable: IPoolable
    {
        /// <summary>
        /// Called when the coroutine engine "unpacks" the yield instruction
        /// </summary>
        /// <param name="engine"></param>
        void OnYield( ICoroutineEngine engine);
    }
}
