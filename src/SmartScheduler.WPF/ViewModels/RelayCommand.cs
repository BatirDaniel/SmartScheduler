using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SmartScheduler.WPF.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _can;

        public RelayCommand(Action<object?> execute, Predicate<object?>? can = null)
        {
            _execute = execute; _can = can;
        }
        public bool CanExecute(object? p) => _can == null || _can(p);
        public void Execute(object? p) => _execute(p);
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
