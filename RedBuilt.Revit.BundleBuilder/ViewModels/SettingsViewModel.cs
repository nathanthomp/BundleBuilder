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
        public Command SaveAndNavigateCommand { get; }

        public SettingsViewModel(NavigationState navigationState)
        {
            NavigateBundleViewCommand = new NavigateCommand<BundleViewModel>(navigationState, () => new BundleViewModel(navigationState));
            SaveAndNavigateCommand = new SaveAndNavigateCommand<BundleViewModel>(navigationState, () => new BundleViewModel(navigationState));
        }

        //
        // Add a reference to the Settings Model, try to implement OnPropertyChanged to update the Model automatically
        //
        // TODO:
        //// Bundle Settings
        //public static double WidthMargins => .33;
        //public static double LengthMargins => .33;
        //public static double MaxBundleWidth => 102.0;
        //public static double MaxBundleLength => 288.0;
        //public static int MaxPanelsPerLevel => 1000;
        //// Truck Settings
        //public static double MaxTruckHeight => 96.0;
        //public static double MaxTruckWidth => 288.0;
        //// Project Settings
        //public static Panel StartingPanel;
        //public static string StartingDirection;

    }
}
