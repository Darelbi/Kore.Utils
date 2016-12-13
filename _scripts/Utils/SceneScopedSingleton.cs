// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using UnityEngine;

// Partially based on: http://wiki.unity3d.com/index.php/Singleton

namespace Kore.Utils
{
    /// <summary>
    /// Singleton through Inheritance. There's also a optional Init() method to be
    /// overrided (because as MonoBehaviour you can't use constructor and if you 
    /// access the instance from within an Awake() method the Awake of the instance 
    /// won't be called. I also provide a replacement OnDestroyCalled.
    /// </summary>
    public class SceneScopedSingletonI< T, I> : MonoBehaviour, IInitiable where T : MonoBehaviour, I
    {
        public virtual void Init()
        {

        }

        public virtual void OnDestroyCalled()
        {

        }

        // since T should be subclass of MonoBehaviour and I, and MonoBehaviour is a class
        // therefore I can only be a interface
        private static I _instance = default(I);

        public static I Instance
        {
            get
            {
                if (_instance == null && _applicationIsQuitting == false)
                {
                    _instance = SingletonInstance_SceneScoped.GO.AddComponent<T>();
                    (_instance as IInitiable).Init();
                    return _instance;
                }

                return _instance;
            }
        }

        private static bool _applicationIsQuitting = false;

        /// <summary>
        /// Forbid to access instance after OnDestroy is called, if some code
        /// access instance then it gets null exception because it is doing obviously
        /// something wrong.
        /// </summary>
        protected void OnDestroy() // protected so user get warning to use "new keyword" if 
                                   // accidentally redeclaring the member:
                                   // Instead the user should override "OnDestroyCalled"
        {
            _applicationIsQuitting = true;
            (_instance as IInitiable).OnDestroyCalled();
            _instance = default( I);
        }
    }

    public class SceneScopedSingleton< T> : MonoBehaviour where T : MonoBehaviour, IInitiable
    {
        public virtual void Init()
        {

        }

        public virtual void OnDestroyCalled()
        {

        }

        private static T _instance = null;

        // flag to avoid null comparation check of monobeahviours (which is very expensive)
        // this actually makes this class one of the fastest Singletons (not necessary on interfaces)
        private static bool _instantiated = false;

        public static T Instance
        {
            get
            {
                if (_instantiated == false && !_applicationIsQuitting)
                {
                    _instantiated = true;
                    _instance = SingletonInstance_SceneScoped.GO.AddComponent< T>();
                    _instance.Init();
                    return _instance;
                }

                return _instance;
            }
        }

        private static bool _applicationIsQuitting = false;

        protected void OnDestroy() // protected so user get warning to use "new keyword" if 
                                   // accidentally redeclaring the member:
                                   // Instead the user should override "OnDestroyCalled"
        {
            _applicationIsQuitting = true;
            _instance.OnDestroyCalled();
            _instance = null;
            _instantiated = false;
        }
    }

    /// <summary>
    /// Internal utility: there will be just 1 GameObject in the scene with
    /// all singletons attached.
    /// </summary>
    internal class SingletonInstance_SceneScoped
    {
        private static bool _instantiated = false;

        internal static GameObject _GO = null;
        internal static GameObject GO
        {
            get
            {
                // Creates only 1 game objects
                if (_instantiated == false)
                {
                    _GO = new GameObject("[Singletons:SceneScoped]");
                    _GO.AddComponent< SingletonInstance_SceneScoped_Resetter>();
                    _instantiated = true;
                }
                
                return _GO;
            }
        }

        internal static void Reset()
        {
            _instantiated = false;
            _GO = null;
        }
    }

    // Reset Instance so that we will instantiate again the GO in the next scene
    internal class SingletonInstance_SceneScoped_Resetter: MonoBehaviour
    {
        private void OnDestroy()
        {
            SingletonInstance_SceneScoped.Reset();
        }
    }
}
