using UnityEngine;
using System.Collections;

namespace Kore.Coroutines.Tests
{
    public class StressTest : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            Koroutine.Run( TestCoroutine( 300));
        }

        // Update is called once per frame
        IEnumerator TestCoroutine( int i)
        {
            if (i > 0)
            {
                Koroutine.Run( Parallel(2*i));
                yield return Koroutine.Nested( TestCoroutine( i - 1) );
            }
        }

        IEnumerator Parallel( int i)
        {
            for (int j = 0; j < i; j++)
                yield return null;

            yield return Wait.For( 1);

            Debug.Log( "Ended i "+i);
        }
    }
}
