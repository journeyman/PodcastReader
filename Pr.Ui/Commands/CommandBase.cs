using System;
using System.Windows.Input;

namespace Pr.Ui.Commands
{
    public abstract class CommandBase<TParam> : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return CanExecute((TParam)parameter);
        }

        public void Execute(object parameter)
        {
            Execute((TParam)parameter);
        }

        protected virtual bool CanExecute(TParam param)
        {
            return true;
        }

        protected abstract void Execute(TParam param);

        public event EventHandler CanExecuteChanged;
    }
}
