using UnityEngine;
using System.Collections;
using System;

namespace Kore.Coroutines.Examples
{
    public class StateExample : MonoBehaviour
    {
        public GameObject cubePrefab;

        private GameObject cube;
        float distance = 4;

        void Start()
        {
            cube = Instantiate(cubePrefab, transform.position, Quaternion.identity) as GameObject;

            Koroutine.Run( StateA());
        }

        IEnumerator StateA()
        {
            var cubepos = cube.transform;
            float advancement = 0;
            Vector3 pos = cubepos.position;

            while (advancement < distance)
            {
                advancement += Time.deltaTime * 2;
                cubepos.position = new Vector3(pos.x + advancement, pos.y, pos.z);
                yield return null;
            }

            cubepos.position = new Vector3( pos.x + distance, pos.y, pos.z);

            yield return State.Change( StateB());
        }

        private IEnumerator StateB()
        {
            var cubepos = cube.transform;
            float advancement = 0;
            Vector3 pos = cubepos.position;

            while (advancement < distance)
            {
                advancement += Time.deltaTime * 2;
                cubepos.position = new Vector3( pos.x - advancement, pos.y, pos.z);
                yield return null;
            }

            cubepos.position = new Vector3( pos.x - distance, pos.y, pos.z);

            yield return State.Change( StateA());
        }
    }
}
