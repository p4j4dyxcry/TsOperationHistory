using System.Collections.Generic;
using System.ComponentModel;

namespace OperationSystem
{
    public interface IOperationManager : INotifyPropertyChanged, IEnumerable<IOperation>
    {
        bool CanUndo { get; }
        bool CanRedo { get; }

        IOperation Peek();
        IOperation Pop();

        IOperation Push(IOperation operation);

        void Undo();
        void Redo();
        void Execute(IOperation operation);
    }
}
