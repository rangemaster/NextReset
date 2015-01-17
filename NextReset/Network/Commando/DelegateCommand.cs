using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Network.Commando
{
    public class DelegateCommand
    {
        private readonly Predicate<object> _canExecute = null;
        private readonly Action<object, RoutedEventArgs> _execute = null;
        public event EventHandler CanExecuteChanged;
        public DelegateCommand(Action<object, RoutedEventArgs> execute)
            : this(execute, null)
        { }
        public DelegateCommand(Action<object, RoutedEventArgs> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute(parameter);
        }
        public void Execute()
        { Execute(null, null); }
        public void Execute(object parameter, RoutedEventArgs e)
        {
            _execute(parameter, e);
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}
