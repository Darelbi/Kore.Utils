// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using Kore.Utils;
using System.Collections;

namespace Kore.Coroutines
{
    public class CoroutineCore 
        : SceneScopedSingletonI< CoroutineCore, ICoroutine>, ICoroutine  // Singleton through inheritance
    {
        public override void Init()
        {
            base.Init();
            CreateCoroutineRunners();
            coroutineCoreRunning = true;
        }

        public override void OnDestroyCalled()
        {
            base.OnDestroyCalled();
            coroutineCoreRunning = false;
            updateRunner = null;
            fixedRunner = null;
            lateRunner = null;
        }

        CoroutineEngine updateRunner;
        CoroutineEngine fixedRunner;
        CoroutineEngine lateRunner;

        private void CreateCoroutineRunners()
        {
            updateRunner = new CoroutineEngine( Method.Update);
            fixedRunner = new CoroutineEngine( Method.FixedUpdate);
            lateRunner = new CoroutineEngine( Method.LateUpdate);
        }

        bool coroutineCoreRunning;

        protected void Update()
        {
            if (coroutineCoreRunning)
                updateRunner.Tick();
        }

        protected void FixedUpdate()
        {
            if (coroutineCoreRunning)
                fixedRunner.Tick();
        }

        protected void LateUpdate()
        {
            if (coroutineCoreRunning)
                lateRunner.Tick();
        }

        public void Run( IEnumerator enumerator, Method method)
        {
            switch (method)
            {
                case Method.Update:
                    updateRunner.Run( enumerator);
                    break;

                case Method.FixedUpdate:
                    fixedRunner.Run( enumerator);
                    break;

                case Method.LateUpdate:
                    lateRunner.Run( enumerator);
                    break;
            }
        }
    }
}
