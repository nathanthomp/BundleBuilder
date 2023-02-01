using RedBuilt.Revit.BundleBuilder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Application.Sort
{
    public class PanelPreferenceSort
    {
        /// <summary>
        /// Sorts the list of panels based on preference input by user
        /// </summary>
        /// <param name="panelList">panels to sort</param>
        /// <returns>sorted panels</returns>
        public static List<Panel> Sort(List<Panel> panelList)
        {
            List<Panel> extPanels = panelList.Where(x => x.Type.Name.Equals("Exterior") || x.Type.Name.Equals("Steel")).ToList();
            List<Panel> otherPanels = panelList.Where(x => !x.Type.Name.Equals("Exterior") && !x.Type.Name.Equals("Steel")).ToList();

            List<Panel> result = new List<Panel>();

            List<Panel> before = new List<Panel>();
            List<Panel> after = new List<Panel>();

            int counter = 0;
            foreach (Panel panel in extPanels)
            {
                if (panel.Equals(Tools.PanelTools.GetPanelFromName(Settings.StartingPanel)))
                    break;
                counter++;
            }

            if (counter != 0)
                before = extPanels.Take(counter).ToList();

            if (counter != extPanels.Count - 1)
                after = extPanels.Skip(counter + 1).ToList();

            result.Add(extPanels[counter]);
            if (Settings.StartingDirection.Equals("Increasing"))
            {
                result.AddRange(after);
                result.AddRange(before);
            }
            else
            {
                before.Reverse();
                after.Reverse();
                result.AddRange(before);
                result.AddRange(after);
            }

            result.AddRange(otherPanels);
            return result;
        }
    }
}
