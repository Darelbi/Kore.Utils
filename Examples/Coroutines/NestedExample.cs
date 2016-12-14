using UnityEngine;
using System.Collections;
using System;

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
            var cubepos = cube.transform;

            for(int i=0; i<5; i++)
            {
                float advancement = 0;
                Vector3 pos = cubepos.position;

                while (advancement < 2)
                {
                    advancement += Time.deltaTime;
                    cubepos.position = new Vector3( pos.x+advancement, pos.y, pos.z);
                    yield return null;
                }

                yield return Coroutine.Nested( ShotBullet());

            }

            Destroy( cube);
            RestartAnimation();
        }

        private IEnumerator ShotBullet()
        {
            throw new NotImplementedException();
        }
    }
}
