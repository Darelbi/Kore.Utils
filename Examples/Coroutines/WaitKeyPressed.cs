namespace Kore.Coroutines.Examples
{
    public class WaitKeyPressed : IYieldable, ICustomYield
    {
        public bool HasDone()
        {
            return UnityEngine.Input.anyKeyDown;
        }

        public void OnYield( ICoroutineEngine engine)
        {
            engine.RegisterCustomYield( this);
        }

        public void Update( Method method)
        {
           
        }
    }
}
