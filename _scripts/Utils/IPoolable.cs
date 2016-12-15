namespace Kore.Utils
{
    public interface IPoolable
    {
        /// <summary>
        /// CONTRACT: each time you obtain a object from the pool Reset
        /// has been called exactly once. It is undefined if that happens
        /// when object is returned to the pool or just before it is given
        /// back to the user.
        /// </summary>
        void Reset();
    }
}
