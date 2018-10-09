using System.Collections.Generic;
using System.Linq;

namespace OperationSystem
{
    public class CompositeOperation : IOperation
    {
        private readonly List<IOperation> _operations ;

        public CompositeOperation(params IOperation[] operations)
        {
            _operations = new List<IOperation>();
            Add(operations);
        }

        public void Add(params IOperation[] operations)
        {
            _operations.AddRange(operations);
        }

        public void Execute()
        {
            foreach (var operation in _operations)
                operation.Execute();
        }

        public void Rollback()
        {
            foreach (var operation in _operations.ToArray().Reverse())
                operation.Rollback();
        }
    }
}
