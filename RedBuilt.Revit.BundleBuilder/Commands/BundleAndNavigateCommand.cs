using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder.Commands
{
    public class BundleAndNavigateCommand<T> : Command
                where T : ViewModel
    {
        private readonly NavigationState _navigationState;
        private readonly Func<T> _createViewModel;

        public BundleAndNavigateCommand(NavigationState navigationState, Func<T> createViewModel)
        {
            _navigationState = navigationState;
            _createViewModel = createViewModel;
        }

        public override void Execute(object parameter)
        {
            // This is where we will start the bundle process

            // Project.Panels = Project.Panels.where(x is ToBundle)
            // PanelPreferenceSort
            // Solve

            MessageBox.Show("Bundled!");
            Application.Reports.TestReport.Export();

            // Update current view
            _navigationState.CurrentViewModel = _createViewModel();
        }
    }
}
