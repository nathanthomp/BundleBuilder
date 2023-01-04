using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Data.Models
{
    public static class Settings
    {
        // Project Settings //

        private static List<Panel> _startingPanels = Project.Panels.Where(x => x.Type.Name.Equals("Exterior")).ToList();

        public static string StartingPanel = _startingPanels[0].ToString();
        public static List<string> StartingPanels = _startingPanels.Select(x => x.ToString()).ToList();

        public static string StartingDirection = "Increasing";
        public static List<string> StartingDirections = new List<string>
        {
            "Increasing",
            "Decreasing"
        };

        // Truck Settings //

        private static double _maxTruckHeight = 96.0;
        public static double MaxTruckHeight { get => _maxTruckHeight; set => _maxTruckHeight = value; }

        private static double _maxTruckWidth = 288.0;
        public static double MaxTruckWidth { get => _maxTruckWidth; set => _maxTruckWidth = value; }

        // Bundle Settings //

        private static double _widthMargin = .33;
        public static double WidthMargin { get => _widthMargin; set => _widthMargin = value; }

        private static double _lengthMargin = .33;
        public static double LengthMargin { get => _lengthMargin; set => _lengthMargin = value; }

        private static double _maxBundleWidth = 97.0;
        public static double MaxBundleWidth { get => _maxBundleWidth; set => _maxBundleWidth = value; }

        private static double _maxBundleLength = 288.0;
        public static double MaxBundleLength { get => _maxBundleLength; set => _maxBundleLength = value; }

        // Level Settings //

        private static int _maxPanelsPerLevel = 1000;
        public static int MaxPanelsPerLevel { get => _maxPanelsPerLevel; set => _maxPanelsPerLevel = value; }

        // Plate Settings //
        private static int _numberOfLevels2x4 = 6;
        public static int NumberOfLevels2x4 { get => _numberOfLevels2x4; set => _numberOfLevels2x4 = value; }

        private static int _numberOfLevels2x6 = 4;
        public static int NumberOfLevels2x6 { get => _numberOfLevels2x6; set => _numberOfLevels2x6 = value; }

        private static int _numberOfLevels2x8 = 3;
        public static int NumberOfLevels2x8 { get => _numberOfLevels2x8; set => _numberOfLevels2x8 = value; }

        private static int _numberOfLevels2x10 = 3;
        public static int NumberOfLevels2x10 { get => _numberOfLevels2x10; set => _numberOfLevels2x10 = value; }

        private static int _numberOfLevels2x12 = 3;
        public static int NumberOfLevels2x12 { get => _numberOfLevels2x12; set => _numberOfLevels2x12 = value; }



    }
}
