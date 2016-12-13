// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri

using Kore.Utils;
using System.Collections;

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
            var node = _current;
            _current.enumerator = WrapCoroutine( node, nested, toBeRestored);
        }

        private IEnumerator WrapCoroutine( Node node, IEnumerator nested, IEnumerator toBeRestored)
        {
            while ( nested.MoveNext())
                yield return nested.Current;

            node.enumerator = toBeRestored;
            yield return null;
        }

        public void RegisterCustomYield(ICustomYield customYield)
        {
            var toBeRestored = _current.enumerator;
            var node = _current;
            _current.enumerator = CustomYieldCoroutine( node, customYield, toBeRestored);
        }

        private IEnumerator CustomYieldCoroutine( Node node, ICustomYield customYield, IEnumerator toBeRestored)
        {
            while (customYield.HasDone() == false)
            {
                yield return null;
                customYield.Update( _method);
            }

            node.enumerator = toBeRestored;
            yield return null;
        }

        public void ReplaceCurrentWith( IEnumerator nextState)
        {
            _current.enumerator = nextState;
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
                        var y = ( IYieldable)e.Current;
                        y.OnYield( this);
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
