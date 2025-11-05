using System;
using System.Windows.Input;

namespace Lab_4_TI.ViewModels
{
    /// <summary>
    /// Команда для связывания с UI
    /// </summary>
    public class RelayCommand : ICommand
    {
        readonly Action<object?> _vipolnit;
        readonly Func<object?, bool>? _mojnoVipolnit;

        public RelayCommand(Action<object?> vipolnit, Func<object?, bool>? mojnoVipolnit = null)
        {
            _vipolnit = vipolnit;
            _mojnoVipolnit = mojnoVipolnit;
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parametr) => _mojnoVipolnit?.Invoke(parametr) ?? true;

        public void Execute(object? parametr) => _vipolnit(parametr);
    }
}