using System.Collections;
using System.Collections.Generic;

namespace OperationSystem
{
    public class UndoStack<T> : IEnumerable<T>
    {
        private readonly CapacityStack<T> _undoStack;
        private readonly CapacityStack<T> _redoStack;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public UndoStack(int capacity)
        {
            _undoStack = new CapacityStack<T>(capacity);
            _redoStack = new CapacityStack<T>(capacity);
        }

        public T Undo()
        {
            return _redoStack.Push(_undoStack.Pop());
        }

        public T Redo()
        {
            return  _undoStack.Push(_redoStack.Pop());
        }

        public T Peek()
        {
            if (CanUndo is false)
                return default(T);

            return _undoStack.Peek();
        }

        public T Push(T item)
        {
            _redoStack.Clear();
            return _undoStack.Push(item);
        }

        public T Pop()
        {
            _redoStack.Clear();

            if (CanUndo is false)
                return default(T);

            return _undoStack.Pop();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _undoStack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
