using UnityEngine;
using System.Collections;

namespace Kore.Coroutines.Tests
{
    public class TestWaitFor : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            Koroutine.Run( TestEnumerator());
        }

        private IEnumerator TestEnumerator()
        {
            Debug.Log( "Started");
            yield return Wait.For( 5);
            Debug.Log( "Ended");
        }
    }
}
