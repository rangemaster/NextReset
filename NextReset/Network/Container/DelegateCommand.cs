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
        private int _available = 0;
        public AvailableCommand(Action<object, RoutedEventArgs> execute) : this(execute, 0) { }
        public AvailableCommand(Action<object, RoutedEventArgs> execute, int available)
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
        public void SetAvailable(int result)
        { this._available = result; }
        public int GetAvailable { get { return _available; } }
        public bool IsAvailable
        { get { return _available > 0; } }
        public bool Decrees()
        {
            _available--;
            return _available >= 0;
        }
    }
}
