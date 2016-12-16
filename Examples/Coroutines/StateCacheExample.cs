using UnityEngine;
using System.Collections;
using Kore.Utils;

namespace Kore.Coroutines.Examples
{
    /// <summary>
    /// This example is to show how to reduce garbage to zero when using
    /// coroutines as StateMachines in order to stop doing pressure over
    /// the GarbageCollector. Look at Debug.Log When playing to see it
    /// in action.
    /// </summary>
    public class StateCacheExample : MonoBehaviour
    {

        IEnumerator A, B; //States

        // Use this for initialization
        void Start()
        {
            // Cache enumerators, NOTE: they can't be resetted (Microsoft choice)
            A = StateA();
            B = StateB();
            Coroutine.Run( A); //Start State Machine

            // Also run a way to detect Input
            Coroutine.Run( InputDetection());
        }

        bool pressed = false;
        IEnumerator InputDetection()
        {
            var keyPressed = new WaitKeyPressed();
            while (true)
            {
                yield return keyPressed;
                pressed = true;
            }
        }

        bool IsInputPressed()
        {
            if (pressed)
            {
                pressed = false;
                return true; //return true just ONCE
            }
            return false;
        }

        IEnumerator StateA()
        {
            // State cache, will live inside the IEnumerator
            var state = State.Cache();

            while (true)
            {
                yield return state.EnterState(() => Debug.Log("Entered A")); // Called once when state is entered

                if (IsInputPressed())
                    yield return state.Change( B, () => Debug.Log("Exited A")); // Called immediatly
            }
        }

        IEnumerator StateB()
        {
            var state = State.Cache();

            while (true)
            {
                yield return state.EnterState(() => Debug.Log("Entered B"));

                if (IsInputPressed())
                    yield return state.Change( A, () => Debug.Log("Exited B"));
            }
        }

    }
}
