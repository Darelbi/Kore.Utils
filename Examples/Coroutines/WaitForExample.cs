using UnityEngine;
using System.Collections;

namespace Kore.Coroutines.Examples
{
    public class WaitForExample : MonoBehaviour
    {
        public GameObject cubePrefab;

        void Start()
        {
            RestartCoroutine();
        }

        private void RestartCoroutine()
        {
            Coroutine.Run( Example());
        }

        IEnumerator Example()
        {
            var cube = Instantiate( cubePrefab, transform.position, Quaternion.identity) as GameObject;
            var cubepos = cube.transform;

            for (int i = 0; i < 5; i++)
            {
                float advancement = 0;
                Vector3 pos = cubepos.position;

                while (advancement < 2)
                {
                    advancement += Time.deltaTime * 2;
                    cubepos.position = new Vector3( pos.x + advancement, pos.y, pos.z);
                    yield return null;
                }

                yield return Wait.For(1);
            }

            Destroy( cube);
            RestartCoroutine();
        }
    }
}
