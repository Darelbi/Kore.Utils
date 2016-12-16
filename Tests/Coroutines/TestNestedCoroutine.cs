// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using UnityEngine;
using System.Collections;

namespace Kore.Coroutines.Tests
{
    public class TestNestedCoroutine : MonoBehaviour
    {
        void Start()
        {
            Coroutine.Run(Test1());
            Coroutine.Run(Test1());
        }

        void Update()
        {
            Debug.Log("Update");
        }

        IEnumerator Test1()
        {
            Debug.Log("A");
            yield return null;
            Debug.Log("B");
            yield return Coroutine.Nested( Nested());
            Debug.Log("C");
            yield return null;
            Debug.Log("D");
        }

        IEnumerator Nested()
        {
            Debug.Log("Nested");
            yield return null;
            Debug.Log("Nested");
            yield return Coroutine.Nested(Nested2()); ;
            Debug.Log("Nested");
            yield return null;
            Debug.Log("Nested");
        }

        IEnumerator Nested2()
        {
            Debug.Log("Nested 2");
            yield return null;
            Debug.Log("Nested 2");
            yield return null;
            Debug.Log("Nested 2");
        }
    }
}
