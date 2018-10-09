using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OperationSystem
{
    public class CapacityStack<T> : IEnumerable<T>
    {
        private readonly LinkedList<T> _collection = new LinkedList<T>();

        public CapacityStack(int capacity) { Capacity = capacity; }

        public int Capacity { get; }

        public int Count => _collection.Count;

        public T Push(T item)
        {
            _collection.AddLast(item);
            if (_collection.Count > Capacity)
                _collection.RemoveFirst();
            return item;
        }
        public T Peek() => _collection.Last();

        public T Pop()
        {
            var item = _collection.Last();
            _collection.RemoveLast();
            return item;
        }

        public void Clear() => _collection.Clear();

        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
    }
}
