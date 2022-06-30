using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedBuilt.Revit.BundleBuilder.Commands;

namespace RedBuilt.Revit.BundleBuilder.ViewModels
{
    public class BundleViewModel : ViewModel
    {
        public Command NavigateSettingsCommand { get; }
        public Command NavigateExportCommand { get; }

        // Constructor
        public BundleViewModel(NavigationState navigationState)
        {
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navigationState, () => new SettingsViewModel(navigationState));
            NavigateExportCommand = new NavigateCommand<ExportViewModel>(navigationState, () => new ExportViewModel(navigationState));
        }

    }
}
