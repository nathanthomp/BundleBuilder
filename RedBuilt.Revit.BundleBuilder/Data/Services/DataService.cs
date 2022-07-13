using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RedBuilt.Revit.BundleBuilder.Data.Models;
using RedBuilt.Revit.BundleBuilder.Data.States;
using Panel = RedBuilt.Revit.BundleBuilder.Data.Models.Panel;

namespace RedBuilt.Revit.BundleBuilder.Data.Services
{
    public class DataService
    {
        public static void Create(Panel panelToMove, int bundleDestination, int levelDestination)
        {
            Bundle bundle = Project.Bundles.Where(x => x.Number == bundleDestination).FirstOrDefault();
            Level level;

            if (bundle.Levels.Count < levelDestination)
            {
                level = new Level(levelDestination);
                level.Add(panelToMove);
                level.Bundle = bundle;
                panelToMove.Bundle = bundle;
                bundle.Add(level);
            }
            else
            {
                level = bundle.Levels.Where(x => x.Number == levelDestination).FirstOrDefault();
                level.Add(panelToMove);
            }
        }

        public static void Update()
        {
            Application.Reports.BundleReport.Export();
            ProjectState.ExportView.createdBundles.Refresh();
        }

        public static void Delete(Panel panel)
        {
            Bundle bundle = panel.Bundle;
            Level level = panel.Level;

            level.Remove(panel);

            if (level.Panels.Count == 0)
            {
                bundle.Remove(level);
                Application.Tools.BundleTools.CorrectBundleLevelNumbers();
            }

            if (bundle.Levels.Count == 0)
            {
                Project.Remove(bundle);
                Application.Tools.BundleTools.CorrectBundleNumbers();
            }
                
        }

    }
}
