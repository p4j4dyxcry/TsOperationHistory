using System;

namespace OperationSystem
{
    public interface IPropertyChangeOperation : IOperation
    {
        IOperationMergeJudger MergeJudger { get;set; }
        IOperation Merge(IOperationManager operationManager);
    }

    public class PropertyChangeOperation<T> : IPropertyChangeOperation
    {
        private T PrevProperty { get; set; }
        private T Property { get; }
        private Action<T> Setter { get; }
        public IOperationMergeJudger MergeJudger { get; set; }

        public PropertyChangeOperation(
            Action<T> setter,
            T newValue,
            T oldValue,
            IOperationMergeJudger operationMergeJudger = null)
        {
            Setter = setter;
            PrevProperty = oldValue;
            Property = newValue;
            MergeJudger = operationMergeJudger;
        }

        public void Execute()
        {
            Setter.Invoke(Property);
        }

        public void Rollback()
        {
            Setter.Invoke(PrevProperty);
        }

        /// <summary>
        /// OperationManagerのUndoStackを考慮し、
        /// 重複するOperationだった場合はマージしてからOperationを実行します。
        /// </summary>
        /// <param name="operationManager"></param>
        public void MergeAndExecute(IOperationManager operationManager)
        {
            var mergedOperation = Merge(operationManager);
            operationManager.Execute(mergedOperation);
        }
        /// <summary>
        /// OperationManagerのUndoStackとマージします。
        /// 統合されたOperationはUndosStackから除外されます。
        /// Operationが統合された場合OperationManagerのRedoStackはクリアされます。
        /// </summary>
        /// <param name="operationManager"></param>
        /// <returns></returns>
        public IOperation Merge(IOperationManager operationManager)
        {
            if (operationManager.CanUndo is false)
                return this;

            if (MergeJudger is null)
                return this;

            var topCommand = operationManager.Peek();
            var prevValue = PrevProperty;
            var mergeInfo = MergeJudger;
            while (topCommand is PropertyChangeOperation<T> propertyChangeOperation)
            {
                if (MergeJudger.CanMerge(propertyChangeOperation.MergeJudger) is false)
                    break;
                mergeInfo = propertyChangeOperation.MergeJudger;
                prevValue = propertyChangeOperation.PrevProperty;
                operationManager.Pop();
                topCommand = operationManager.Peek();
            }

            PrevProperty = prevValue;
            MergeJudger = mergeInfo;
            return this;
        }

        public override string ToString()
        {
            return $"Property ={MergeJudger}\n" +
                   $"Value = {Property,0:.00}\n" +
                   $"Prev  = {PrevProperty,0:.00}";
        }
    }
}
