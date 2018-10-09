using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace OperationSystem
{
    public class OperationManager : IOperationManager
    {
        private readonly UndoStack<IOperation> _undoStack;
        public bool CanUndo => _undoStack.CanUndo;

        public bool CanRedo => _undoStack.CanRedo;

        public OperationManager(int capacity)
        {
            _undoStack = new UndoStack<IOperation>(capacity);
        }

        public void Undo()
        {
            if (!CanUndo)
                return;

            PreStackChanged();
            _undoStack.Undo().Rollback();
            OnStackChanged();
        }

        public void Redo()
        {
            if (!CanRedo)
                return;

            PreStackChanged();
            _undoStack.Redo().Execute();
            OnStackChanged();
        }

        public IOperation Push(IOperation operation)
        {
            Debug.Assert(operation != null);
            PreStackChanged();
            _undoStack.Push(operation).Execute();
            OnStackChanged();
            return operation;
        }

        public IOperation Peek()
        {
            return _undoStack.Peek();
        }

        public IOperation Pop()
        {
            PreStackChanged();
            var result = _undoStack.Pop();
            OnStackChanged();
            return result;
        }

        public void Execute(IOperation operation)
        {
            Push(operation).Execute();
        }

        #region Enumrable

        public IEnumerator<IOperation> GetEnumerator()
        {
            return _undoStack.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region PropertyChanged

        private bool _prevCanRedo;
        private bool _prevCanUndo;
        private int _preStackChangedCall;

        public event PropertyChangedEventHandler PropertyChanged;

        private void PreStackChanged()
        {
            Debug.Assert(_preStackChangedCall == 0 , "stack over flow");
            _preStackChangedCall++;
            _prevCanRedo = CanRedo;
            _prevCanUndo = CanUndo;
        }

        private void OnStackChanged()
        {
            Debug.Assert(_preStackChangedCall == 1, "stack over flow");
            _preStackChangedCall--;
            if (_prevCanUndo != CanUndo)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanUndo)));

            if (_prevCanRedo != CanRedo)
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanRedo)));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OperationManager)));
        }


        #endregion
    }
}
