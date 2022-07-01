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
            // When an ExportViewModelCommand gets instanciated, the bundling will be initiated

        }

        public string Header  => "HEADER";
    }
}
