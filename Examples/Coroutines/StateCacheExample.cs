using UnityEngine;
using System.Collections;

namespace Kore.Coroutines.Examples
{
    /// <summary>
    /// This example is to show how to reduce garbage to zero when using
    /// coroutines as StateMachines in order to stop doing pressure over
    /// the GarbageCollector. Look at Debug.Log When playing to see it
    /// in action. 
    /// 
    /// You have just 2 Coroutines 
    /// One for detecting Input
    /// One is actually running the State machine (using 2 alternating IEnumerators)
    /// </summary>
    public class StateCacheExample : MonoBehaviour
    {

        IEnumerator A, B; //States

        // Use this for initialization
        void Start()
        {
            // Cache enumerators, Garbage generate only when creating a IEnumerator
            // so as long as we keep and reuse the same reference, we stop generating garbage.
            A = StateA();
            B = StateB();
            Koroutine.Run( A); //Start State Machine

            // This coroutine is just for detecting a key pressed.
            Koroutine.Run( InputDetection());
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
                return true; //return true just ONCE (avoid continuos state change)
            }
            return false;
        }

        IEnumerator StateA()
        {
            // State cache, will live inside the IEnumerator
            var state = State.Cache();

            while (true)
            {
                // I Use Lambdas just for showing the state change with a LOG, actually you can use
                // any callback you prefer (And lambdas are anyway optional).
                yield return state.EnterState(() => Debug.Log("Entered A")); // Called once when state is entered

                if (IsInputPressed()) // Go to state B
                    yield return state.Change( B, () => Debug.Log("Exited A"));
            }
        }

        IEnumerator StateB()
        {
            var state = State.Cache();

            while (true)
            {
                yield return state.EnterState(() => Debug.Log("Entered B"));

                if (IsInputPressed()) // Go to state A
                    yield return state.Change( A, () => Debug.Log("Exited B"));
            }
        }

    }
}
