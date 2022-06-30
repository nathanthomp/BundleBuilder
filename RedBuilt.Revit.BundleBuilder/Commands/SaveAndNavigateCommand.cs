using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder.Commands
{
    public class SaveAndNavigateCommand<T> : Command
            where T : ViewModel
    {
        private readonly NavigationState _navigationState;
        private readonly Func<T> _createViewModel;

        public SaveAndNavigateCommand(NavigationState navigationState, Func<T> createViewModel)
        {
            _navigationState = navigationState;
            _createViewModel = createViewModel;
        }

        public override void Execute(object parameter)
        {

            // This is where we will save the data
            MessageBox.Show("Settings Saved!");
            SaveSettings();

            _navigationState.CurrentViewModel = _createViewModel();
        }

        public static void SaveSettings()
        {

        }
    }
}
