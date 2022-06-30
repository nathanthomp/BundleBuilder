using RedBuilt.Revit.BundleBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        public Command NavigateBundleViewCommand { get; }
        public Command SaveSettingsCommand { get; }

        // Constructor
        public SettingsViewModel(NavigationState navigationState)
        {
            NavigateBundleViewCommand = new NavigateCommand<BundleViewModel>(navigationState, () => new BundleViewModel(navigationState));
            SaveSettingsCommand = new SaveSettingsCommand();
        }

    }
}
