using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Kore.Coroutines.Examples
{
    public class CustomYieldExample : MonoBehaviour
    {
        public GameObject cubePrefab;
        public Text pressAnyKey;

        void Start()
        {
            pressAnyKey.enabled = false;
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
                    cubepos.position = new Vector3(pos.x + advancement, pos.y, pos.z);
                    yield return null;
                }

                //return the custom yield
                pressAnyKey.enabled = true;
                yield return new WaitKeyPressed();
                pressAnyKey.enabled = false;
            }

            Destroy(cube);
            RestartCoroutine();
        }
    }
}
