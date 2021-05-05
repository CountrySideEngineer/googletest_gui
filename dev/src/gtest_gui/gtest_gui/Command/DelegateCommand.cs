using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace gtest_gui.Command
{
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// Body of command operation.
        /// </summary>
        private Action execute;

        /// <summary>
        /// Shows whether the command can execute or not.
        /// </summary>
        private Func<bool> canExecute;

        /// <summary>
        /// Constructor.
        /// Create the object with specifying the method called when the Execute method run.
        /// </summary>
        /// <param name="Execute">Action object to exeucte command.</param>
        public DelegateCommand(Action Execute) : this(Execute, () => true) { }
        public DelegateCommand(Action Execute, Func<bool> CanExecute)
        {
            this.execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            this.canExecute = CanExecute ?? throw new ArgumentNullException(nameof(CanExecute));
        }

        /// <summary>
        /// Event to notify the CanExecute method result has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Property of Action to execution.
        /// </summary>
        protected Action Exec
        {
            get { return this.execute; }
            set { this.execute = value; }
        }

        /// <summary>
        /// Property of whether the action can execute.
        /// </summary>
        protected Func<Boolean> CanExec
        {
            get { return this.canExecute; }
            set { this.canExecute = value; }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Boolean CanExecute(object parameter)
        {
            return this.canExecute();
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            this.execute();
        }
    }

    /// <summary>
    /// DelegateCommand class with template data type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _Execute;
        private readonly Predicate<object> _CanExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Boolean CanExecute(object parameter)
        {
            return this._CanExecute == null ? true : this._CanExecute(parameter);
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Parameter of command to execute.</param>
        public void Execute(object parameter)
        {
            this._Execute((T)parameter);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="execute">Action object to execute.</param>
        /// <param name="canExecute">Object indicates the command can execute or not.</param>
        public DelegateCommand(Action<T> execute, Predicate<object> canExecute)
        {
            this._Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._CanExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }
    }
}
