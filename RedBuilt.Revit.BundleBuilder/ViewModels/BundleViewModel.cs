﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using RedBuilt.Revit.BundleBuilder.Commands;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.Services;
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;

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

        public IEnumerable<Panel> Panels { get; }

        public string Title { get; set; }
    }
}
