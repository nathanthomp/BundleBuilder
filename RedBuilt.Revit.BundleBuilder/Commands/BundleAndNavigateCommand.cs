using RedBuilt.Revit.BundleBuilder.Application.Reports;
using RedBuilt.Revit.BundleBuilder.Application.Solve;
using RedBuilt.Revit.BundleBuilder.Application.Sort;
using RedBuilt.Revit.BundleBuilder.Application.Tools;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RedBuilt.Revit.BundleBuilder.Commands
{
    public class BundleAndNavigateCommand<T> : Command
                where T : ViewModel
    {
        private readonly NavigationState _navigationState;
        private readonly Func<T> _createViewModel;

        public BundleAndNavigateCommand(NavigationState navigationState, Func<T> createViewModel)
        {
            _navigationState = navigationState;
            _createViewModel = createViewModel;
        }

        public override void Execute(object parameter)
        {
            Panel startingPanel = PanelTools.GetPanelFromName(Settings.StartingPanel);

            // Exceptions
            if (!startingPanel.ToBundle)
                throw new Exception("Starting panel cannot be excluded in bundles");

            // Assign panels to project panels only if the ToBundle property is true
            Project.Panels = Project.Panels.Where(x => x.ToBundle == true).ToList();

            // Sort exterior panels by user preference
            Project.Panels = PanelPreferenceSort.Sort(Project.Panels);

            // Sort panels by type then plate
            Dictionary<string, Dictionary<string, List<Panel>>> panelsByTypeThenPlate = PanelTypeAndPlateSort.Sort();

            // Solve list of panels that includes the starting panel
            string startPanelType = startingPanel.Type.Name;
            string startPanelPlate = PanelTools.GetPanelFromName(Settings.StartingPanel).Plate.Description;
            foreach (KeyValuePair<string, Dictionary<string, List<Panel>>> typePlateDict in panelsByTypeThenPlate)
                if (typePlateDict.Key.Equals(startPanelType))
                    foreach (KeyValuePair<string, List<Panel>> platePanelsDict in typePlateDict.Value)
                        if (platePanelsDict.Key.Equals(startPanelPlate))
                        {
                            BundleSolve.Solve(startPanelType, startPanelPlate, platePanelsDict.Value);
                            typePlateDict.Value.Remove(startPanelPlate);
                            break;
                        }

            // Solve the rest of the list of panels
            foreach (KeyValuePair<string, Dictionary<string, List<Panel>>> typePlateDict in panelsByTypeThenPlate)
                foreach (KeyValuePair<string, List<Panel>> platePanelsDict in typePlateDict.Value)
                    BundleSolve.Solve(typePlateDict.Key, platePanelsDict.Key, platePanelsDict.Value);

            // Remove empty levels in bundle
            for (int i = 0; i < Project.Bundles.Count; i++)
            {
                Bundle bundle = Project.Bundles[i];
                while (BundleTools.NumberOfEmptyLevels(bundle) > 0)
                    for (int j = 0; j < bundle.Levels.Count; j++)
                    {
                        Level level = bundle.Levels[j];
                        if (level.Panels.Count < 1)
                            bundle.Remove(level);
                    }
            }

            // Correct the bundle numbers
            for (int i = 0; i < Project.Bundles.Count; i++)
            {
                Bundle bundle = Project.Bundles[i];

                int counter = 1;
                for (int j = bundle.NumberOfLevels - 1; j >= 0; j--)
                {
                    bundle.Levels[j].Number = counter;
                    counter++;
                }
            }

            // Correct the depth and column numbers
            for (int i = 0; i < Project.Bundles.Count; i++)
            {
                Bundle bundle = Project.Bundles[i];

                for (int j = bundle.NumberOfLevels - 1; j >= 0; j--)
                {
                    Level level = bundle.Levels[j];

                    
                    if (level.Panels.Max(x => x.Depth) > 1)
                    {
                        // Correct depth if level has more than 1 depth
                        List<Panel> depthPanels1 = level.Panels.Where(x => x.Depth == 1).ToList();
                        List<Panel> depthPanels2 = level.Panels.Where(x => x.Depth == 2).ToList();

                        double depthLength1 = depthPanels1.Max(x => x.Height.AsDouble);
                        double depthLength2 = depthPanels2.Max(x => x.Height.AsDouble);

                        if (depthLength2 < depthLength1)
                        {
                            // Switch
                            for (int k = 0; k < depthPanels1.Count; k++)
                                depthPanels1[k].Depth = 2;
                            for (int k = 0; k < depthPanels2.Count; k++)
                                depthPanels2[k].Depth = 1;
                        }

                        // Correct column if level has more than 1 depth
                        List<Panel> columnPanelsWithDepthOf1 = level.Panels.Where(x => x.Depth == 1).ToList();
                        List<Panel> columnPanelsWithDepthOf2 = level.Panels.Where(x => x.Depth == 2).ToList();

                        // Take panels with depth of 1 and sort smallest to largest
                        int numberOfColumnsWithDepthOf1 = columnPanelsWithDepthOf1.Count;
                        while (columnPanelsWithDepthOf1.Count > 0)
                        {
                            for (int k = 0; k < numberOfColumnsWithDepthOf1; k++)
                            {
                                // Find the smallest panel
                                double smallestColumnWidth = columnPanelsWithDepthOf1.Min(x => x.Width.AsDouble);
                                Panel panel = columnPanelsWithDepthOf1.Where(x => x.Width.AsDouble == smallestColumnWidth).First();

                                // Make the smallest panel column of k + 1
                                panel.Column = k + 1;

                                // Remove smallest panel from panelsCopy
                                columnPanelsWithDepthOf1.Remove(panel);
                            }
                        }

                        // Take panels with depth of 2 and sort smallest to largest
                        int numberOfColumnsWithDepthOf2 = columnPanelsWithDepthOf2.Count;
                        while (columnPanelsWithDepthOf2.Count > 0)
                        {
                            for (int k = 0; k < numberOfColumnsWithDepthOf2; k++)
                            {
                                // Find the smallest panel
                                double smallestColumnWidth = columnPanelsWithDepthOf2.Min(x => x.Width.AsDouble);
                                Panel panel = columnPanelsWithDepthOf2.Where(x => x.Width.AsDouble == smallestColumnWidth).First();

                                // Make the smallest panel column of k + 1
                                panel.Column = k + 1;

                                // Remove smallest panel from panelsCopy
                                columnPanelsWithDepthOf2.Remove(panel);
                            }
                        }
                    }
                    else
                    {
                        // Correct column if level has 1 depth

                        // Take all panels and sort smallest to largest
                        List<Panel> panelsCopy = new List<Panel>(level.Panels);
                        int numberOfColumns = panelsCopy.Count;
                        while (panelsCopy.Count > 0)
                        {
                            for (int k = 0; k < numberOfColumns; k++)
                            {
                                // Find the smallest panel
                                double smallestColumnWidth = panelsCopy.Min(x => x.Width.AsDouble);
                                Panel panel = panelsCopy.Where(x => x.Width.AsDouble == smallestColumnWidth).First();

                                // Make the smallest panel column of k + 1
                                panel.Column = k + 1;

                                // Remove smallest panel from panelsCopy
                                panelsCopy.Remove(panel);
                            }
                        }

                    }
                }
            }

            // Export the bundle preview
            BundleReport.CreateHtml();

            // Update current view
            _navigationState.CurrentViewModel = _createViewModel();
        }
    }
}
