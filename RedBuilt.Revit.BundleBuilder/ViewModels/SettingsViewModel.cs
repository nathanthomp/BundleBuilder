using RedBuilt.Revit.BundleBuilder.Application.Tools;
using RedBuilt.Revit.BundleBuilder.Commands;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.States;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.ViewModels
{
    public class SettingsViewModel : ViewModel, IDataErrorInfo
    {
        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

        public Command NavigateBundleViewCommand { get; }

        public SettingsViewModel(NavigationState navigationState)
        {
            NavigateBundleViewCommand = new NavigateCommand<BundleViewModel>(navigationState, () => new BundleViewModel(navigationState));
        }

        #region Properties

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

        // Plate Settings //
        public int NumberOfLevels2x4 
        { 
            get { return Settings.NumberOfLevels2x4; }
            set
            {
                Settings.NumberOfLevels2x4 = value;
                NotifyPropertyChanged();
            } 
        }

        public int NumberOfLevels2x6
        {
            get { return Settings.NumberOfLevels2x6; }
            set
            {
                Settings.NumberOfLevels2x6 = value;
                NotifyPropertyChanged();
            }
        }

        public int NumberOfLevels2x8
        {
            get { return Settings.NumberOfLevels2x8; }
            set
            {
                Settings.NumberOfLevels2x8 = value;
                NotifyPropertyChanged();
            }
        }

        public int NumberOfLevels2x10
        {
            get { return Settings.NumberOfLevels2x10; }
            set
            {
                Settings.NumberOfLevels2x10 = value;
                NotifyPropertyChanged();
            }
        }

        public int NumberOfLevels2x12
        {
            get { return Settings.NumberOfLevels2x12; }
            set
            {
                Settings.NumberOfLevels2x12 = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Validation

        public string Error { get { return null; } }

        public bool IsBackValid { get; set; } = ProjectState.CanBundle;

        public string this[string propertyName]
        {
            get
            {
                string result = null;

                // Switch for different properties
                switch (propertyName)
                {
                    case "StartingPanel":
                        if (!PanelTools.GetPanelFromName(StartingPanel).ToBundle)
                        {
                            result = "Staring panel must be included in bundles";
                            ProjectState.CanBundle = false;
                        }
                        else
                        {
                            ProjectState.CanBundle = true;
                        }
                            
                        break;

                    case "WidthMargin":
                        if (WidthMargin == 0)
                            result = "Width margin cannot be zero.";
                        if (WidthMargin >= 1)
                            result = "Width margin cannot be greater than or equal to 1.";
                        break;

                    case "LengthMargin":
                        if (LengthMargin == 0)
                            result = "Length margin cannot be zero.";
                        if (LengthMargin >= 1)
                            result = "Length margin cannot be greater than or equal to 1.";
                        break;

                    case "MaxBundleWidth":
                        if (MaxBundleWidth == 0)
                            result = "Max bundle width cannot be zero.";
                        break;

                    case "MaxBundleLength":
                        if (MaxBundleLength == 0)
                            result = "Max bundle length cannot be zero.";
                        break;

                    case "MaxPanelsPerLevel":
                        if (MaxPanelsPerLevel == 0)
                            result = "Max panels per level cannot be zero.";
                        break;

                    case "NumberOfLevels2x4":
                        if (MaxPanelsPerLevel == 0)
                            result = "Levels in a bundle cannot be zero.";
                        break;

                    case "NumberOfLevels2x6":
                        if (MaxPanelsPerLevel == 0)
                            result = "Levels in a bundle cannot be zero.";
                        break;

                    case "NumberOfLevels2x8":
                        if (MaxPanelsPerLevel == 0)
                            result = "Levels in a bundle cannot be zero.";
                        break;

                    case "NumberOfLevels2x10":
                        if (MaxPanelsPerLevel == 0)
                            result = "Levels in a bundle cannot be zero.";
                        break;

                    case "NumberOfLevels2x12":
                        if (MaxPanelsPerLevel == 0)
                            result = "Levels in a bundle cannot be zero.";
                        break;
                }

                if (ErrorCollection.ContainsKey(propertyName))
                {
                    ErrorCollection[propertyName] = result;
                } 
                else if (result != null)
                {
                    ErrorCollection.Add(propertyName, result);
                }
                
                
                if (result == null)
                    IsBackValid = true;
                else
                    IsBackValid = false;

                NotifyPropertyChanged("IsBackValid");
                NotifyPropertyChanged("ErrorCollection");
                return result;
            }
        }

        #endregion
    }
}
