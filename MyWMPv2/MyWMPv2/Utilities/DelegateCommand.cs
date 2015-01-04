using System;
using System.Windows.Input;

namespace MyWMPv2.Utilities
{
    class DelegateCommand : ICommand
    {
        #region Private member variables
        private readonly Action<object> _action;
        #endregion Private member variables

        public DelegateCommand(Action<object> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
