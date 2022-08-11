using Autodesk.Revit.DB;
using RedBuilt.Revit.BundleBuilder.ViewModels;
using RedBuilt.Revit.BundleBuilder.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.States
{
    public static class ProjectState
    {
        public static Document Doc;

        public static int CurrentBundleNumber { get; set; } = 1;

        public static ExportView ExportView;

        public static MainWindow MainWindow;

        public static readonly string Version = "1.0";

        public static bool CanBundle { get; set; } = true;


    }
}
