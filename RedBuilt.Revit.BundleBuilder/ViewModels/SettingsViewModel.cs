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

        public SettingsViewModel(NavigationState navigationState)
        {
            NavigateBundleViewCommand = new NavigateCommand<BundleViewModel>(navigationState, () => new BundleViewModel(navigationState));
        }

        // Project Settings //

        public string StartingPanel
        {
            get { return Settings.StartingPanel; }
            set
            {
                Settings.StartingPanel = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> StartingPanels
        {
            get { return Settings.StartingPanels; }
            set
            {
                Settings.StartingPanels = value;
                NotifyPropertyChanged();
            }
        }

        public string StartingDirection
        {
            get { return Settings.StartingDirection; }
            set
            {
                Settings.StartingDirection = value;
                NotifyPropertyChanged();
            }
        }

        public List<string> StartingDirections
        {
            get { return Settings.StartingDirections; }
            set
            {
                Settings.StartingDirections = value;
                NotifyPropertyChanged();
            }
        }

        // Truck Settings //

        //public double MaxTruckHeight
        //{
        //    get { return Settings.MaxTruckHeight; }
        //    set
        //    {
        //        Settings.MaxTruckHeight = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        //public double MaxTruckWidth
        //{
        //    get { return Settings.MaxTruckWidth; }
        //    set
        //    {
        //        Settings.MaxTruckWidth = value;
        //        NotifyPropertyChanged();
        //    }
        //}

        // Bundle Settings //

        public double WidthMargin
        {
            get { return Settings.WidthMargin; }
            set
            {
                Settings.WidthMargin = value;
                NotifyPropertyChanged();
            }
        }

        public double LengthMargin
        {
            get { return Settings.LengthMargin; }
            set
            {
                Settings.LengthMargin = value;
                NotifyPropertyChanged();
            }
        }

        public double MaxBundleWidth
        {
            get { return Settings.MaxBundleWidth; }
            set
            {
                Settings.MaxBundleWidth = value;
                NotifyPropertyChanged();
            }
        }

        public double MaxBundleLength
        {
            get { return Settings.MaxBundleLength; }
            set
            {
                Settings.MaxBundleLength = value;
                NotifyPropertyChanged();
            }
        }

        // Level Settings //

        public int MaxPanelsPerLevel
        {
            get { return Settings.MaxPanelsPerLevel; }
            set
            {
                Settings.MaxPanelsPerLevel = value;
                NotifyPropertyChanged();
            }
        }

    }
}
