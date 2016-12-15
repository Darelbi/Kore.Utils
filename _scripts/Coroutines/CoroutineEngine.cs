// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using Kore.Utils;
using System.Collections;
using UnityEngine;

namespace Kore.Coroutines
{
    public class CoroutineEngine: ICoroutineEngine, ITickable
    {
        private Method _method;

        public CoroutineEngine( Method method)
        {
            _method = method;
        }

        internal class Node
        {
            public Node _next;
            public IEnumerator enumerator;
        }

        Node _first = null;
        Node _current;
        Node _previous;
        Node _last;
        
        public void PushOverCurrent( IEnumerator nested)
        {
            var toBeRestored = _current.enumerator;
            _current.enumerator = WrapCoroutine( nested, toBeRestored);
        }

        private IEnumerator WrapCoroutine( IEnumerator nested, IEnumerator toBeRestored)
        {
            while ( nested.MoveNext())
                yield return nested.Current;

            _current.enumerator = toBeRestored;
            yield return null;
        }

        public void RegisterCustomYield( ICustomYield customYield)
        {
            var toBeRestored = _current.enumerator;
            _current.enumerator = CustomYieldCoroutine( customYield, toBeRestored);
        }

        private IEnumerator CustomYieldCoroutine( ICustomYield customYield, IEnumerator toBeRestored)
        {
            while (customYield.HasDone() == false)
            {
                yield return null;
                customYield.Update( _method);
            }

            _current.enumerator = toBeRestored;
            yield return null;
        }

        public void ReplaceCurrentWith( IEnumerator nextState)
        {
            _current.enumerator = nextState;
        }

        private void RegisterLegacyCustomYield ( IEnumerator customYield)
        {
            var toBeRestored = _current.enumerator;
            _current.enumerator = LegacyCustomYieldCoroutine( customYield, toBeRestored);
        }

        private IEnumerator LegacyCustomYieldCoroutine( IEnumerator customYield, IEnumerator toBeRestored)
        {
            while (customYield.MoveNext())
                yield return null;

            _current.enumerator = toBeRestored;
            yield return null;
        }

        public void Run( IEnumerator enumerator)
        {
            var node = new Node();
            node.enumerator = enumerator;

            if (_first == null)
            {
                _first = node;
                _last = node;
                return; 
            }

            _last._next = node;
            _last = node;
        }

        public void Tick()
        {
            _current = _first;
            _previous = null;

            while(_current != null)
            {
                var e = _current.enumerator;
                if (e.MoveNext())
                {
                    if (e.Current != null)
                    {
                        if( e.Current is IEnumerator)
                        {
                            // Keep compatibility with Cystom yield instructions (because
                            // those are used in many frameworks, like DOTween)
                            RegisterLegacyCustomYield( (IEnumerator)e.Current);
                            if (e.Current is IYieldable)
                                Debug.LogWarning(
                                            "Warning: implemented both IYieldable and IEnumerator:"
                                           +"you cannot implement both (class: "+ e.GetType().Name+" )");
                        }
                        else
                        {
                            var y = (IYieldable)e.Current;
                            y.OnYield(this);
                        }
                    }
                    _previous = _current;
                    _current = _current._next;
                }
                else
                    RemoveCurrentNode();
            }
        }

        // This one is bugged
        private void RemoveCurrentNode()
        {
            var removed = _current;

            if (removed == _first)
                _first = removed._next;

            if (removed == _last)
                _last = _previous;

            _current = removed._next; // may be null

            if(_previous != null)
                _previous._next = _current;
        }
    }
}
