using System;
using System.Windows.Input;

namespace OperationSystem
{
    public class DelegateCommnd : ICommand
    {        
        public bool CanExecute(object parameter)
            => CanExecuteFunc?.Invoke() ?? true;

        public void Execute(object parameter)
            => Action?.Invoke();

        public event EventHandler CanExecuteChanged;

        private Action Action { get; }


        public DelegateCommnd(Action action)
            => Action = action;

        private Func<bool> CanExecuteFunc { get; }
        public DelegateCommnd(Action action,Func<bool> canExecute):this(action)
            => CanExecuteFunc = canExecute;

        public void OnCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public class DelegateCommnd<T> : ICommand 
    {
        public bool CanExecute(object parameter)
            => CanExecuteFunc?.Invoke((T)parameter) ?? true;

        public void Execute(object parameter)
            => Action?.Invoke((T)parameter);

        public event EventHandler CanExecuteChanged;

        private Action<T> Action { get; }

        public DelegateCommnd(Action<T> action)
            => Action = action;

        private Func<T, bool> CanExecuteFunc { get; }
        public DelegateCommnd(Action<T> action, Func<T,bool> canExecute) : this(action)
            => CanExecuteFunc = canExecute;

        public void OnCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

}
