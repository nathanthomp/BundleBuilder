using RedBuilt.Revit.BundleBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.ViewModels
{
    public class ExportViewModel : ViewModel
    {
        public Command RestartAndNavigateCommand { get; }
        public Command ExportCommand { get; }

        public ExportViewModel(NavigationState navigationState)
        {
            RestartAndNavigateCommand = new RestartAndNavigateCommand<BundleViewModel>(navigationState, () => new BundleViewModel(navigationState));
            ExportCommand = new ExportCommand();

        }
    }
}
