// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using UnityEngine;
using System.Collections;

namespace Kore.Coroutines.Tests
{
    public class TestCoroutine : MonoBehaviour
    {
        void Start()
        {
            Koroutine.Run(Test("eeee"));
            Koroutine.Run(Test("ffff"));
        }

        IEnumerator Test( string prefix)
        {
            Debug.Log( prefix + "A");
            yield return null;
            Debug.Log( prefix + "B");
            yield return null;
            Debug.Log( prefix + "C");
        }
    }
}
