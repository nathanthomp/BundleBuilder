using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Autodesk.Revit.DB;
using RedBuilt.Revit.BundleBuilder.Application.Tools;
using RedBuilt.Revit.BundleBuilder.Commands;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.Services;
using RedBuilt.Revit.BundleBuilder.Data.States;
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;
using Settings = RedBuilt.Revit.BundleBuilder.Data.Models.Settings;

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

        public List<Panel> Panels
        {
            get 
            {
                return Project.Panels; 
            }
            set
            {
                Project.Panels = value;
                NotifyPropertyChanged();
            }
        }
        
    }
}
