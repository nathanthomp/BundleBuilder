using RedBuilt.Revit.BundleBuilder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder.Commands
{
    public class RestartAndNavigateCommand<T> : Command
            where T : ViewModel
    {
        private readonly NavigationState _navigationState;
        private readonly Func<T> _createViewModel;

        public RestartAndNavigateCommand(NavigationState navigationState, Func<T> createViewModel)
        {
            _navigationState = navigationState;
            _createViewModel = createViewModel;
        }

        public override void Execute(object parameter)
        {
            // Clear Bundles
            Project.Bundles.Clear();

            MessageBox.Show("Data Cleared!");

            _navigationState.CurrentViewModel = _createViewModel();
        }
    }
}
