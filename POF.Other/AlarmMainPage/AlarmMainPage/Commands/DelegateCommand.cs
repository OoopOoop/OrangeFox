using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AlarmMainPage.Commands
{
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _executeMethod = null;
        private readonly Func<T, bool> _canExecuteMethod = null;
        private bool _isAutomaticRequeryDisabled = false;


        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null, false)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : this(executeMethod, canExecuteMethod, false)
        {
        }


        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod, bool isAutomaticRequeryDisabled)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod is null. Please set executeMethod.");
            }
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
            _isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        }



        public bool CanExecute(T parameter)
        {
            if (_canExecuteMethod != null)
            {
                return _canExecuteMethod(parameter);
            }
            return true;
        }


        public event EventHandler CanExecuteChanged;

        public void Execute(T parameter)
        {
            if (_executeMethod != null)
            {
                _executeMethod(parameter);
            }
        }



        public void RaiseCanExecuteChanged()
        {
            //EventHandler handles = CanExecuteChanged;

            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);



            //if(handles!=null)
            //    handles(this, new PropertyChangedEventArgs(name));
        }



        bool ICommand.CanExecute(object parameter)
        {
            // if T is of value type and the parameter is not
            // set yet, then return false if CanExecute delegate
            // exists, else return true

            if (parameter == null)
            {
                return (_canExecuteMethod == null);
            }
            return CanExecute((T)parameter);
        }

        void ICommand.Execute(object parameter)
        {
            Execute((T)parameter);
        }

      
    }
}
