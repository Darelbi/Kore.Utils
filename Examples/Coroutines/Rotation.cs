using UnityEngine;
using System.Collections;

namespace Kore.Coroutines.Examples
{
    public class Rotation : MonoBehaviour
    {
        float angle;

        // Update is called once per frame
        void Update()
        {
            angle += Time.deltaTime * 90;
            if (angle >= 360)
                angle -= 360;

            transform.localRotation = Quaternion.Euler( new Vector3( 0, angle,0));
        }
    }
}
