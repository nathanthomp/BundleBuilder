using RedBuilt.Revit.BundleBuilder.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBuilt.Revit.BundleBuilder.Application.Sort
{
    public class PanelNameSort
    {
        /// <summary>
        /// Sorts panels based on symbol, instance, then instance and version
        /// </summary>
        /// <param name="panels">panels to sort</param>
        /// <returns>sorted panels</returns>
        public static List<Panel> Sort(List<Panel> panels)
        {
            return panels.OrderBy(x => x.Name.Symbol).ThenBy(x => x.Name.Instance).ThenBy(x => x.Name.InstanceAndVersion).ToList();
        } 


    }
}
