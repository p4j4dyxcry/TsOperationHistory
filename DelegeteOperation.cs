using System;
using System.Diagnostics;

namespace OperationSystem
{
    public class DelegeteOperation : IOperation
    {
        private readonly Action _execute;
        private readonly Action _rollback;

        public DelegeteOperation( Action execute , Action rollback)
        {
            Debug.Assert(execute != null);
            Debug.Assert(rollback != null);
            _execute = execute;
            _rollback = rollback;            
        }

        public void Execute()
        {
            _execute.Invoke();
        }

        public void Rollback()
        {
            _rollback.Invoke();
        }
    }
}