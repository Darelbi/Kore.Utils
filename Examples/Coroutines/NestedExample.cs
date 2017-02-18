using UnityEngine;
using System.Collections;

namespace Kore.Coroutines.Examples
{
    public class NestedExample : MonoBehaviour
    {
        public GameObject cubePrefab;
        public GameObject bulletPrefab;

        void Start()
        {
            RestartAnimation();
        }

        private void RestartAnimation()
        {
            Koroutine.Run( Example());
        }

        IEnumerator Example()
        {
            var cube = Instantiate( cubePrefab, transform.position, Quaternion.identity) as GameObject;
            var cubepos = cube.transform;

            for(int i=0; i<5; i++)
            {
                float advancement = 0;
                Vector3 pos = cubepos.position;

                while (advancement < 2)
                {
                    advancement += Time.deltaTime*2;
                    cubepos.position = new Vector3( pos.x+advancement, pos.y, pos.z);
                    yield return null;
                }

                yield return Koroutine.Nested( ShotBullet( cubepos));

            }

            Destroy( cube);
            RestartAnimation();
        }

        private IEnumerator ShotBullet( Transform parent)
        {
            var cube = Instantiate( bulletPrefab, parent.position, Quaternion.identity) as GameObject;
            var cubepos = cube.transform;

            float advancement = 0;
            Vector3 pos = cubepos.position;

            while (advancement < 2)
            {
                advancement += Time.deltaTime*4.5f;
                cubepos.position = new Vector3( pos.x, pos.y - advancement, pos.z);
                yield return null;
            }

            Destroy(cube);
        }
    }
}
