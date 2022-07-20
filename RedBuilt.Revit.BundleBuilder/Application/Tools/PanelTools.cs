using RedBuilt.Revit.BundleBuilder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Application.Tools
{
    public class PanelTools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="panelName"></param>
        /// <returns></returns>
        public static Panel GetPanelFromName(string panelName)
        {
            Panel result = null;
            foreach (Panel panel in Project.Panels)
            {
                if (panel.Name.FullName.Equals(panelName))
                {
                    result = panel;
                    break;
                }
            }
            return result;
        }

        //
        // dead code
        //
        public static Panel GetLargestHeightPanelInLevel(List<Panel> panelList)
        {
            Panel result = null;

            double maxHeight = 0;

            foreach (Panel panel in panelList)
            {
                if (panel.Height.AsDouble > maxHeight)
                {
                    maxHeight = panel.Height.AsDouble;
                    result = panel;
                }
            }

            return result;
        }
    }
}
