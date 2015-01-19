using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Network.Commando
{
    public class AvailableCommand
    {
        private readonly Action<object, RoutedEventArgs> _execute = null;
        private bool _available = false;
        public AvailableCommand(Action<object, RoutedEventArgs> execute) : this(execute, false) { }
        public AvailableCommand(Action<object, RoutedEventArgs> execute, bool available)
        {
            _execute = execute;
            _available = available;
        }
        public void Execute()
        { Execute(null, null); }
        public void Execute(object parameter, RoutedEventArgs e)
        {
            _execute(parameter, e);
        }
        public void SetAvailable(bool result)
        { this._available = result; }
        public bool IsAvailable
        { get { return _available; } }
    }
}
