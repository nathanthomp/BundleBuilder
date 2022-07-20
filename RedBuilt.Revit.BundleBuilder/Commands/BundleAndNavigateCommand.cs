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
            // Assign panels to project panels only if the ToBundle property is true
            Project.Panels = Project.Panels.Where(x => x.ToBundle = true).ToList();

            // Sort exterior panels by user preference
            Project.Panels = PanelPreferenceSort.Sort(Project.Panels);

            // Sort panels by type then plate
            Dictionary<string, Dictionary<string, List<Panel>>> panelsByTypeThenPlate = PanelTypeAndPlateSort.Sort();

            // Solve list of panels that includes the starting panel
            string startPanelType = PanelTools.GetPanelFromName(Settings.StartingPanel).Type.Name;
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
                {
                    for (int j = 0; j < bundle.Levels.Count; j++)
                    {
                        Level level = bundle.Levels[j];
                        if (level.Panels.Count < 1)
                        {
                            bundle.Remove(level);
                        }
                    }
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

            BundleReport.Export();

            // Update current view
            _navigationState.CurrentViewModel = _createViewModel();
        }
    }
}
