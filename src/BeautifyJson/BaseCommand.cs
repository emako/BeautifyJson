using System;
using System.Windows.Input;

namespace BeautifyJson
{
    internal class BaseCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public bool CanExecute(object? parameter) => true;
        public Action<object?> Action;
        public BaseCommand(Action<object?> action)
        {
            Action = action;
        }
        public void Execute(object? parameter)
        {
            Action?.Invoke(parameter);
        }
    }
}
