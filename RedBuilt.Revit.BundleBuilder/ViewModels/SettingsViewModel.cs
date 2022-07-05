using RedBuilt.Revit.BundleBuilder.Commands;
using RedBuilt.Revit.BundleBuilder.Data.Models;
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
        public Command SaveAndNavigateCommand { get; }

        public SettingsViewModel(NavigationState navigationState)
        {
            NavigateBundleViewCommand = new NavigateCommand<BundleViewModel>(navigationState, () => new BundleViewModel(navigationState));
            SaveAndNavigateCommand = new SaveAndNavigateCommand<BundleViewModel>(navigationState, () => new BundleViewModel(navigationState));
        }

        // TODO:
        // Add a reference to the Settings Model, try to implement OnPropertyChanged to update the Model automatically

        // Project Settings
        public static Panel StartingPanel;
        public static string StartingDirection;

        //// Truck Settings
        //public static double MaxTruckHeight => 96.0;
        //public static double MaxTruckWidth => 288.0;

        // Bundle Settings
        public static double WidthMargins { get; set; } = .33;
        public static double LengthMargins { get; set; } = .33;
        public static double MaxBundleWidth { get; set; } = 102.0;
        public static double MaxBundleLength { get; set; } = 288.0;

        // Level Settings
        public static int MaxPanelsPerLevel { get; set; } = 1000;

    }
}
