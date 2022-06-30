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
        public Command BundleAndNavigateCommand { get; }

        public BundleViewModel(NavigationState navigationState)
        {
            NavigateSettingsCommand = new NavigateCommand<SettingsViewModel>(navigationState, () => new SettingsViewModel(navigationState));
            BundleAndNavigateCommand = new BundleAndNavigateCommand<ExportViewModel>(navigationState, () => new ExportViewModel(navigationState));
        }



        // TODO:
        // List of panels
        // Project Title
    }
}
