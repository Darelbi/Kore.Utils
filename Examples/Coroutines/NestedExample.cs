using UnityEngine;
using System.Collections;


// PROBLEMS: NEED TO UPDATE DURING REGULAR COROUTINES
// HAVE TO SUPPORT CUSTOM YIELD INSTRUCTIONS
namespace Kore.Coroutines.Examples
{
    public class NestedExample : MonoBehaviour
    {
        public GameObject cubePrefab;
        public GameObject bulletPrefab;

        void Start()
        {
            Coroutine.Run( Example());
        }

        private void RestartAnimation()
        {
            Coroutine.Run( Example());
        }

        IEnumerator Example()
        {
            var cube = Instantiate( cubePrefab, transform.position, Quaternion.identity) as GameObject;
            
            for(int i=0; i<5; i++)
            {
                float advancement = 0;
                Vector3 pos = transform.position;
                while (advancement < 2)
                {
                    advancement += Time.deltaTime;
                    Vector3 pos2 = pos;
                    pos2.x += advancement;

                    transform.position = pos2;
                    yield return null;
                }

            }
        }

    }
}
