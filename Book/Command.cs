using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Book
{
    public class Command:ICommand
    {
        public Command(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;

        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;

            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        public bool CanExecute(object parameter) 
        {
            if (_canExecute!=null)
            {
                return _canExecute(parameter);
            }
            return true;
        }



        Action<object> _execute;
        Predicate<object> _canExecute;
    }
}
