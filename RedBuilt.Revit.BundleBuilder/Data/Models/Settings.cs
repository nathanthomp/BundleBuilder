using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public static class Settings
    {
        // Bundle Settings
        public static double WidthMargins => .33;
        public static double LengthMargins => .33;
        public static double MaxBundleWidth => 102.0;
        public static double MaxBundleLength => 288.0;
        public static int MaxPanelsPerLevel => 1000;
        // Truck Settings
        public static double MaxTruckHeight => 96.0;
        public static double MaxTruckWidth => 288.0;
        // Project Settings
        // public static Panel StartingPanel;
        // public static string StartingDirection;


    }
}
