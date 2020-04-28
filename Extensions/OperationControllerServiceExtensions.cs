using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using TsOperationHistory.Internal;

namespace TsOperationHistory.Extensions
{
    public static class OperationControllerServiceExtensions
    {
        public static void ExecuteAdd<T>(this IOperationController controller, IList<T> list, T value)
        {
            var operation = list.ToAddOperation(value);
            controller.Execute(operation);
        }
        
        public static void ExecuteInsert<T>(this IOperationController controller, IList<T> list, T value , int index)
        {
            var operation = new InsertOperation<T>(@list, value , index);
            controller.Execute(operation);
        }
        
        public static void ExecuteAddRange<T>(this IOperationController controller, IList<T> list, IEnumerable<T> value )
        {
            var operation = list.ToAddRangeOperation(value);
            controller.Execute(operation);
        }
        
        public static void ExecuteRemove<T>(this IOperationController controller, IList<T> list, T value)
        {
            var operation = list.ToRemoveOperation(value);
            controller.Execute(operation);
        }
        
        public static void ExecuteRemove<T>(this IOperationController controller, IList<T> list, int index)
        {
            if (list is IList iList)
            {
                var operation = iList.ToRemoveAtOperation(index);
                controller.Execute(operation);                
            }
            else
            {
                var target = list[index];
                var operation = list.ToRemoveOperation(target);
                controller.Execute(operation);
            }
        }
        
        public static void ExecuteRemoveItems<T>(this IOperationController controller, IList<T> list, IEnumerable<T> value )
        {
            var operation = list.ToRemoveRangeOperation(value);
            controller.Execute(operation);
        }

        public static void ExecuteSetProperty<T,TProperty>(this IOperationController controller, T owner , string propertyName , TProperty value)
        {
            var operation = owner
                .GenerateSetPropertyOperation(propertyName, value)
                .Merge(controller);
            
            controller.Execute(operation);
        }

        public static IDisposable BindPropertyChanged<T>(this IOperationController controller , INotifyPropertyChanged owner, string propertyName)
        {
            owner.PropertyChanged += PropertyChanged;
            
            return new Disposer(() => owner.PropertyChanged -= PropertyChanged);

            void PropertyChanged(object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName == propertyName)
                {
                    var operation = owner
                        .ToOperation<T>(propertyName)
                        .Merge(controller);
                    
                    controller.Push(operation);
                }
            }
        }
    }
}