using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using InteractiveGraphUserControl.MVVM;

namespace InteractiveGraphUserControl.MVVM.Commands
{
    public class AddRowCommand : ICommand
    {
        #region Fields

        // Member variables
        private ViewModel ViewModel;

        #endregion

        #region Constructor

        public AddRowCommand(ViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Whether this command can be executed.
        /// </summary>
        public bool CanExecute(object parameter)
        {
            return (ViewModel.SelectedItem != null);
        }

        /// <summary>
        /// Fires when the CanExecute status of this command changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>   
        /// Invokes this command to perform its intended task.
        /// </summary>
        public void Execute(object parameter)
        {
            ViewModel.DoliInputCollection.Add(new DoliInput(false, 10,"Coucou", 0, "Coucou", 0, "Coucou", 0, "Coucou"));
        }

        #endregion
    }
}

